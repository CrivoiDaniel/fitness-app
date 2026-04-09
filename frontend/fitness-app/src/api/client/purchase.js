import { apiFetch } from "../http";
import { API_BASE_URL } from "../../../config/env";

export function purchaseSubscription(token, dto) {
  return apiFetch(`${API_BASE_URL}/api/subscriptions/purchase`, {
    method: "POST",
    token,
    body: dto,
  });
}