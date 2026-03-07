export async function getPublicSubscriptionPlans(baseUrl = "http://localhost:5140") {
  const res = await fetch(`${baseUrl}/api/public/subscription-plans`);

  if (!res.ok) {
    const text = await res.text();
    throw new Error(`Failed to load subscription plans: ${res.status} ${text}`);
  }

  return await res.json();
}