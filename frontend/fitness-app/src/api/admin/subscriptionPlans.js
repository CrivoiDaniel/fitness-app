import { apiFetch } from "../http";

const BASE_URL = "http://localhost:5140";

export function getAllSubscriptionPlans(token) {
  return apiFetch(`${BASE_URL}/api/SubscriptionPlans`, { token });
}

export function getSubscriptionPlanWithDetails(token, id) {
  return apiFetch(`${BASE_URL}/api/SubscriptionPlans/${id}/with-details`, { token });
}

export function createSubscriptionPlan(token, dto) {
  return apiFetch(`${BASE_URL}/api/SubscriptionPlans`, {
    method: "POST",
    token,
    body: dto,
  });
}

export function updateSubscriptionPlan(token, id, dto) {
  return apiFetch(`${BASE_URL}/api/SubscriptionPlans/${id}`, {
    method: "PUT",
    token,
    body: dto,
  });
}

export function deleteSubscriptionPlan(token, id) {
  return apiFetch(`${BASE_URL}/api/SubscriptionPlans/${id}`, {
    method: "DELETE",
    token,
  });
}