import React from "react";
import { useAuth } from "../../context/AuthContext";
import { useTranslation } from "react-i18next";

const ClientDashboard = () => {
  const { t } = useTranslation();
  const { user, logout } = useAuth();

  return (
    <div className="min-h-screen bg-black text-white p-10">
      <h1 className="text-4xl font-extrabold italic">{t("dashboard.clientTitle")}</h1>
      <p className="mt-4 text-white/70">
        {user?.firstName} {user?.lastName} ({user?.email})
      </p>

      <button onClick={logout} className="mt-6 bg-yellow-400/80 text-black px-4 py-2 rounded-md font-semibold hover:bg-yellow-300">
        {t("common.logout")}
      </button>
    </div>
  );
};

export default ClientDashboard;