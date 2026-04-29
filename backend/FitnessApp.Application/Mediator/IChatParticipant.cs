namespace FitnessApp.Application.Mediator
{
    public interface IChatParticipant
    {
        string Name { get; }
        string Role { get; }
        void Receive(string message, string from);
        void Send(string message);
    }
}
