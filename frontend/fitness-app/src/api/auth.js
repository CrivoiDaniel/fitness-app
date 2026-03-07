import { apiFetch } from "./http";

const BASE_URL = "http://localhost:5140";

export async function loginRequest(baseUrl, { email, password }) {
  const res = await fetch(`${baseUrl}/api/Auth/login`, {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify({ email, password }),
  });

  const data = await res.json().catch(() => null);

  if (!res.ok) {
    const msg =
      data?.message ||
      (data?.details ? `Login failed: ${data.details}` : "Login failed.");
    throw new Error(msg);
  }

  return data;
}

export function changePassword(token, dto) {
  // dto: { currentPassword, newPassword }
  return apiFetch(`${BASE_URL}/api/Auth/change-password`, {
    method: "POST",
    token,
    body: dto,
  });
}
