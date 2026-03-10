import { apiFetch } from "../http";
import { API_BASE_URL } from "../../../config/env";

export function purchaseSubscription(token, dto) {
  return apiFetch(`${API_BASE_URL}/api/PurchaseSubscription`, {
    method: "POST",
    token,
    body: dto,
  });
}