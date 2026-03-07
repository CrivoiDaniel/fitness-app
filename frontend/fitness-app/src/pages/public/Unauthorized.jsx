import React from "react";
import { Link } from "react-router-dom";
import { useTranslation } from "react-i18next";

const Unauthorized = () => {
  const { t } = useTranslation();

  return (
    <div className="min-h-screen flex items-center justify-center px-4 bg-gray-100">
      <div className="max-w-md w-full bg-white border rounded-xl p-8 text-center">
        <h1 className="text-2xl font-bold mb-2">{t("unauthorized.title")}</h1>
        <p className="text-gray-600 mb-6">{t("unauthorized.message")}</p>

        <div className="flex justify-center gap-3">
          <Link to="/" className="px-4 py-2 rounded-md bg-black text-white font-semibold">
            {t("unauthorized.backHome")}
          </Link>
          <Link to="/dashboard" className="px-4 py-2 rounded-md border font-semibold">
            {t("unauthorized.dashboard")}
          </Link>
        </div>
      </div>
    </div>
  );
};

export default Unauthorized;