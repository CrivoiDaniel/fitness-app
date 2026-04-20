using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using FitnessApp.Domain.Entities.Workouts;
using FitnessApp.Application.Features.Workouts.Command;
using FitnessApp.Application.Memento;
using FitnessApp.Domain.Enums;
using FitnessApp.Application.Interfaces.Admin.Clients;
using FitnessApp.Application.Interfaces.Workout;
using FitnessApp.Application.Interfaces.Repositories.Users;
using System.Security.Claims;
using System.Linq;

namespace FitnessApp.API.Controllers.Workouts;

[ApiController]
[Authorize(Roles = "Trainer")]
[Route("api/trainer/workout-editor")]
public class WorkoutEditorController : ControllerBase
{
    private readonly IClientQueryService _clientQueryService;
    private readonly IWorkoutPlanRepository _workoutPlanRepository;
    private readonly ITrainerRepository _trainerRepository;

    // Pentru demo, folosim o instanță statică pentru a păstra starea de Undo/Redo
    private static WorkoutPlan? _activePlan;
    private static WorkoutPlanEditor? _editor;
    private static WorkoutPlanCaretaker? _caretaker;

    public WorkoutEditorController(
        IClientQueryService clientQueryService,
        IWorkoutPlanRepository workoutPlanRepository,
        ITrainerRepository trainerRepository)
    {
        _clientQueryService = clientQueryService;
        _workoutPlanRepository = workoutPlanRepository;
        _trainerRepository = trainerRepository;
        
        if (_editor == null)
        {
            _editor = new WorkoutPlanEditor();
        }

        if (_caretaker == null)
        {
            _caretaker = new WorkoutPlanCaretaker();
        }
    }

    [HttpGet("clients")]
    public async Task<IActionResult> GetClients()
    {
        var clients = await _clientQueryService.GetAllAsync();
        return Ok(clients);
    }

    [HttpPost("start-session/{clientId}")]
    public async Task<IActionResult> StartSession(int clientId, [FromQuery] string planName)
    {
        var client = await _clientQueryService.GetByIdAsync(clientId);
        if (client == null) return NotFound("Client not found");

        // Obținem ID-ul antrenorului curent
        var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out int userId))
            return Unauthorized();

        var trainer = await _trainerRepository.GetByUserIdAsync(userId);
        if (trainer == null) return BadRequest("Only registered trainers can start a session");

        // Verificăm dacă există deja un plan pentru acest client și antrenor
        var existingPlans = await _workoutPlanRepository.GetByClientIdAsync(clientId);
        var existingPlan = existingPlans
            .Where(p => p.TrainerId == trainer.Id)
            .OrderByDescending(p => p.CreatedAt)
            .FirstOrDefault();

        if (existingPlan != null)
        {
             // Încărcăm planul existent (cu detalii)
             _activePlan = await _workoutPlanRepository.GetByIdWithDetailsAsync(existingPlan.Id);
             _editor = new WorkoutPlanEditor(); // Resetăm Undo/Redo pentru noua sesiune de editare
             _caretaker = new WorkoutPlanCaretaker(); // Resetăm punctele de control
             return Ok(new { Message = $"Restored existing session for {client.FirstName} {client.LastName}", IsResume = true });
        }

        _activePlan = new WorkoutPlan(
            planName, 
            clientId, 
            WorkoutGoal.MuscleGain, 
            DifficultyLevel.Intermediate, 
            12, 
            DayOfWeekFlag.Monday | DayOfWeekFlag.Wednesday | DayOfWeekFlag.Friday, 
            3, 
            60);
        
        _activePlan.AssignTrainer(trainer.Id);
            
        _editor = new WorkoutPlanEditor();
        _caretaker = new WorkoutPlanCaretaker();
        
        return Ok(new { Message = $"New session started for {client.FirstName} {client.LastName}", IsResume = false });
    }

    [HttpPost("save")]
    public async Task<IActionResult> Save()
    {
        if (_activePlan == null) return BadRequest("No active session to save");

        try 
        {
            if (_activePlan.Id == 0)
            {
                await _workoutPlanRepository.AddAsync(_activePlan);
            }
            else 
            {
                await _workoutPlanRepository.UpdateAsync(_activePlan);
            }
            
            return Ok(new { Message = "Plan saved successfully to database" });
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error while saving: {ex.Message}");
        }
    }

    [HttpGet("state")]
    public IActionResult GetCurrentState()
    {
        return Ok(new
        {
            PlanName = _activePlan?.Name,
            ExerciseCount = _activePlan?.Exercises.Count,
            Exercises = _activePlan?.Exercises,
            UndoHistory = _editor?.GetUndoHistory(),
            RedoHistory = _editor?.GetRedoHistory(),
            CanUndo = _editor?.CanUndo,
            CanRedo = _editor?.CanRedo,
            Checkpoints = _caretaker?.GetAllMementos().Select(m => new { m.Name, m.CreatedAt })
        });
    }

    [HttpPost("add-exercise")]
    public async Task<IActionResult> AddExercise([FromQuery] string name, [FromQuery] int sets, [FromQuery] int reps)
    {
        var exercise = new WorkoutExercise(name, sets, reps, (_activePlan?.Exercises.Count ?? 0) + 1);
        var command = new AddExerciseCommand(_activePlan!, exercise);
        
        await _editor!.ExecuteCommandAsync(command);
        
        return Ok(new { Message = $"Added {name}", NewCount = _activePlan!.Exercises.Count });
    }

    [HttpPost("update-sets")]
    public async Task<IActionResult> UpdateSets([FromQuery] int index, [FromQuery] int newSets, [FromQuery] int newReps)
    {
        var exercises = new List<WorkoutExercise>(_activePlan!.Exercises);
        if (index < 0 || index >= exercises.Count) return BadRequest("Invalid index");

        var exercise = exercises[index];
        var command = new UpdateExerciseCommand(exercise, newSets, newReps);
        
        await _editor!.ExecuteCommandAsync(command);
        
        return Ok(new { Message = $"Updated {exercise.ExerciseName}", NewSets = exercise.Sets });
    }

    [HttpPost("undo")]
    public async Task<IActionResult> Undo()
    {
        if (!_editor!.CanUndo) return BadRequest("Nothing to undo");
        
        await _editor.UndoAsync();
        return Ok(new { Message = "Undo successful", NewCount = _activePlan!.Exercises.Count });
    }

    [HttpPost("redo")]
    public async Task<IActionResult> Redo()
    {
        if (!_editor!.CanRedo) return BadRequest("Nothing to redo");
        
        await _editor.RedoAsync();
        return Ok(new { Message = "Redo successful", NewCount = _activePlan!.Exercises.Count });
    }

    [HttpDelete("reset")]
    public IActionResult Reset()
    {
        _activePlan = null;
        _editor = null;
        _caretaker = null;
        return Ok(new { Message = "Lab session reset" });
    }

    // ========== MEMENTO ENDPOINTS ==========

    [HttpPost("checkpoint")]
    public IActionResult CreateCheckpoint([FromQuery] string name)
    {
        if (_activePlan == null) return BadRequest("No active session");
        
        var memento = _activePlan.Save(name);
        _caretaker!.AddMemento(memento);
        
        return Ok(new { Message = $"Checkpoint '{name}' created successfully" });
    }

    [HttpPost("load-checkpoint/{index}")]
    public IActionResult LoadCheckpoint(int index)
    {
        if (_activePlan == null) return BadRequest("No active session");
        
        var memento = _caretaker!.GetMemento(index);
        if (memento == null) return NotFound("Checkpoint not found");
        
        _activePlan.Restore(memento);
        
        // Când restaurăm tot planul, istoria de Undo/Redo devine invalidă
        _editor = new WorkoutPlanEditor(); 
        
        return Ok(new { Message = $"Restored to checkpoint: {memento.Name}" });
    }
}
