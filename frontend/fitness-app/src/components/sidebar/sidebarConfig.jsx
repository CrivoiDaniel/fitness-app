import React from "react";
import { FiUsers, FiUser, FiClipboard } from "react-icons/fi";

export const adminSidebarItems = [
  { to: "/dashboard/admin/clients", labelKey: "sidebar.admin.clients", icon: <FiUsers /> },
  { to: "/dashboard/admin/trainers", labelKey: "sidebar.admin.trainers", icon: <FiUser /> }
];

export const trainerSidebarItems = [
  { to: "/dashboard/trainer/plans", labelKey: "sidebar.trainer.plans", icon: <FiClipboard /> }
];

export const clientSidebarItems = [
  // momentan gol sau adaugi ce vrei
];