using Microsoft.AspNetCore.Mvc;
using FitnessApp.Application.Mediator;
using System.Collections.Generic;

namespace FitnessApp.API.Controllers.DesignPatterns
{
    [ApiController]
    [Route("api/[controller]")]
    public class MediatorController : ControllerBase
    {
        // Pentru demo, păstrăm o singură cameră de chat în memorie
        private static readonly ChatRoomMediator _chatRoom = new();
        private static bool _initialized = false;

        public MediatorController()
        {
            if (!_initialized)
            {
                // Înregistrăm câțiva participanți fictivi pentru demo
                var trainer = new TrainerChatParticipant(_chatRoom, "Alex Trainer");
                var client = new ClientChatParticipant(_chatRoom, "Ion Client");
                
                _chatRoom.RegisterParticipant(trainer);
                _chatRoom.RegisterParticipant(client);
                
                _initialized = true;
            }
        }

        [HttpGet("messages")]
        public IActionResult GetMessages()
        {
            return Ok(_chatRoom.ChatLog);
        }

        [HttpPost("send")]
        public IActionResult SendMessage([FromBody] ChatRequest request)
        {
            // Simulăm un participant care trimite un mesaj
            // În realitate, am identifica participantul din Token-ul JWT
            IChatParticipant sender;
            if (request.Role == "Trainer")
                sender = new TrainerChatParticipant(_chatRoom, request.From);
            else
                sender = new ClientChatParticipant(_chatRoom, request.From);

            _chatRoom.RegisterParticipant(sender);
            sender.Send(request.Message);

            return Ok(new { Status = "Sent", Message = request.Message });
        }
    }

    public class ChatRequest
    {
        public string From { get; set; } = "";
        public string Role { get; set; } = ""; // Client sau Trainer
        public string Message { get; set; } = "";
    }
}
