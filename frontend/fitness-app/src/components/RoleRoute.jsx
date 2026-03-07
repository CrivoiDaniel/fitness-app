import React from "react";
import { Navigate } from "react-router-dom";
import { useAuth } from "../context/AuthContext";

const RoleRoute = ({ allowedRoles, children }) => {
  const { token, user } = useAuth();

  if (!token) return <Navigate to="/login" replace />;

  // user.role este numeric la tine (0=Admin)
  if (!allowedRoles.includes(user?.role)) {
    // în proiecte reale: fie 403 page, fie redirect la dashboard-ul corect
    return <Navigate to="/unauthorized" replace />;
  }

  return children;
};

export default RoleRoute;