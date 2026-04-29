using FitnessApp.Application.ChainOfResponsibility.TrainerAssignment;
using FitnessApp.Application.Interfaces.Repositories.Users;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FitnessApp.Application.Services.Users;
using System.Security.Claims;

using Microsoft.AspNetCore.Authorization;

namespace FitnessApp.API.Controllers.DesignPatterns
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ChainOfResponsibilityController : ControllerBase
    {
        private readonly ITrainerRepository _trainerRepository;
        private readonly TrainerAssignmentService _assignmentService;
        private readonly TrainerRequestService _requestService;

        public ChainOfResponsibilityController(
            ITrainerRepository trainerRepository, 
            TrainerAssignmentService assignmentService,
            TrainerRequestService requestService)
        {
            _trainerRepository = trainerRepository;
            _assignmentService = assignmentService;
            _requestService = requestService;
        }

        [HttpPost("request-trainer/{trainerId}")]
        public async Task<IActionResult> RequestTrainer(int trainerId, [FromBody] string? message)
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdStr)) return Unauthorized();
            
            var requestId = await _requestService.SendRequestAsync(int.Parse(userIdStr), trainerId, message);
            return Ok(new { Message = "Assignment request sent successfully.", RequestId = requestId });
        }

        [HttpGet("my-requests")]
        public async Task<IActionResult> GetMyRequests()
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdStr)) return Unauthorized();

            var requests = await _requestService.GetTrainerRequestsAsync(int.Parse(userIdStr));
            return Ok(requests.Select(r => new {
                r.Id,
                ClientName = r.Client.User.GetFullName(),
                r.Message,
                r.Status,
                r.CreatedAt
            }));
        }

        [HttpPost("mark-under-review/{requestId}")]
        public async Task<IActionResult> MarkUnderReview(int requestId)
        {
            await _requestService.MarkAsUnderReviewAsync(requestId);
            return Ok(new { Message = "Request is now under review." });
        }

        public class ResponseDto
        {
            public bool Accept { get; set; }
            public string? Reason { get; set; }
        }

        [HttpPost("respond-to-request/{requestId}")]
        public async Task<IActionResult> RespondToRequest(int requestId, [FromBody] ResponseDto response)
        {
            await _requestService.RespondToRequestAsync(requestId, response.Accept, response.Reason);
            return Ok(new { Message = response.Accept ? "Request accepted." : "Request rejected." });
        }

        [HttpPost("assign-trainer")]
        public async Task<IActionResult> AssignTrainer([FromBody] AssignmentRequest request)
        {
            // 1. Get all trainers from DB
            var allTrainers = await _trainerRepository.GetAllAsync();

            // 2. Use the Chain of Responsibility to filter them
            var matchingTrainers = _assignmentService.FindTrainers(request, allTrainers);

            // 3. Return the results
            return Ok(new
            {
                Message = "Chain of Responsibility executed successfully.",
                Request = request,
                FoundCount = matchingTrainers.Count(),
                Results = matchingTrainers.Select(t => new
                {
                    t.Id,
                    FullName = t.User?.GetFullName() ?? "Unknown",
                    t.Specialization,
                    t.YearsOfExperience,
                    t.Rating
                })
            });
        }

        [HttpGet("demo-info")]
        public IActionResult GetDemoInfo()
        {
            return Ok(new
            {
                Pattern = "Chain of Responsibility",
                Scenario = "Trainer Assignment Filtering",
                Handlers = new[] { "SpecializationHandler", "ExperienceHandler", "RatingHandler" },
                Description = "This demo shows how a request to find a trainer is passed through a chain of filters. Each handler decides if it should filter the list further based on the request criteria."
            });
        }
    }
}
