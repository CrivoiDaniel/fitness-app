using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FitnessApp.Application.Interfaces.Repositories.Users;
using FitnessApp.Domain.Entities.Users;

namespace FitnessApp.Application.Services.Users
{
    public class TrainerRequestService
    {
        private readonly IClientRepository _clientRepository;
        private readonly ITrainerRequestRepository _requestRepository;

        public TrainerRequestService(
            IClientRepository clientRepository,
            ITrainerRequestRepository requestRepository)
        {
            _clientRepository = clientRepository;
            _requestRepository = requestRepository;
        }

        public async Task<TrainerRequest> SendRequestAsync(int clientUserId, int trainerId, string? message)
        {
            var client = await _clientRepository.GetByUserIdAsync(clientUserId);
            if (client == null) throw new Exception("Client not found");

            var request = new TrainerRequest(client.Id, trainerId, message);
            return await _requestRepository.AddAsync(request);
        }

        public async Task<List<TrainerRequest>> GetTrainerRequestsAsync(int trainerUserId)
        {
            return await _requestRepository.GetPendingByTrainerUserIdAsync(trainerUserId);
        }

        public async Task RespondToRequestAsync(int requestId, bool accept)
        {
            var request = await _requestRepository.GetByIdAsync(requestId);
            if (request == null) throw new Exception("Request not found");

            if (accept)
            {
                request.Accept();
                // Optionally: link client to trainer here if your domain allows it
            }
            else
            {
                request.Reject();
            }

            await _requestRepository.UpdateAsync(request);
        }
    }
}
