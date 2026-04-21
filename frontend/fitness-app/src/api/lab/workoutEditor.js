import { apiFetch } from "../http";

const BASE_URL = "/api/trainer/workout-editor";

export const workoutEditorApi = {
  getClients: async (token) => {
    return await apiFetch(`${BASE_URL}/clients`, { token });
  },

  startSession: async (token, clientId, planName) => {
    return await apiFetch(`${BASE_URL}/start-session/${clientId}?planName=${encodeURIComponent(planName)}`, {
      method: "POST",
      token
    });
  },

  getState: async (token) => {
    return await apiFetch(`${BASE_URL}/state`, { token });
  },

  addExercise: async (token, name, sets, reps) => {
    return await apiFetch(`${BASE_URL}/add-exercise?name=${encodeURIComponent(name)}&sets=${sets}&reps=${reps}`, {
      method: "POST",
      token
    });
  },

  undo: async (token) => {
    return await apiFetch(`${BASE_URL}/undo`, { method: "POST", token });
  },

  redo: async (token) => {
    return await apiFetch(`${BASE_URL}/redo`, { method: "POST", token });
  },

  reset: async (token) => {
    return await apiFetch(`${BASE_URL}/reset`, { method: "DELETE", token });
  },

  savePlan: async (token) => {
    return await apiFetch(`${BASE_URL}/save`, { method: "POST", token });
  },

  createCheckpoint: async (token, name) => {
    return await apiFetch(`${BASE_URL}/checkpoint?name=${encodeURIComponent(name)}`, { 
      method: "POST", 
      token 
    });
  },

  loadCheckpoint: async (token, index) => {
    return await apiFetch(`${BASE_URL}/load-checkpoint/${index}`, { 
      method: "POST", 
      token 
    });
  },

  startNavigation: async (token, type) => {
    return await apiFetch(`${BASE_URL}/navigation/start?type=${type}`, { 
      method: "POST", 
      token 
    });
  },

  nextExercise: async (token) => {
    return await apiFetch(`${BASE_URL}/navigation/next`, { 
      method: "POST", 
      token 
    });
  },

  resetNavigation: async (token) => {
    return await apiFetch(`${BASE_URL}/navigation/reset`, { 
      method: "POST", 
      token 
    });
  }
};
