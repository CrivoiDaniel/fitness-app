import { apiFetch } from './http';

const BASE_URL = "http://localhost:5140";

const appointmentsApi = {
    getTrainerAppointments: async (token) => {
        return apiFetch(`${BASE_URL}/api/appointments/trainer`, { token });
    },
    getClientAppointments: async (token) => {
        return apiFetch(`${BASE_URL}/api/appointments/client`, { token });
    },
    create: async (token, data) => {
        return apiFetch(`${BASE_URL}/api/appointments`, { method: 'POST', token, body: data });
    },
    update: async (token, id, data) => {
        return apiFetch(`${BASE_URL}/api/appointments/${id}`, { method: 'PUT', token, body: data });
    },
    delete: async (token, id) => {
        return apiFetch(`${BASE_URL}/api/appointments/${id}`, { method: 'DELETE', token });
    }
};

export default appointmentsApi;
