import React from "react";
import { Navigate, useLocation } from "react-router-dom";
import { useAuth } from "../context/AuthContext";

const RoleRoute = ({ allowedRoles = [], children }) => {
  const { user } = useAuth();
  const location = useLocation();

  // debug (poți șterge după ce merge)
  console.log("RoleRoute user:", user);
  console.log("RoleRoute allowedRoles:", allowedRoles);

  if (!user) return <Navigate to="/login" replace state={{ from: location }} />;

  const role = user.role; // trebuie să fie "Admin" | "Client" | "Trainer"

  if (!role) return <Navigate to="/unauthorized" replace />;

  // allowedRoles pot fi ["Admin"] etc.
  if (!allowedRoles.includes(role)) return <Navigate to="/unauthorized" replace />;

  return children;
};

export default RoleRoute;