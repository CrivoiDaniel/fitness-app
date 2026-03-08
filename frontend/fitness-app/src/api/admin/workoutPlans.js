import { apiFetch } from "../http";

const BASE_URL = "http://localhost:5140";

export function getAllWorkoutPlans(token) {
  return apiFetch(`${BASE_URL}/api/WorkoutPlans`, { token });
}

export function getWorkoutPlanById(token, id) {
  return apiFetch(`${BASE_URL}/api/WorkoutPlans/${id}`, { token });
}

export function getWorkoutPlansByClientId(token, clientId) {
  return apiFetch(`${BASE_URL}/api/WorkoutPlans/client/${clientId}`, { token });
}

export function createWorkoutPlan(token, dto) {
  return apiFetch(`${BASE_URL}/api/WorkoutPlans`, {
    method: "POST",
    token,
    body: dto,
  });
}

export function cloneWorkoutPlan(token, dto) {
  // dto: { sourceWorkoutPlanId, targetClientId, newName, newTrainerId? }
  return apiFetch(`${BASE_URL}/api/WorkoutPlans/clone`, {
    method: "POST",
    token,
    body: dto,
  });
}

export function cloneAsTemplate(token, sourceId, newName) {
  const qs = `?newName=${encodeURIComponent(newName)}`;
  return apiFetch(`${BASE_URL}/api/WorkoutPlans/${sourceId}/clone-as-template${qs}`, {
    method: "POST",
    token,
  });
}