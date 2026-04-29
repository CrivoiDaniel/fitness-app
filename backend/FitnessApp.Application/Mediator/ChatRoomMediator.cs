using System.Collections.Generic;
using System.Linq;

namespace FitnessApp.Application.Mediator
{
    public class ChatRoomMediator : IChatMediator
    {
        private readonly List<IChatParticipant> _participants = new();
        public List<ChatMessageDto> ChatLog { get; } = new();

        public void RegisterParticipant(IChatParticipant participant)
        {
            if (!_participants.Contains(participant))
            {
                _participants.Add(participant);
            }
        }

        public void SendMessage(string message, IChatParticipant sender)
        {
            // Logica Mediatorului: decide cine primește mesajul
            // În acest caz, trimitem mesajul tuturor CELORLALȚI participanți
            foreach (var participant in _participants.Where(p => p != sender))
            {
                participant.Receive(message, sender.Name);
            }

            // Salvăm în log pentru demo
            ChatLog.Add(new ChatMessageDto 
            { 
                From = sender.Name, 
                Role = sender.Role, 
                Content = message,
                Timestamp = System.DateTime.Now.ToString("HH:mm")
            });
        }
    }

    public class ChatMessageDto
    {
        public string From { get; set; } = "";
        public string Role { get; set; } = "";
        public string Content { get; set; } = "";
        public string Timestamp { get; set; } = "";
    }
}
