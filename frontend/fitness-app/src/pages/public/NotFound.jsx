import React from "react";
import { Link } from "react-router-dom";
import { useTranslation } from "react-i18next";

const NotFound = () => {
  const { t } = useTranslation();

  return (
    <div className="min-h-screen flex items-center justify-center px-4 bg-gray-100">
      <div className="max-w-md w-full bg-white border rounded-xl p-8 text-center">
        <h1 className="text-3xl font-extrabold mb-2">{t("notFound.title")}</h1>
        <p className="text-gray-600 mb-6">{t("notFound.message")}</p>
        <Link to="/" className="inline-block px-4 py-2 rounded-md bg-black text-white font-semibold">
          {t("notFound.backHome")}
        </Link>
      </div>
    </div>
  );
};

export default NotFound;