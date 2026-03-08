import { apiFetch } from "../http";

const BASE_URL = "http://localhost:5140";

export function getDashboardSummary(token) {
  return apiFetch(`${BASE_URL}/api/Statistics/dashboard`, { token });
}

export function getStatistics(token) {
  return apiFetch(`${BASE_URL}/api/Statistics`, { token });
}

export function getRevenueBreakdown(token) {
  return apiFetch(`${BASE_URL}/api/Statistics/revenue`, { token });
}

export function getTrends(token) {
  return apiFetch(`${BASE_URL}/api/Statistics/trends`, { token });
}

export function getExpiringSubscriptions(token, daysAhead = 30) {
  return apiFetch(`${BASE_URL}/api/Statistics/expiring?daysAhead=${encodeURIComponent(daysAhead)}`, { token });
}

export function getCacheInfo(token) {
  return apiFetch(`${BASE_URL}/api/Statistics/cache-info`, { token });
}

export function refreshStatisticsCache(token) {
  return apiFetch(`${BASE_URL}/api/Statistics/refresh`, { method: "POST", token });
}