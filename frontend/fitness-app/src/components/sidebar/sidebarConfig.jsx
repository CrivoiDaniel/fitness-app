import { FiUsers, FiUser, FiClipboard, FiGift, FiPackage, FiCreditCard, FiDollarSign, FiActivity, FiList, FiCalendar, FiMessageSquare } from "react-icons/fi";

export const adminSidebarItems = [
  { to: "/dashboard/admin/clients", labelKey: "sidebar.admin.clients", icon: <FiUsers /> },
  { to: "/dashboard/admin/trainers", labelKey: "sidebar.admin.trainers", icon: <FiUser /> },
  { to: "/dashboard/admin/benefits", labelKey: "sidebar.admin.benefits", icon: <FiGift /> },
  { to: "/dashboard/admin/benefit-packages", labelKey: "sidebar.admin.benefitPackages", icon: <FiPackage /> },
  { to: "/dashboard/admin/subscription-plans", labelKey: "sidebar.admin.subscriptionPlans", icon: <FiCreditCard /> },
  { to: "/dashboard/admin/subscriptions", labelKey: "sidebar.admin.subscriptions", icon: <FiClipboard /> },
  { to: "/dashboard/admin/payments", labelKey: "sidebar.admin.payments", icon: <FiDollarSign /> },
  { to: "/dashboard/admin/payment-gateway-logs", labelKey: "sidebar.admin.gatewayLogs", icon: <FiList /> },
  { to: "/dashboard/admin/workout-plans", labelKey: "sidebar.admin.workoutPlans", icon: <FiActivity /> },
  { to: "/dashboard/lab/mediator", labelKey: "Chat (Mediator)", icon: <FiMessageSquare /> },
  { to: "/dashboard/lab/template-method", labelKey: "Reports (Template)", icon: <FiClipboard /> },
  { to: "/dashboard/lab/visitor", labelKey: "Scores (Visitor)", icon: <FiActivity /> }
];

export const trainerSidebarItems = [
  { to: "/dashboard/trainer/calendar", labelKey: "sidebar.trainer.calendar", icon: <FiCalendar /> },
  { to: "/dashboard/trainer/plans", labelKey: "sidebar.trainer.plans", icon: <FiClipboard /> },
  { to: "/dashboard/trainer/requests", labelKey: "sidebar.trainer.requests", icon: <FiList /> },
  { to: "/dashboard/lab/mediator", labelKey: "Chat (Mediator)", icon: <FiMessageSquare /> },
  { to: "/dashboard/lab/template-method", labelKey: "Reports (Template)", icon: <FiClipboard /> },
  { to: "/dashboard/lab/visitor", labelKey: "Scores (Visitor)", icon: <FiActivity /> }
];

export const clientSidebarItems = [
  { to: "/dashboard/client/calendar", labelKey: "sidebar.client.calendar", icon: <FiCalendar /> },
  { to: "/dashboard/client/lab/chain-of-responsibility", labelKey: "sidebar.client.labChain", icon: <FiActivity /> },
  { to: "/dashboard/lab/mediator", labelKey: "Chat (Mediator)", icon: <FiMessageSquare /> },
  { to: "/dashboard/lab/template-method", labelKey: "Reports (Template)", icon: <FiClipboard /> },
  { to: "/dashboard/lab/visitor", labelKey: "Scores (Visitor)", icon: <FiActivity /> }
];