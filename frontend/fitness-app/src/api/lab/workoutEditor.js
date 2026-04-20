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
  }
};
