export async function getPublicSubscriptionPlans(baseUrl = "http://localhost:5140") {
  const res = await fetch(`${baseUrl}/api/public/subscription-plans`);

  if (!res.ok) {
    const text = await res.text();
    throw new Error(`Failed to load subscription plans: ${res.status} ${text}`);
  }

  return await res.json();
}

// NEW (fallback): ia lista și filtrează
export async function getPublicSubscriptionPlanById(id) {
  const all = await getPublicSubscriptionPlans();
  const found = (all || []).find((x) => Number(x.id) === Number(id));
  if (!found) throw new Error("Subscription plan not found.");
  return found;
}