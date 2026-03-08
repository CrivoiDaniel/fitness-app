import React, { useMemo } from "react";
import { Outlet } from "react-router-dom";
import { useAuth } from "../context/AuthContext";
import Sidebar from "../components/sidebar/Sidebar";
import {
  adminSidebarItems,
  clientSidebarItems,
  trainerSidebarItems,
} from "../components/sidebar/sidebarConfig";

const AppDashboardLayout = () => {
  const { user } = useAuth();

  const { brand, items } = useMemo(() => {
    if (user?.role === "Admin") return { brand: "Admin", items: adminSidebarItems };
    if (user?.role === "Trainer") return { brand: "Trainer", items: trainerSidebarItems };
    return { brand: "Client", items: clientSidebarItems };
  }, [user?.role]);

  return (
    <div className="min-h-screen flex bg-gray-100">
      <Sidebar brand={brand} items={items} />
      <main className="flex-1 p-6">
        <Outlet />
      </main>
    </div>
  );
};

export default AppDashboardLayout;