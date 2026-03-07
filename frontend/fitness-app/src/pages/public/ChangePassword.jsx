import React, { useMemo, useState } from "react";
import { useNavigate } from "react-router-dom";
import { useAuth } from "../../context/AuthContext";
import { changePassword } from "../../api/auth";
import { useTranslation } from "react-i18next";

const ChangePassword = () => {
  const { t } = useTranslation();
  const { token, logout } = useAuth();
  const navigate = useNavigate();

  const [currentPassword, setCurrentPassword] = useState("");
  const [newPassword, setNewPassword] = useState("");
  const [confirmNewPassword, setConfirmNewPassword] = useState("");

  const [loading, setLoading] = useState(false);
  const [error, setError] = useState("");
  const [success, setSuccess] = useState("");

  const newPasswordError = useMemo(() => {
    if (!newPassword) return t("changePassword.newRequired");
    if (newPassword.length < 6) return t("changePassword.newMin");
    return "";
  }, [newPassword, t]);

  const confirmError = useMemo(() => {
    if (!confirmNewPassword) return t("changePassword.confirmRequired");
    if (confirmNewPassword !== newPassword) return t("changePassword.confirmMismatch");
    return "";
  }, [confirmNewPassword, newPassword, t]);

  const canSubmit = !!token && !newPasswordError && !confirmError && !!currentPassword && !loading;

  const onSubmit = async (e) => {
    e.preventDefault();
    setError("");
    setSuccess("");

    if (!canSubmit) return;

    try {
      setLoading(true);
      await changePassword(token, { currentPassword, newPassword });
      setSuccess(t("changePassword.success"));

      setTimeout(() => {
        logout();
        navigate("/login", { replace: true });
      }, 800);
    } catch (err) {
      setError(err.message || "Nu am putut schimba parola.");
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="min-h-screen flex items-center justify-center px-4 bg-gray-100">
      <form onSubmit={onSubmit} className="w-full max-w-md bg-white border rounded-xl p-8">
        <h1 className="text-2xl font-bold mb-2">{t("changePassword.title")}</h1>
        <p className="text-gray-600 mb-6">{t("changePassword.subtitle")}</p>

        <label className="text-sm font-semibold">{t("changePassword.current")}</label>
        <input className="w-full border rounded p-2 mb-3" type="password" value={currentPassword} onChange={(e) => setCurrentPassword(e.target.value)} />

        <label className="text-sm font-semibold">{t("changePassword.new")}</label>
        <input className="w-full border rounded p-2 mb-1" type="password" value={newPassword} onChange={(e) => setNewPassword(e.target.value)} />
        {newPasswordError && <p className="text-sm text-red-600 mb-3">{newPasswordError}</p>}

        <label className="text-sm font-semibold">{t("changePassword.confirmNew")}</label>
        <input className="w-full border rounded p-2 mb-1" type="password" value={confirmNewPassword} onChange={(e) => setConfirmNewPassword(e.target.value)} />
        {confirmError && <p className="text-sm text-red-600 mb-3">{confirmError}</p>}

        {error && <div className="mb-3 p-2 rounded bg-red-50 text-red-700 border">{error}</div>}
        {success && <div className="mb-3 p-2 rounded bg-green-50 text-green-700 border">{success}</div>}

        <button type="submit" disabled={!canSubmit} className="w-full mt-2 px-4 py-2 rounded bg-black text-white font-semibold disabled:opacity-50">
          {loading ? t("changePassword.saving") : t("changePassword.save")}
        </button>
      </form>
    </div>
  );
};

export default ChangePassword;