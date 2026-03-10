import { apiFetch } from "../http";
import { API_BASE_URL } from "../../../config/env";

export function getMySubscriptions(token, user) {
  const clientId = user?.clientId;
  if (!clientId) throw new Error("Missing clientId in auth profile.");

  return apiFetch(`${API_BASE_URL}/api/Subscriptions/client/${clientId}`, { token });
}