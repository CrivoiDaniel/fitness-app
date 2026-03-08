import React from "react";
import { BrowserRouter as Router, Routes, Route, Outlet } from "react-router-dom";

import { AuthProvider } from "./context/AuthContext";
import { NavbarTriggerProvider } from "./context/NavbarTriggerContext";

import Navbar from "./components/public/Navbar";
import Home from "./pages/public/Home";
import Login from "./pages/public/Login";
import ChangePasswordPage from "./pages/public/ChangePassword";

import NotFound from "./pages/public/NotFound";
import Unauthorized from "./pages/public/Unauthorized";

import ProtectedRoute from "./components/ProtectedRoute";
import RoleRoute from "./components/RoleRoute";

import ClientsPage from "./pages/admin/clients/ClientsPage";
import TrainersPage from "./pages/admin/trainers/TrainersPage";

import Subscription from "./components/public/Subscription";

import AppDashboardLayout from "./layouts/AppDashboardLayout";

import BenefitsPage from "./pages/admin/benefits/BenefitsPage";
import BenefitPackagesPage from "./pages/admin/benefitPackages/BenefitPackagesPage";
import SubscriptionPlansPage from "./pages/admin/subscriptionPlans/SubscriptionPlansPage";
import SubscriptionsPage from "./pages/admin/subscriptions/SubscriptionsPage";
import PaymentsPage from "./pages/admin/payments/PaymentsPage";
import WorkoutPlansPage from "./pages/admin/workoutPlans/WorkoutPlansPage";

import AdminDashboardPage from "./pages/admin/dashboard/AdminDashboardPage";

// Dashboards (temporar)
const AdminDashboard = () => <div className="text-2xl font-bold">Admin Dashboard</div>;
const TrainerDashboard = () => <div className="text-2xl font-bold">Trainer Dashboard</div>;
const ClientDashboard = () => <div className="text-2xl font-bold">Client Dashboard</div>;


function PublicLayout() {
  return (
    <div className="min-h-screen">
      <Navbar />
      <main>
        <Outlet />
      </main>
    </div>
  );
}

const App = () => {
  return (
    <AuthProvider>
      <NavbarTriggerProvider>
        <Router>
          <Routes>
            {/* PUBLIC */}
            <Route element={<PublicLayout />}>
              <Route path="/" element={<Home />} />
              <Route path="/subscription" element={<Subscription />} />
              <Route path="/login" element={<Login />} />

              <Route
                path="/change-password"
                element={
                  <ProtectedRoute>
                    <ChangePasswordPage />
                  </ProtectedRoute>
                }
              />
            </Route>

            {/* DASHBOARD */}
            <Route
              path="/dashboard"
              element={
                <ProtectedRoute>
                  <AppDashboardLayout />
                </ProtectedRoute>
              }
            >
              {/* Admin only */}
              <Route
                path="admin"
                element={
                  <RoleRoute allowedRoles={["Admin"]}>
                    <AdminDashboardPage />
                  </RoleRoute>
                }
              />

              <Route
                path="admin/clients"
                element={
                  <RoleRoute allowedRoles={["Admin"]}>
                    <ClientsPage />
                  </RoleRoute>
                }
              />

              <Route
                path="admin/trainers"
                element={
                  <RoleRoute allowedRoles={["Admin"]}>
                    <TrainersPage />
                  </RoleRoute>
                }
              />

              <Route
                path="admin/benefits"
                element={
                  <RoleRoute allowedRoles={["Admin"]}>
                    <BenefitsPage />
                  </RoleRoute>
                }
              />

              <Route
                path="admin/benefit-packages"
                element={
                  <RoleRoute allowedRoles={["Admin"]}>
                    <BenefitPackagesPage />
                  </RoleRoute>
                }
              />

              <Route
                path="admin/subscription-plans"
                element={
                  <RoleRoute allowedRoles={["Admin"]}>
                    <SubscriptionPlansPage />
                  </RoleRoute>
                }
              />
              <Route
                path="admin/subscriptions"
                element={
                  <RoleRoute allowedRoles={["Admin"]}>
                    <SubscriptionsPage />
                  </RoleRoute>
                }
              />

              <Route
                path="admin/payments"
                element={
                  <RoleRoute allowedRoles={["Admin"]}>
                    <PaymentsPage />
                  </RoleRoute>
                }
              />
              <Route
                path="admin/workout-plans"
                element={
                  <RoleRoute allowedRoles={["Admin"]}>
                    <WorkoutPlansPage />
                  </RoleRoute>
                }
              />

              {/* Trainer only */}
              <Route
                path="trainer"
                element={
                  <RoleRoute allowedRoles={["Trainer"]}>
                    <TrainerDashboard />
                  </RoleRoute>
                }
              />

              {/* Client only */}
              <Route
                path="client"
                element={
                  <RoleRoute allowedRoles={["Client"]}>
                    <ClientDashboard />
                  </RoleRoute>
                }
              />

              <Route path="*" element={<NotFound />} />
            </Route>

            <Route path="/unauthorized" element={<Unauthorized />} />
            <Route path="*" element={<NotFound />} />
          </Routes>
        </Router>
      </NavbarTriggerProvider>
    </AuthProvider>
  );
};

export default App;