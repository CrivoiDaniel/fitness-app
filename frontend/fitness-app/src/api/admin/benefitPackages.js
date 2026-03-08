import { apiFetch } from "../http";

const BASE_URL = "http://localhost:5140";

export function getAllBenefitPackages(token) {
  return apiFetch(`${BASE_URL}/api/BenefitPackages`, { token });
}

export function getBenefitPackageWithItems(token, id) {
  return apiFetch(`${BASE_URL}/api/BenefitPackages/${id}/with-items`, { token });
}

export function createBenefitPackage(token, dto) {
  return apiFetch(`${BASE_URL}/api/BenefitPackages`, {
    method: "POST",
    token,
    body: dto,
  });
}

export function updateBenefitPackage(token, id, dto) {
  return apiFetch(`${BASE_URL}/api/BenefitPackages/${id}`, {
    method: "PUT",
    token,
    body: dto,
  });
}

export function deleteBenefitPackage(token, id) {
  return apiFetch(`${BASE_URL}/api/BenefitPackages/${id}`, {
    method: "DELETE",
    token,
  });
}