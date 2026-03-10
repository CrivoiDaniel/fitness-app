import React, { useEffect, useMemo, useState } from "react";
import { useNavigate, useParams } from "react-router-dom";
import { useTranslation } from "react-i18next";

import { useAuth } from "../../context/AuthContext";
import { getPublicSubscriptionPlans } from "../../api/public/subscriptions";
import { purchaseSubscription } from "../../api/client/purchase";

import { loadStripe } from "@stripe/stripe-js";
import { Elements } from "@stripe/react-stripe-js";
import { STRIPE_PUBLISHABLE_KEY } from "../../../config/env";
import StripeCheckoutForm from "./StripeCheckoutForm";

const stripePromise = STRIPE_PUBLISHABLE_KEY ? loadStripe(STRIPE_PUBLISHABLE_KEY) : null;

function MethodCard({ selected, onClick, title, subtitle }) {
  return (
    <button
      type="button"
      onClick={onClick}
      className={[
        "w-full text-left rounded-2xl border p-4 transition shadow-sm",
        selected ? "border-black bg-gray-50" : "border-gray-200 bg-white hover:bg-gray-50",
      ].join(" ")}
    >
      <div className="font-extrabold">{title}</div>
      <div className="text-sm text-gray-600 mt-1">{subtitle}</div>
    </button>
  );
}

export default function SubscriptionCheckout() {
  const { t } = useTranslation();
  const { planId } = useParams();
  const navigate = useNavigate();
  const { token, user } = useAuth();

  const [plan, setPlan] = useState(null);
  const [loading, setLoading] = useState(true);

  const [provider, setProvider] = useState("Stripe"); // "Stripe" | "PayPal"
  const [step, setStep] = useState("method"); // "method" | "pay"
  const [creating, setCreating] = useState(false);
  const [error, setError] = useState("");

  const [payment, setPayment] = useState(null);
  // payment expected from backend:
  // { subscriptionId, paymentId, paymentStatus, transactionId, provider, clientSecret? }

  useEffect(() => {
    let cancelled = false;

    (async () => {
      try {
        setLoading(true);
        setError("");

        const plans = await getPublicSubscriptionPlans();
        const found = (plans || []).find((p) => Number(p.id) === Number(planId));
        if (!found) throw new Error("Plan not found.");

        if (!cancelled) setPlan(found);
      } catch (e) {
        if (!cancelled) setError(e.message || t("errors.unknown"));
      } finally {
        if (!cancelled) setLoading(false);
      }
    })();

    return () => {
      cancelled = true;
    };
  }, [planId, t]);

  const onContinue = async () => {
    if (!token) {
      navigate(`/login?returnUrl=${encodeURIComponent(`/subscription/checkout/${planId}`)}`);
      return;
    }

    try {
      setCreating(true);
      setError("");

      const res = await purchaseSubscription(token, {
        subscriptionPlanId: Number(planId),
        autoRenew: true,
        provider, // IMPORTANT: adapter choice
      });

      setPayment(res);

      if (provider === "PayPal") {
        // demo adapter -> arătăm success imediat
        navigate(`/subscription/success?paymentId=${res.paymentId}&tx=${encodeURIComponent(res.transactionId || "")}`, {
          replace: true,
        });
        return;
      }

      // Stripe -> trecem la step Pay (Stripe Elements)
      setStep("pay");
    } catch (e) {
      setError(e.message || t("errors.unknown"));
    } finally {
      setCreating(false);
    }
  };

  const elementsOptions = useMemo(() => {
    if (!payment?.clientSecret) return null;
    return { clientSecret: payment.clientSecret, appearance: { theme: "stripe" } };
  }, [payment?.clientSecret]);

  if (loading) return <div className="p-6">{t("common.loading")}</div>;

  return (
    <div className="min-h-screen bg-gradient-to-b from-gray-50 to-white">
      <div className="max-w-5xl mx-auto px-4 py-10">
        <div className="flex items-center justify-between flex-wrap gap-3">
          <h1 className="text-3xl font-extrabold tracking-tight">{t("checkout.title")}</h1>
          <button
            className="px-4 py-2 rounded-xl border bg-white hover:bg-gray-50 font-semibold"
            onClick={() => (step === "method" ? navigate(-1) : setStep("method"))}
          >
            {t("common.back")}
          </button>
        </div>

        {error && (
          <div className="mt-4 p-3 rounded-xl bg-red-50 text-red-700 border border-red-200">
            {error}
          </div>
        )}

        {!plan ? (
          <div className="mt-6 text-gray-600">—</div>
        ) : (
          <div className="mt-6 grid grid-cols-1 md:grid-cols-2 gap-6">
            <div className="rounded-3xl border bg-white p-6 shadow-sm">
              <div className="text-sm text-gray-500">{t("checkout.plan")}</div>
              <div className="mt-1 text-2xl font-extrabold">{plan.type}</div>

              <div className="mt-6 rounded-2xl border bg-gray-50 p-4">
                <div className="flex justify-between">
                  <span className="text-gray-600">{t("checkout.price")}</span>
                  <span className="font-extrabold">{plan.price} MDL</span>
                </div>
              </div>

              {payment?.transactionId ? (
                <div className="mt-6 rounded-2xl border bg-gray-50 p-4">
                  <div className="text-sm text-gray-500">Adapter provider</div>
                  <div className="mt-1 font-extrabold">{payment.provider}</div>
                  <div className="mt-2 text-xs text-gray-500 break-all">
                    TX: <span className="font-mono">{payment.transactionId}</span>
                  </div>
                </div>
              ) : null}
            </div>

            <div className="rounded-3xl border bg-white p-6 shadow-sm">
              <div className="text-sm text-gray-500">{t("checkout.account")}</div>
              <div className="mt-1 font-semibold">
                {user ? `${user.firstName} ${user.lastName}` : t("checkout.notLoggedIn")}
              </div>

              {step === "method" ? (
                <>
                  <div className="mt-6 space-y-3">
                    <MethodCard
                      selected={provider === "Stripe"}
                      onClick={() => setProvider("Stripe")}
                      title="Card (Stripe)"
                      subtitle="UI real de card + confirmare Stripe (test)."
                    />
                    <MethodCard
                      selected={provider === "PayPal"}
                      onClick={() => setProvider("PayPal")}
                      title="PayPal (Demo Adapter)"
                      subtitle="Generează OrderId demo (PAYPAL-DEMO-...)."
                    />
                  </div>

                  <button
                    onClick={onContinue}
                    disabled={creating}
                    className="mt-6 w-full py-3 rounded-2xl bg-black text-white font-extrabold hover:bg-gray-900 disabled:opacity-50"
                  >
                    {creating ? t("checkout.processing") : t("checkout.continueToPay")}
                  </button>

                  <p className="mt-3 text-xs text-gray-500">{t("checkout.note")}</p>
                </>
              ) : (
                <>
                  {!stripePromise ? (
                    <div className="mt-6 text-sm text-red-700">
                      Missing VITE_STRIPE_PUBLISHABLE_KEY in frontend/.env
                    </div>
                  ) : !elementsOptions ? (
                    <div className="mt-6 text-sm text-red-700">
                      Backend must return clientSecret for Stripe in PurchaseSubscriptionResultDto.
                    </div>
                  ) : (
                    <div className="mt-6">
                      <Elements stripe={stripePromise} options={elementsOptions}>
                        <StripeCheckoutForm
                          onSuccess={() =>
                            navigate(
                              `/subscription/success?paymentId=${payment.paymentId}&tx=${encodeURIComponent(payment.transactionId || "")}`,
                              { replace: true }
                            )
                          }
                        />
                      </Elements>
                    </div>
                  )}
                </>
              )}
            </div>
          </div>
        )}
      </div>
    </div>
  );
}