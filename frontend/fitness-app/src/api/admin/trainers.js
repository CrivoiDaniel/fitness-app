import { apiFetch } from "../http";

const BASE_URL = "http://localhost:5140";

export function getAllTrainers(token) {
  return apiFetch(`${BASE_URL}/api/admin/trainers`, { token });
}

export function createTrainer(token, dto) {
  return apiFetch(`${BASE_URL}/api/admin/trainers`, { method: "POST", token, body: dto });
}

export function updateTrainer(token, userId, dto) {
  return apiFetch(`${BASE_URL}/api/admin/trainers/${userId}`, { method: "PUT", token, body: dto });
}

export function deleteTrainer(token, userId) {
  return apiFetch(`${BASE_URL}/api/admin/trainers/${userId}`, { method: "DELETE", token });
}