import { apiFetch } from './http';

const BASE_URL = "http://localhost:5140";

const googleAuthApi = {
    getAuthUrl: async (token) => {
        const response = await apiFetch(`${BASE_URL}/api/google-auth/url`, { token });
        return response;
    },
    callback: async (token, code) => {
        const response = await apiFetch(`${BASE_URL}/api/google-auth/callback`, { 
            method: 'POST', 
            token, 
            body: { code } 
        });
        return response;
    }
};

export default googleAuthApi;
