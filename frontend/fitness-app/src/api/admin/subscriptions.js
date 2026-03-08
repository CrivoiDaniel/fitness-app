import { apiFetch } from "../http";

const BASE_URL = "http://localhost:5140";

export function getAllSubscriptions(token) {
  return apiFetch(`${BASE_URL}/api/Subscriptions`, { token });
}

export function createSubscription(token, dto) {
  // dto: { clientId, subscriptionPlanId, startDate, autoRenew }
  return apiFetch(`${BASE_URL}/api/Subscriptions`, {
    method: "POST",
    token,
    body: dto,
  });
}

export function updateSubscription(token, id, dto) {
  // dto: { endDate, status, autoRenew }
  return apiFetch(`${BASE_URL}/api/Subscriptions/${id}`, {
    method: "PUT",
    token,
    body: dto,
  });
}

export function cancelSubscription(token, id) {
  return apiFetch(`${BASE_URL}/api/Subscriptions/${id}/cancel`, {
    method: "POST",
    token,
  });
}

export function renewSubscription(token, id) {
  return apiFetch(`${BASE_URL}/api/Subscriptions/${id}/renew`, {
    method: "POST",
    token,
  });
}

export function deleteSubscription(token, id) {
  return apiFetch(`${BASE_URL}/api/Subscriptions/${id}`, {
    method: "DELETE",
    token,
  });
}