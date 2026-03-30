import { apiFetch } from "../http";
import { API_BASE_URL } from "../../../config/env";

export function getMyWorkoutPlans(token) {
  return apiFetch(`${API_BASE_URL}/api/WorkoutPlans/me`, { token });
}

export async function exportWorkoutPlan(token, planId, format, detailLevel) {
  const url = `${API_BASE_URL}/api/WorkoutPlans/${planId}/export?format=${format}&detailLevel=${detailLevel}`;
  const res = await fetch(url, {
    method: "GET",
    headers: {
      Authorization: `Bearer ${token}`,
    },
  });

  if (!res.ok) {
    const errorData = await res.json().catch(() => null);
    throw new Error(errorData?.message || "Export failed.");
  }

  // Fix fallback extension for excel
  let ext = format === "excel" ? "xlsx" : format;
  let filename = `WorkoutPlan_${planId}_${detailLevel}.${ext}`;
  const disposition = res.headers.get("Content-Disposition");
  if (disposition && disposition.indexOf("filename=") !== -1) {
    const filenameRegex = /filename[^;=\n]*=((['"]).*?\2|[^;\n]*)/;
    const matches = filenameRegex.exec(disposition);
    if (matches != null && matches[1]) {
      filename = matches[1].replace(/['"]/g, "");
    }
  }

  const blob = await res.blob();
  const downloadUrl = window.URL.createObjectURL(blob);
  const link = document.createElement("a");
  link.href = downloadUrl;
  link.download = filename;
  document.body.appendChild(link);
  link.click();
  link.remove();
  window.URL.revokeObjectURL(downloadUrl);
}