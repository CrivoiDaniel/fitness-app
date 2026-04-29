namespace FitnessApp.Application.Mediator
{
    public interface IChatMediator
    {
        void SendMessage(string message, IChatParticipant sender);
        void RegisterParticipant(IChatParticipant participant);
    }
}
