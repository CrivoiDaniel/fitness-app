import React, { useState } from "react";
import { PaymentElement, useElements, useStripe } from "@stripe/react-stripe-js";

export default function StripeCheckoutForm({ onSuccess }) {
  const stripe = useStripe();
  const elements = useElements();

  const [loading, setLoading] = useState(false);
  const [error, setError] = useState("");

  const onSubmit = async (e) => {
    e.preventDefault();
    setError("");

    if (!stripe || !elements) return;

    try {
      setLoading(true);

      // confirm payment in Stripe UI
      const { error: stripeErr, paymentIntent } = await stripe.confirmPayment({
        elements,
        confirmParams: {
          // fără redirect, rămâne în pagină
          return_url: window.location.href,
        },
        redirect: "if_required",
      });

      if (stripeErr) {
        setError(stripeErr.message || "Payment failed.");
        return;
      }

      if (paymentIntent?.status === "succeeded" || paymentIntent?.status === "processing") {
        onSuccess?.();
      } else {
        setError(`Payment status: ${paymentIntent?.status || "unknown"}`);
      }
    } finally {
      setLoading(false);
    }
  };

  return (
    <form onSubmit={onSubmit} className="space-y-4">
      <div className="rounded-2xl border p-4 bg-white">
        <PaymentElement />
      </div>

      {error && (
        <div className="p-3 rounded-xl bg-red-50 text-red-700 border border-red-200">
          {error}
        </div>
      )}

      <button
        type="submit"
        disabled={!stripe || !elements || loading}
        className="w-full py-3 rounded-2xl bg-black text-white font-extrabold hover:bg-gray-900 disabled:opacity-50"
      >
        {loading ? "Processing..." : "Pay now"}
      </button>

      <p className="text-xs text-gray-500">
        Card test: <code className="font-mono">4242 4242 4242 4242</code>
      </p>
    </form>
  );
}