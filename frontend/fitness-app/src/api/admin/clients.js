import { apiFetch } from "../http";

const BASE_URL = "http://localhost:5140";

export function getAllClients(token) {
  return apiFetch(`${BASE_URL}/api/admin/clients`, { token });
}

export function createClient(token, dto) {
  return apiFetch(`${BASE_URL}/api/admin/clients`, { method: "POST", token, body: dto });
}

export function updateClient(token, userId, dto) {
  return apiFetch(`${BASE_URL}/api/admin/clients/${userId}`, { method: "PUT", token, body: dto });
}

export function deleteClient(token, userId) {
  return apiFetch(`${BASE_URL}/api/admin/clients/${userId}`, { method: "DELETE", token });
}