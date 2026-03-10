import React from "react";
import { Link, useSearchParams } from "react-router-dom";
import { useTranslation } from "react-i18next";

export default function SubscriptionSuccess() {
  const { t } = useTranslation();
  const [sp] = useSearchParams();

  const paymentId = sp.get("paymentId");
  const tx = sp.get("tx");

  return (
    <div className="min-h-screen bg-gradient-to-b from-gray-50 to-white">
      <div className="max-w-3xl mx-auto px-4 py-12">
        <div className="rounded-3xl border bg-white p-8 shadow-sm">
          <h1 className="text-3xl font-extrabold tracking-tight">
            {t("checkout.successTitle")}
          </h1>
          <p className="mt-2 text-gray-700">{t("checkout.successText")}</p>

          <div className="mt-6 grid gap-4">
            <div className="rounded-2xl border bg-gray-50 p-4">
              <div className="text-sm text-gray-500">Payment ID</div>
              <div className="text-lg font-extrabold mt-1">{paymentId ?? "—"}</div>
            </div>

            <div className="rounded-2xl border bg-gray-50 p-4">
              <div className="text-sm text-gray-500">Transaction</div>
              <div className="text-lg font-extrabold mt-1 break-all">{tx || "—"}</div>
            </div>
          </div>

          <div className="mt-8 flex gap-3 flex-wrap">
            <Link
              to="/"
              className="px-4 py-2 rounded-xl border bg-white hover:bg-gray-50 font-semibold"
            >
              {t("common.goHome")}
            </Link>

            <Link
              to="/dashboard/client"
              className="px-4 py-2 rounded-xl bg-black text-white hover:bg-gray-900 font-extrabold"
            >
              {t("common.goDashboard")}
            </Link>
          </div>
        </div>
      </div>
    </div>
  );
}