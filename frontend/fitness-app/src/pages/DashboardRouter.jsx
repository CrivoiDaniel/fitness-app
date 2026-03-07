import React, { useEffect } from "react";
import { useNavigate } from "react-router-dom";
import { useAuth } from "../context/AuthContext";

const DashboardRouter = () => {
  const { user } = useAuth();
  const navigate = useNavigate();

  useEffect(() => {
    if (!user?.role) return;

    if (user.role === "Admin") navigate("/dashboard/admin", { replace: true });
    else if (user.role === "Trainer") navigate("/dashboard/trainer", { replace: true });
    else navigate("/dashboard/client", { replace: true });
  }, [user, navigate]);

  return (
    <div className="min-h-screen bg-black text-white flex items-center justify-center">
      Se încarcă dashboard...
    </div>
  );
};

export default DashboardRouter;