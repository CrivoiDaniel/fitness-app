using FitnessApp.Domain.Entities.Users;
using FitnessApp.Domain.Entities.Workouts;
using FitnessApp.Domain.Enums;


namespace FitnessApp.Domain.Builders;

/// <summary>
/// Builder Pattern for WorkoutPlan
/// Constructs complex workout plans step by step with validation
/// NO PRESETS - All data comes from API requests
/// </summary>
public class WorkoutPlanBuilder
{
    // Required parameters
    private string? _name;
    private int _clientId;
    private WorkoutGoal _goal;
    private DifficultyLevel _difficulty;
    private int _durationWeeks;
    private DayOfWeekFlag _workoutDays;
    private int _sessionsPerWeek;
    private int _sessionDurationMinutes;
    
    // Optional parameters
    private int? _trainerId;
    private string? _description;
    private int? _restDays;
    private string? _specialNotes;
    
    // Exercises list
    private List<ExerciseDefinition> _exercises = new();
    
    // References (optional)
    private Client? _client;
    private Trainer? _trainer;
    
    public WorkoutPlanBuilder()
    {
        Reset();
    }
    
    public WorkoutPlanBuilder Reset()
    {
        _name = null;
        _clientId = 0;
        _goal = WorkoutGoal.GeneralFitness;
        _difficulty = DifficultyLevel.Beginner;
        _durationWeeks = 0;
        _workoutDays = DayOfWeekFlag.None;
        _sessionsPerWeek = 0;
        _sessionDurationMinutes = 0;
        _trainerId = null;
        _description = null;
        _restDays = null;
        _specialNotes = null;
        _exercises = new List<ExerciseDefinition>();
        _client = null;
        _trainer = null;
        return this;
    }
    
    // ========== REQUIRED CONFIGURATION ==========
    
    public WorkoutPlanBuilder WithName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name cannot be empty", nameof(name));
        
        _name = name;
        return this;
    }
    
    public WorkoutPlanBuilder ForClient(int clientId)
    {
        if (clientId <= 0)
            throw new ArgumentException("ClientId must be positive", nameof(clientId));
        
        _clientId = clientId;
        return this;
    }
    
    public WorkoutPlanBuilder ForClient(Client client)
    {
        if (client == null)
            throw new ArgumentNullException(nameof(client));
        
        _clientId = client.Id;
        _client = client;
        return this;
    }
    
    public WorkoutPlanBuilder WithGoal(WorkoutGoal goal)
    {
        _goal = goal;
        return this;
    }
    
    public WorkoutPlanBuilder WithDifficulty(DifficultyLevel difficulty)
    {
        _difficulty = difficulty;
        return this;
    }
    
    public WorkoutPlanBuilder WithDuration(int weeks)
    {
        if (weeks <= 0)
            throw new ArgumentException("Duration must be at least 1 week", nameof(weeks));
        
        _durationWeeks = weeks;
        return this;
    }
    
    public WorkoutPlanBuilder OnDays(DayOfWeekFlag days)
    {
        _workoutDays = days;
        _sessionsPerWeek = CountSetBits((int)days);
        return this;
    }
    
    public WorkoutPlanBuilder OnDays(params DayOfWeek[] days)
    {
        if (days == null || days.Length == 0)
            throw new ArgumentException("At least one day must be specified", nameof(days));
        
        _workoutDays = DayOfWeekFlag.None;
        foreach (var day in days)
        {
            _workoutDays |= ConvertToDayOfWeekFlag(day);
        }
        
        _sessionsPerWeek = days.Length;
        return this;
    }
    
    public WorkoutPlanBuilder WithSessionDuration(int minutes)
    {
        if (minutes <= 0)
            throw new ArgumentException("Session duration must be positive", nameof(minutes));
        
        _sessionDurationMinutes = minutes;
        return this;
    }
    
    // ========== OPTIONAL CONFIGURATION ==========
    
    public WorkoutPlanBuilder WithTrainer(int trainerId)
    {
        if (trainerId <= 0)
            throw new ArgumentException("TrainerId must be positive", nameof(trainerId));
        
        _trainerId = trainerId;
        return this;
    }
    
    public WorkoutPlanBuilder WithTrainer(Trainer trainer)
    {
        if (trainer == null)
            throw new ArgumentNullException(nameof(trainer));
        
        _trainerId = trainer.Id;
        _trainer = trainer;
        return this;
    }
    
    public WorkoutPlanBuilder WithDescription(string description)
    {
        _description = description;
        return this;
    }
    
    public WorkoutPlanBuilder WithRestDays(int days)
    {
        if (days < 0 || days > 7)
            throw new ArgumentException("Rest days must be between 0 and 7", nameof(days));
        
        _restDays = days;
        return this;
    }
    
    public WorkoutPlanBuilder WithNotes(string notes)
    {
        _specialNotes = notes;
        return this;
    }
    
    // ========== EXERCISES ==========
    
    public WorkoutPlanBuilder AddExercise(string name, int sets, int reps, int? durationSeconds = null, string? notes = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Exercise name cannot be empty", nameof(name));
        
        if (sets <= 0)
            throw new ArgumentException("Sets must be positive", nameof(sets));
        
        if (reps <= 0)
            throw new ArgumentException("Reps must be positive", nameof(reps));
        
        _exercises.Add(new ExerciseDefinition
        {
            Name = name,
            Sets = sets,
            Reps = reps,
            DurationSeconds = durationSeconds,
            Notes = notes
        });
        
        return this;
    }
    
    // ========== BUILD METHOD ==========
    
    public WorkoutPlan Build()
    {
        Validate();
        
        var workoutPlan = new WorkoutPlan(
            name: _name!,
            clientId: _clientId,
            goal: _goal,
            difficulty: _difficulty,
            durationWeeks: _durationWeeks,
            workoutDays: _workoutDays,
            sessionsPerWeek: _sessionsPerWeek,
            sessionDurationMinutes: _sessionDurationMinutes
        );
        
        if (!string.IsNullOrWhiteSpace(_description))
            workoutPlan.SetDescription(_description);
        
        if (_trainerId.HasValue)
            workoutPlan.AssignTrainer(_trainerId.Value);
        
        if (_restDays.HasValue)
            workoutPlan.SetRestDays(_restDays.Value);
        
        if (!string.IsNullOrWhiteSpace(_specialNotes))
            workoutPlan.AddSpecialNotes(_specialNotes);
        
        int orderIndex = 1;
        foreach (var exercise in _exercises)
        {
            var workoutExercise = new WorkoutExercise(
                workoutPlanId: 0,
                exerciseName: exercise.Name,
                sets: exercise.Sets,
                reps: exercise.Reps,
                orderInWorkout: orderIndex++
            );
            
            if (exercise.DurationSeconds.HasValue)
                workoutExercise.SetDuration(exercise.DurationSeconds.Value);
            
            if (!string.IsNullOrWhiteSpace(exercise.Notes))
                workoutExercise.AddNotes(exercise.Notes);
            
            workoutPlan.AddExercise(workoutExercise);
        }
        
        return workoutPlan;
    }
    
    private void Validate()
    {
        var errors = new List<string>();
        
        if (string.IsNullOrWhiteSpace(_name))
            errors.Add("Name is required");
        
        if (_clientId <= 0)
            errors.Add("Client is required");
        
        if (_durationWeeks <= 0)
            errors.Add("Duration must be at least 1 week");
        
        if (_workoutDays == DayOfWeekFlag.None)
            errors.Add("At least one workout day must be selected");
        
        if (_sessionsPerWeek <= 0)
            errors.Add("Sessions per week must be at least 1");
        
        if (_sessionDurationMinutes <= 0)
            errors.Add("Session duration must be positive");
        
        if (_exercises.Count == 0)
            errors.Add("At least one exercise must be added");
        
        if (errors.Any())
        {
            throw new InvalidOperationException(
                $"Cannot build WorkoutPlan. Validation errors:\n- {string.Join("\n- ", errors)}"
            );
        }
    }
    
    private DayOfWeekFlag ConvertToDayOfWeekFlag(DayOfWeek day)
    {
        return day switch
        {
            DayOfWeek.Monday => DayOfWeekFlag.Monday,
            DayOfWeek.Tuesday => DayOfWeekFlag.Tuesday,
            DayOfWeek.Wednesday => DayOfWeekFlag.Wednesday,
            DayOfWeek.Thursday => DayOfWeekFlag.Thursday,
            DayOfWeek.Friday => DayOfWeekFlag.Friday,
            DayOfWeek.Saturday => DayOfWeekFlag.Saturday,
            DayOfWeek.Sunday => DayOfWeekFlag.Sunday,
            _ => DayOfWeekFlag.None
        };
    }
    
    private int CountSetBits(int n)
    {
        int count = 0;
        while (n > 0)
        {
            count += n & 1;
            n >>= 1;
        }
        return count;
    }
    
    private class ExerciseDefinition
    {
        public string Name { get; set; } = string.Empty;
        public int Sets { get; set; }
        public int Reps { get; set; }
        public int? DurationSeconds { get; set; }
        public string? Notes { get; set; }
    }
}