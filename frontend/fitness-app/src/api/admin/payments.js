import { apiFetch } from "../http";

const BASE_URL = "http://localhost:5140";

export function getAllPayments(token) {
  return apiFetch(`${BASE_URL}/api/Payments`, { token });
}

export function createPayment(token, dto) {
  // dto: { subscriptionId, amount, paymentDate, installmentNumber, transactionId }
  return apiFetch(`${BASE_URL}/api/Payments`, { method: "POST", token, body: dto });
}

export function updatePayment(token, id, dto) {
  // dto: { status, transactionId }
  return apiFetch(`${BASE_URL}/api/Payments/${id}`, { method: "PUT", token, body: dto });
}

export function markPaymentSuccess(token, id, transactionId) {
  const qs = transactionId ? `?transactionId=${encodeURIComponent(transactionId)}` : "";
  return apiFetch(`${BASE_URL}/api/Payments/${id}/mark-success${qs}`, { method: "POST", token });
}

export function markPaymentFailed(token, id) {
  return apiFetch(`${BASE_URL}/api/Payments/${id}/mark-failed`, { method: "POST", token });
}

export function deletePayment(token, id) {
  return apiFetch(`${BASE_URL}/api/Payments/${id}`, { method: "DELETE", token });
}