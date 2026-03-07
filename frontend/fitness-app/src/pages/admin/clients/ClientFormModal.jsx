import React, { useEffect, useMemo, useState } from "react";
import Modal from "../../../components/common/Modal";
import { useTranslation } from "react-i18next";

function isValidEmail(email) {
  return /^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(email);
}

const ClientFormModal = ({ open, mode, initial, onClose, onSubmit }) => {
  const { t } = useTranslation();
  const isEdit = mode === "edit";

  const [email, setEmail] = useState("");
  const [firstName, setFirstName] = useState("");
  const [lastName, setLastName] = useState("");
  const [phoneNumber, setPhoneNumber] = useState("");
  const [dateOfBirth, setDateOfBirth] = useState("");
  const [isActive, setIsActive] = useState(true);

  const [submitted, setSubmitted] = useState(false);

  useEffect(() => {
    if (!open) return;

    setSubmitted(false);

    setEmail(initial?.email ?? "");
    setFirstName(initial?.firstName ?? "");
    setLastName(initial?.lastName ?? "");
    setPhoneNumber(initial?.phoneNumber ?? "");
    setDateOfBirth(initial?.dateOfBirth ? String(initial.dateOfBirth).slice(0, 10) : "");
    setIsActive(initial?.isActive ?? true);
  }, [open, initial]);

  const emailError = useMemo(() => {
    if (!email) return t("admin.clients.form.emailRequired");
    if (!isValidEmail(email)) return t("admin.clients.form.emailInvalid");
    return "";
  }, [email, t]);

  const firstNameError = useMemo(() => {
    if (!firstName) return t("admin.clients.form.firstNameRequired");
    return "";
  }, [firstName, t]);

  const lastNameError = useMemo(() => {
    if (!lastName) return t("admin.clients.form.lastNameRequired");
    return "";
  }, [lastName, t]);

  const dobError = useMemo(() => {
    if (!dateOfBirth) return t("admin.clients.form.dobRequired");
    return "";
  }, [dateOfBirth, t]);

  const canSubmit = !emailError && !firstNameError && !lastNameError && !dobError;

  const handleSubmit = (e) => {
    e.preventDefault();
    setSubmitted(true);
    if (!canSubmit) return;

    const dto = isEdit
      ? {
          email,
          firstName,
          lastName,
          phoneNumber: phoneNumber || null,
          dateOfBirth,
          isActive,
        }
      : {
          email,
          firstName,
          lastName,
          phoneNumber: phoneNumber || null,
          dateOfBirth,
        };

    onSubmit(dto);
  };

  return (
    <Modal
      open={open}
      title={isEdit ? t("admin.clients.form.editTitle") : t("admin.clients.form.createTitle")}
      onClose={onClose}
    >
      <form onSubmit={handleSubmit} className="space-y-3">
        <div>
          <label className="text-sm font-semibold">{t("admin.clients.form.email")}</label>
          <input className="w-full border rounded p-2" value={email} onChange={(e) => setEmail(e.target.value)} />
          {submitted && emailError && <p className="text-sm text-red-600 mt-1">{emailError}</p>}
        </div>

        <div className="grid grid-cols-2 gap-3">
          <div>
            <label className="text-sm font-semibold">{t("admin.clients.form.firstName")}</label>
            <input className="w-full border rounded p-2" value={firstName} onChange={(e) => setFirstName(e.target.value)} />
            {submitted && firstNameError && <p className="text-sm text-red-600 mt-1">{firstNameError}</p>}
          </div>
          <div>
            <label className="text-sm font-semibold">{t("admin.clients.form.lastName")}</label>
            <input className="w-full border rounded p-2" value={lastName} onChange={(e) => setLastName(e.target.value)} />
            {submitted && lastNameError && <p className="text-sm text-red-600 mt-1">{lastNameError}</p>}
          </div>
        </div>

        <div>
          <label className="text-sm font-semibold">{t("admin.clients.form.phone")}</label>
          <input className="w-full border rounded p-2" value={phoneNumber} onChange={(e) => setPhoneNumber(e.target.value)} />
        </div>

        <div>
          <label className="text-sm font-semibold">{t("admin.clients.form.dob")}</label>
          <input type="date" className="w-full border rounded p-2" value={dateOfBirth} onChange={(e) => setDateOfBirth(e.target.value)} />
          {submitted && dobError && <p className="text-sm text-red-600 mt-1">{dobError}</p>}
        </div>

        {isEdit && (
          <label className="flex items-center gap-2">
            <input type="checkbox" checked={!!isActive} onChange={(e) => setIsActive(e.target.checked)} />
            <span className="text-sm">{t("admin.clients.form.active")}</span>
          </label>
        )}

        <div className="pt-2 flex justify-end gap-2">
          <button type="button" className="px-4 py-2 rounded border" onClick={onClose}>
            {t("common.cancel")}
          </button>
          <button type="submit" className="px-4 py-2 rounded bg-black text-white font-semibold hover:bg-gray-800">
            {isEdit ? t("admin.clients.form.save") : t("admin.clients.form.create")}
          </button>
        </div>
      </form>
    </Modal>
  );
};

export default ClientFormModal;