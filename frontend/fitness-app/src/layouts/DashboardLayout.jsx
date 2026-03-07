import React from "react";
import { Link, Outlet } from "react-router-dom";
import { useAuth } from "../context/AuthContext";
import { useTranslation } from "react-i18next";

const DashboardLayout = () => {
  const { t } = useTranslation();
  const { user, logout } = useAuth();

  return (
    <div className="min-h-screen flex bg-gray-100">
      <aside className="w-64 bg-white border-r p-4">
        <div className="mb-6">
          <div className="font-extrabold text-lg">Dashboard</div>
          <div className="text-sm text-gray-600">
            {user?.firstName} {user?.lastName}
          </div>
        </div>

        <nav className="flex flex-col gap-2">
          <Link to="/" className="px-3 py-2 rounded hover:bg-gray-100">
            {t("common.home")}
          </Link>

          <Link to="/dashboard/admin/clients" className="px-3 py-2 rounded hover:bg-gray-100">
            {t("admin.clients.title")}
          </Link>

          <Link to="/dashboard/admin/trainers" className="px-3 py-2 rounded hover:bg-gray-100">
            Traineri
          </Link>

          <button
            onClick={logout}
            className="mt-4 text-left px-3 py-2 rounded bg-black text-white font-semibold hover:bg-gray-800"
          >
            {t("common.logout")}
          </button>
        </nav>
      </aside>

      <main className="flex-1 p-6">
        <Outlet />
      </main>
    </div>
  );
};

export default DashboardLayout;