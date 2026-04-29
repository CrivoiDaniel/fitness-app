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
        markAsUnderReview: (requestId, token) => apiFetch(`${BASE_URL}/api/ChainOfResponsibility/mark-under-review/${requestId}`, {
            method: "POST",
            token
        }),
        respondToRequest: (requestId, accept, reason, token) => apiFetch(`${BASE_URL}/api/ChainOfResponsibility/respond-to-request/${requestId}`, {
            method: "POST",
            body: { accept, reason },
            token
        })
    },
    mediator: {
        getMessages: () => apiFetch(`${BASE_URL}/api/Mediator/messages`),
        sendMessage: (from, role, message) => apiFetch(`${BASE_URL}/api/Mediator/send`, {
            method: "POST",
            body: { from, role, message }
        })
    },
    templateMethod: {
        generateReport: (type, clientId) => apiFetch(`${BASE_URL}/api/TemplateMethod/generate/${type}?clientId=${clientId}`)
    }
};

export default designPatternsApi;
