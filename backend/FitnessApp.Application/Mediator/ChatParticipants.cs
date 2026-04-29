using System;

namespace FitnessApp.Application.Mediator
{
    public abstract class BaseChatParticipant : IChatParticipant
    {
        protected IChatMediator _mediator;
        public string Name { get; }
        public string Role { get; }

        protected BaseChatParticipant(IChatMediator mediator, string name, string role)
        {
            _mediator = mediator;
            Name = name;
            Role = role;
        }

        public abstract void Receive(string message, string from);

        public void Send(string message)
        {
            Console.WriteLine($"{Name} ({Role}) trimite: {message}");
            _mediator.SendMessage(message, this);
        }
    }

    // Participant de tip Client
    public class ClientChatParticipant : BaseChatParticipant
    {
        public ClientChatParticipant(IChatMediator mediator, string name) 
            : base(mediator, name, "Client") { }

        public override void Receive(string message, string from)
        {
            // Aici ar fi logica de notificare a clientului în UI sau pe mobil
            Console.WriteLine($"[Client UI - {Name}] Mesaj nou de la {from}: {message}");
        }
    }

    // Participant de tip Antrenor
    public class TrainerChatParticipant : BaseChatParticipant
    {
        public TrainerChatParticipant(IChatMediator mediator, string name) 
            : base(mediator, name, "Trainer") { }

        public override void Receive(string message, string from)
        {
            Console.WriteLine($"[Trainer Dashboard - {Name}] Mesaj primit de la un client ({from}): {message}");
        }
    }
}
