import React, { useMemo, useState } from "react";
import { useNavigate } from "react-router-dom";
import { loginRequest } from "../../api/auth";
import { useAuth } from "../../context/AuthContext";
import { useTranslation } from "react-i18next";

const BASE_URL = "http://localhost:5140";

function isValidEmail(email) {
  return /^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(email);
}

function roleToDashboard(role) {
  if (role === "Admin") return "/dashboard/admin/clients";
  if (role === "Trainer") return "/dashboard/trainer";
  return "/dashboard/client";
}

const Login = () => {
  const { t } = useTranslation();
  const navigate = useNavigate();
  const { login } = useAuth();

  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");

  const [touched, setTouched] = useState({ email: false, password: false });
  const [submitted, setSubmitted] = useState(false);

  const [loading, setLoading] = useState(false);
  const [serverError, setServerError] = useState("");

  const emailError = useMemo(() => {
    if (!email) return t("auth.emailRequired");
    if (!isValidEmail(email)) return t("auth.emailInvalid");
    return "";
  }, [email, t]);

  const passError = useMemo(() => {
    if (!password) return t("auth.passwordRequired");
    if (password.length < 6) return t("auth.passwordMin");
    return "";
  }, [password, t]);

  const showEmailError = (submitted || touched.email) && !!emailError;
  const showPassError = (submitted || touched.password) && !!passError;

  const canSubmit = !emailError && !passError && !loading;

  const onSubmit = async (e) => {
    e.preventDefault();
    setSubmitted(true);
    setServerError("");

    if (!canSubmit) return;

    try {
      setLoading(true);

      const data = await loginRequest(BASE_URL, { email, password });

      if (!data?.token) {
        console.error("Login response missing token:", data);
        throw new Error(t("auth.missingToken"));
      }

      login(data);

      if (data.mustChangePassword === true) {
        navigate("/change-password", { replace: true });
        return;
      }

      navigate(roleToDashboard(data.role), { replace: true });
    } catch (err) {
      setServerError(err.message || t("auth.loginFailed"));
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="min-h-screen bg-black flex items-center justify-center px-4">
      <form
        onSubmit={onSubmit}
        className="w-full max-w-md bg-white/10 backdrop-blur border border-white/10 rounded-2xl p-8"
      >
        <h1 className="text-3xl font-extrabold italic text-white mb-6">
          {t("auth.loginTitle")}
        </h1>

        <label className="block text-white/80 text-sm mb-2">
          {t("auth.email")}
        </label>
        <input
          className="w-full rounded-md p-3 mb-2 bg-black/40 text-white border border-white/10 outline-none focus:border-yellow-400/80"
          value={email}
          onChange={(e) => setEmail(e.target.value)}
          onBlur={() => setTouched((tt) => ({ ...tt, email: true }))}
          placeholder="admin@fitness.com"
          type="email"
        />
        {showEmailError && <p className="text-red-400 text-sm mb-3">{emailError}</p>}

        <label className="block text-white/80 text-sm mb-2">
          {t("auth.password")}
        </label>
        <input
          className="w-full rounded-md p-3 mb-2 bg-black/40 text-white border border-white/10 outline-none focus:border-yellow-400/80"
          value={password}
          onChange={(e) => setPassword(e.target.value)}
          onBlur={() => setTouched((tt) => ({ ...tt, password: true }))}
          placeholder="••••••••"
          type="password"
        />
        {showPassError && <p className="text-red-400 text-sm mb-3">{passError}</p>}

        {serverError && <p className="text-red-400 text-sm mb-3">{serverError}</p>}

        <button
          type="submit"
          disabled={!canSubmit}
          className={[
            "w-full py-3 rounded-md font-semibold transition-colors duration-300",
            canSubmit
              ? "bg-yellow-400/80 text-black hover:bg-yellow-300"
              : "bg-gray-600 text-gray-300 cursor-not-allowed",
          ].join(" ")}
        >
          {loading ? t("auth.connecting") : t("auth.connect")}
        </button>
      </form>
    </div>
  );
};

export default Login;