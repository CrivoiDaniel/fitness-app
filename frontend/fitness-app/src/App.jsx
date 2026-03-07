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

            {/* DASHBOARD (fără navbar) - sidebar modern */}
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
                  <RoleRoute allowedRoles={[0]}>
                    <AdminDashboard />
                  </RoleRoute>
                }
              />

              <Route
                path="admin/clients"
                element={
                  <RoleRoute allowedRoles={[0]}>
                    <ClientsPage />
                  </RoleRoute>
                }
              />
              
              <Route
                path="admin/trainers"
                element={
                  <RoleRoute allowedRoles={[0]}>
                    <TrainersPage />
                  </RoleRoute>
                }
              />

              {/* Trainer only */}
              <Route
                path="trainer"
                element={
                  <RoleRoute allowedRoles={[2]}>
                    <TrainerDashboard />
                  </RoleRoute>
                }
              />

              {/* Client only */}
              <Route
                path="client"
                element={
                  <RoleRoute allowedRoles={[1]}>
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