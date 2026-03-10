import React, { useEffect, useMemo, useState } from "react";
import { useTranslation } from "react-i18next";
import { useAuth } from "../../context/AuthContext";
import { getMySubscriptions } from "../../api/client/subscriptions";
import { getMyWorkoutPlans } from "../../api/client/workoutPlans";
import { Link } from "react-router-dom";

function badgeClass(status) {
  if (status === "Active") return "bg-green-100 text-green-800 border-green-200";
  if (status === "Pending") return "bg-yellow-100 text-yellow-800 border-yellow-200";
  if (status === "Expired") return "bg-gray-100 text-gray-800 border-gray-200";
  if (status === "Cancelled") return "bg-red-100 text-red-800 border-red-200";
  return "bg-gray-100 text-gray-800 border-gray-200";
}

function safeDate(value) {
  if (!value) return null;
  const d = new Date(value);
  return Number.isNaN(d.getTime()) ? null : d;
}

function daysBetween(a, b) {
  const ms = b.getTime() - a.getTime();
  return Math.ceil(ms / (1000 * 60 * 60 * 24));
}

export default function ClientDashboardPage() {
  const { t } = useTranslation();
  const { token, user } = useAuth();

  const [loading, setLoading] = useState(true);
  const [error, setError] = useState("");

  const [subs, setSubs] = useState([]);
  const [plans, setPlans] = useState([]);

  useEffect(() => {
    let cancelled = false;

    (async () => {
      try {
        setLoading(true);
        setError("");

        const subsData = await getMySubscriptions(token, user);
        if (!cancelled) setSubs(Array.isArray(subsData) ? subsData : []);

        const wp = await getMyWorkoutPlans(token);
        if (!cancelled) setPlans(Array.isArray(wp) ? wp : []);
      } catch (e) {
        if (!cancelled) setError(e.message || "Request failed.");
      } finally {
        if (!cancelled) setLoading(false);
      }
    })();

    return () => {
      cancelled = true;
    };
  }, [token, user]);

  const current = useMemo(() => {
    if (!subs.length) return null;
    const byPriority = [...subs].sort((a, b) => {
      const pa = a.status === "Active" ? 0 : a.status === "Pending" ? 1 : 2;
      const pb = b.status === "Active" ? 0 : b.status === "Pending" ? 1 : 2;
      if (pa !== pb) return pa - pb;
      const da = safeDate(a.startDate)?.getTime() ?? 0;
      const db = safeDate(b.startDate)?.getTime() ?? 0;
      return db - da;
    });
    return byPriority[0];
  }, [subs]);

  const myPlan = plans?.[0] ?? null;

  const startDate = safeDate(current?.startDate);
  const endDate = safeDate(current?.endDate);
  const daysLeft = endDate ? Math.max(daysBetween(new Date(), endDate), 0) : null;

  return (
    <div className="min-h-screen bg-gradient-to-b from-gray-50 to-white">
      <div className="max-w-6xl mx-auto px-4 py-8">
        <div className="flex items-end justify-between gap-4 flex-wrap">
          <div>
            <h1 className="text-3xl font-extrabold tracking-tight">
              {t("client.dashboardTitle")}
            </h1>
            <p className="text-gray-600 mt-1">
              {user ? `${user.firstName} ${user.lastName}` : "—"}
            </p>
          </div>

          <div className="flex gap-2">
            <Link
              to="/subscription"
              className="px-4 py-2 rounded-xl border bg-white hover:bg-gray-50 font-semibold"
            >
              {t("home.purchase")}
            </Link>
          </div>
        </div>

        {error && (
          <div className="mt-4 p-3 rounded-xl bg-red-50 text-red-700 border border-red-200">
            {error}
          </div>
        )}

        {loading ? (
          <div className="mt-6 text-gray-600">{t("client.loading")}</div>
        ) : (
          <div className="mt-6 grid grid-cols-1 lg:grid-cols-3 gap-6">
            {/* Subscription */}
            <div className="lg:col-span-2 rounded-3xl border bg-white p-6 shadow-sm">
              <div className="flex items-start justify-between gap-3">
                <h2 className="text-xl font-extrabold">{t("client.mySubscription")}</h2>

                {current?.status ? (
                  <span className={["px-3 py-1 text-sm rounded-full border font-semibold", badgeClass(current.status)].join(" ")}>
                    {current.status}
                  </span>
                ) : null}
              </div>

              {!current ? (
                <p className="mt-4 text-gray-600">{t("client.notAvailable")}</p>
              ) : (
                <div className="mt-5 grid sm:grid-cols-2 gap-4">
                  <div className="rounded-2xl border p-4 bg-gray-50">
                    <div className="text-sm text-gray-500">{t("client.plan")}</div>
                    <div className="text-lg font-bold mt-1">
                      {current.planType ?? current.subscriptionPlanType ?? current.type ?? "—"}
                    </div>
                  </div>

                  <div className="rounded-2xl border p-4 bg-gray-50">
                    <div className="text-sm text-gray-500">{t("client.expiresIn")}</div>
                    <div className="text-lg font-bold mt-1">
                      {daysLeft !== null ? `${daysLeft} ${t("client.days")}` : "—"}
                    </div>
                    <div className="text-xs text-gray-500 mt-1">
                      {endDate ? endDate.toLocaleDateString() : "—"}
                    </div>
                  </div>

                  <div className="rounded-2xl border p-4 bg-gray-50">
                    <div className="text-sm text-gray-500">Start</div>
                    <div className="text-lg font-bold mt-1">
                      {startDate ? startDate.toLocaleDateString() : "—"}
                    </div>
                  </div>

                  <div className="rounded-2xl border p-4 bg-gray-50">
                    <div className="text-sm text-gray-500">{t("client.status")}</div>
                    <div className="text-lg font-bold mt-1">{current.status}</div>
                  </div>
                </div>
              )}
            </div>

            {/* Trainer placeholder */}
            <div className="rounded-3xl border bg-white p-6 shadow-sm">
              <h2 className="text-xl font-extrabold">{t("client.trainer")}</h2>
              <div className="mt-5 rounded-2xl border bg-gray-50 p-4">
                <p className="text-gray-700 font-semibold">{t("client.notAvailable")}</p>
              </div>
            </div>

            {/* Workouts REAL */}
            <div className="lg:col-span-3 rounded-3xl border bg-white p-6 shadow-sm">
              <h2 className="text-xl font-extrabold">{t("client.workouts")}</h2>

              {!myPlan ? (
                <div className="mt-5 rounded-2xl border bg-gray-50 p-4">
                  <p className="text-gray-700 font-semibold">{t("client.noWorkoutPlan")}</p>
                </div>
              ) : (
                <div className="mt-5 grid grid-cols-1 md:grid-cols-3 gap-4">
                  <div className="rounded-2xl border bg-gray-50 p-4">
                    <div className="text-sm text-gray-500">{t("client.planName")}</div>
                    <div className="text-lg font-extrabold mt-1">{myPlan.name}</div>
                    <div className="text-xs text-gray-500 mt-1">
                      {myPlan.goal} • {myPlan.difficulty}
                    </div>
                  </div>

                  <div className="rounded-2xl border bg-gray-50 p-4">
                    <div className="text-sm text-gray-500">{t("client.totalSets")}</div>
                    <div className="text-2xl font-extrabold mt-1">{myPlan.totalSets ?? 0}</div>
                  </div>

                  <div className="rounded-2xl border bg-gray-50 p-4">
                    <div className="text-sm text-gray-500">{t("client.totalReps")}</div>
                    <div className="text-2xl font-extrabold mt-1">{myPlan.totalReps ?? 0}</div>
                  </div>

                  <div className="md:col-span-3 rounded-2xl border bg-gray-50 p-4">
                    <div className="text-sm text-gray-500">{t("client.exercises")}</div>
                    <ul className="mt-2 space-y-2">
                      {(myPlan.exercises || []).slice(0, 12).map((e) => (
                        <li key={e.id} className="flex justify-between gap-3">
                          <span className="font-semibold">
                            {e.orderInWorkout}. {e.name}
                          </span>
                          <span className="text-gray-600 text-sm">
                            {e.sets}x{e.reps}{e.durationSeconds ? ` • ${e.durationSeconds}s` : ""}
                          </span>
                        </li>
                      ))}
                    </ul>
                  </div>
                </div>
              )}
            </div>
          </div>
        )}
      </div>
    </div>
  );
}