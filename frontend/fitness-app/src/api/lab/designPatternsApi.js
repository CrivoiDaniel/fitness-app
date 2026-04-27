import { apiFetch } from "../http";

const BASE_URL = import.meta.env.VITE_API_BASE_URL;

const designPatternsApi = {
    chainOfResponsibility: {
        getDemoInfo: (token) => apiFetch(`${BASE_URL}/api/ChainOfResponsibility/demo-info`, { token }),
        assignTrainer: (request, token) => apiFetch(`${BASE_URL}/api/ChainOfResponsibility/assign-trainer`, {
            method: "POST",
            body: request,
            token
        }),
        requestTrainer: (trainerId, message, token) => apiFetch(`${BASE_URL}/api/ChainOfResponsibility/request-trainer/${trainerId}`, {
            method: "POST",
            body: message,
            token
        }),
        getMyRequests: (token) => apiFetch(`${BASE_URL}/api/ChainOfResponsibility/my-requests`, {
            token
        }),
        respondToRequest: (requestId, accept, token) => apiFetch(`${BASE_URL}/api/ChainOfResponsibility/respond-to-request/${requestId}`, {
            method: "POST",
            body: accept,
            token
        })
    }
};

export default designPatternsApi;
