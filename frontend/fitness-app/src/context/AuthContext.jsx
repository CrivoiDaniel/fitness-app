import React, { createContext, useContext, useMemo, useState } from "react";

const AuthContext = createContext(null);

function loadAuthFromStorage() {
  try {
    const raw = localStorage.getItem("auth");
    return raw ? JSON.parse(raw) : null;
  } catch {
    return null;
  }
}

export const AuthProvider = ({ children }) => {
  const [auth, setAuth] = useState(() => loadAuthFromStorage());

  const token = auth?.token || "";

  const user = useMemo(() => {
    if (!auth) return null;
    return {
      userId: auth.userId,
      email: auth.email,
      firstName: auth.firstName,
      lastName: auth.lastName,
      role: auth.role, // numeric (0/1/2 etc)
      clientId: auth.clientId ?? null,
      trainerId: auth.trainerId ?? null,
    };
  }, [auth]);

  const login = (loginResponse) => {
    // loginResponse este exact ce primești de la backend (camelCase)
    localStorage.setItem("auth", JSON.stringify(loginResponse));
    setAuth(loginResponse);
  };

  const logout = () => {
    localStorage.removeItem("auth");
    setAuth(null);
  };

  return (
    <AuthContext.Provider value={{ token, user, auth, login, logout }}>
      {children}
    </AuthContext.Provider>
  );
};

export const useAuth = () => {
  const ctx = useContext(AuthContext);
  if (!ctx) throw new Error("useAuth must be used inside AuthProvider");
  return ctx;
};