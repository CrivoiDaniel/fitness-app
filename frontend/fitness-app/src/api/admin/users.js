import { apiFetch } from "../http";
const BASE_URL = "http://localhost:5140";

export function resetUserPassword(token, userId) {
  return apiFetch(`${BASE_URL}/api/admin/users/${userId}/reset-password`, {
    method: "POST",
    token,
  });
}