import { apiFetch } from "../http";
import { API_BASE_URL } from "../../../config/env";

export function getMyWorkoutPlans(token) {
  return apiFetch(`${API_BASE_URL}/api/WorkoutPlans/me`, { token });
}