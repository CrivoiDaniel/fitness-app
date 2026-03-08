import React from "react";
import { FiUsers, FiUser, FiClipboard, FiGift, FiPackage, FiCreditCard, FiDollarSign, FiActivity } from "react-icons/fi";

export const adminSidebarItems = [
  { to: "/dashboard/admin/clients", labelKey: "sidebar.admin.clients", icon: <FiUsers /> },
  { to: "/dashboard/admin/trainers", labelKey: "sidebar.admin.trainers", icon: <FiUser /> },
  { to: "/dashboard/admin/benefits", labelKey: "sidebar.admin.benefits", icon: <FiGift /> },
  { to: "/dashboard/admin/benefit-packages", labelKey: "sidebar.admin.benefitPackages", icon: <FiPackage /> },
  { to: "/dashboard/admin/subscription-plans", labelKey: "sidebar.admin.subscriptionPlans", icon: <FiCreditCard /> },
  { to: "/dashboard/admin/subscriptions", labelKey: "sidebar.admin.subscriptions", icon: <FiClipboard /> },
  { to: "/dashboard/admin/payments", labelKey: "sidebar.admin.payments", icon: <FiDollarSign /> },
  { to: "/dashboard/admin/workout-plans", labelKey: "sidebar.admin.workoutPlans", icon: <FiActivity /> }
];

export const trainerSidebarItems = [
  { to: "/dashboard/trainer/plans", labelKey: "sidebar.trainer.plans", icon: <FiClipboard /> }
];

export const clientSidebarItems = [
  // momentan gol sau adaugi ce vrei
];