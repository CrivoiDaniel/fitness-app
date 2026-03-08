import { apiFetch } from "../http";

const BASE_URL = "http://localhost:5140";

export function getAllBenefits(token) {
  return apiFetch(`${BASE_URL}/api/Benefits`, { token });
}

export function createBenefit(token, dto) {
  return apiFetch(`${BASE_URL}/api/Benefits`, {
    method: "POST",
    token,
    body: dto,
  });
}

export function updateBenefit(token, id, dto) {
  return apiFetch(`${BASE_URL}/api/Benefits/${id}`, {
    method: "PUT",
    token,
    body: dto,
  });
}

export function deleteBenefit(token, id) {
  return apiFetch(`${BASE_URL}/api/Benefits/${id}`, {
    method: "DELETE",
    token,
  });
}