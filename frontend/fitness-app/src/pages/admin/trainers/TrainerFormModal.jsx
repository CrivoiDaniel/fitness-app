import React, { useEffect, useMemo, useState } from "react";
import Modal from "../../../components/common/Modal";
import { useTranslation } from "react-i18next";

function isValidEmail(email) {
  return /^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(email);
}

const TrainerFormModal = ({ open, mode, initial, onClose, onSubmit }) => {
  const { t } = useTranslation();
  const isEdit = mode === "edit";

  const [email, setEmail] = useState("");
  const [firstName, setFirstName] = useState("");
  const [lastName, setLastName] = useState("");
  const [phoneNumber, setPhoneNumber] = useState("");
  const [specialization, setSpecialization] = useState("");
  const [yearsOfExperience, setYearsOfExperience] = useState(0);
  const [isActive, setIsActive] = useState(true);

  const [submitted, setSubmitted] = useState(false);

  useEffect(() => {
    if (!open) return;

    setSubmitted(false);

    setEmail(initial?.email ?? "");
    setFirstName(initial?.firstName ?? "");
    setLastName(initial?.lastName ?? "");
    setPhoneNumber(initial?.phoneNumber ?? "");
    setSpecialization(initial?.specialization ?? "");
    setYearsOfExperience(initial?.yearsOfExperience ?? 0);
    setIsActive(initial?.isActive ?? true);
  }, [open, initial]);

  const emailError = useMemo(() => {
    if (!email) return t("admin.trainers.form.emailRequired");
    if (!isValidEmail(email)) return t("admin.trainers.form.emailInvalid");
    return "";
  }, [email, t]);

  const firstNameError = useMemo(() => {
    if (!firstName) return t("admin.trainers.form.firstNameRequired");
    return "";
  }, [firstName, t]);

  const lastNameError = useMemo(() => {
    if (!lastName) return t("admin.trainers.form.lastNameRequired");
    return "";
  }, [lastName, t]);

  const specError = useMemo(() => {
    if (!specialization) return t("admin.trainers.form.specializationRequired");
    return "";
  }, [specialization, t]);

  const yoeError = useMemo(() => {
    if (yearsOfExperience === "" || yearsOfExperience === null || yearsOfExperience === undefined)
      return t("admin.trainers.form.yearsRequired");
    const num = Number(yearsOfExperience);
    if (Number.isNaN(num)) return t("admin.trainers.form.yearsInvalid");
    if (num < 0 || num > 50) return t("admin.trainers.form.yearsRange");
    return "";
  }, [yearsOfExperience, t]);

  const canSubmit = !emailError && !firstNameError && !lastNameError && !specError && !yoeError;

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
          specialization,
          yearsOfExperience: Number(yearsOfExperience),
          isActive,
        }
      : {
          email,
          firstName,
          lastName,
          phoneNumber: phoneNumber || null,
          specialization,
          yearsOfExperience: Number(yearsOfExperience),
        };

    onSubmit(dto);
  };

  return (
    <Modal
      open={open}
      title={isEdit ? t("admin.trainers.form.editTitle") : t("admin.trainers.form.createTitle")}
      onClose={onClose}
    >
      <form onSubmit={handleSubmit} className="space-y-3">
        <div>
          <label className="text-sm font-semibold">{t("admin.trainers.form.email")}</label>
          <input className="w-full border rounded p-2" value={email} onChange={(e) => setEmail(e.target.value)} />
          {submitted && emailError && <p className="text-sm text-red-600 mt-1">{emailError}</p>}
        </div>

        <div className="grid grid-cols-2 gap-3">
          <div>
            <label className="text-sm font-semibold">{t("admin.trainers.form.firstName")}</label>
            <input className="w-full border rounded p-2" value={firstName} onChange={(e) => setFirstName(e.target.value)} />
            {submitted && firstNameError && <p className="text-sm text-red-600 mt-1">{firstNameError}</p>}
          </div>
          <div>
            <label className="text-sm font-semibold">{t("admin.trainers.form.lastName")}</label>
            <input className="w-full border rounded p-2" value={lastName} onChange={(e) => setLastName(e.target.value)} />
            {submitted && lastNameError && <p className="text-sm text-red-600 mt-1">{lastNameError}</p>}
          </div>
        </div>

        <div>
          <label className="text-sm font-semibold">{t("admin.trainers.form.phone")}</label>
          <input className="w-full border rounded p-2" value={phoneNumber} onChange={(e) => setPhoneNumber(e.target.value)} />
        </div>

        <div>
          <label className="text-sm font-semibold">{t("admin.trainers.form.specialization")}</label>
          <input className="w-full border rounded p-2" value={specialization} onChange={(e) => setSpecialization(e.target.value)} />
          {submitted && specError && <p className="text-sm text-red-600 mt-1">{specError}</p>}
        </div>

        <div>
          <label className="text-sm font-semibold">{t("admin.trainers.form.years")}</label>
          <input
            type="number"
            min={0}
            max={50}
            className="w-full border rounded p-2"
            value={yearsOfExperience}
            onChange={(e) => setYearsOfExperience(e.target.value)}
          />
          {submitted && yoeError && <p className="text-sm text-red-600 mt-1">{yoeError}</p>}
        </div>

        {isEdit && (
          <label className="flex items-center gap-2">
            <input type="checkbox" checked={!!isActive} onChange={(e) => setIsActive(e.target.checked)} />
            <span className="text-sm">{t("admin.trainers.form.active")}</span>
          </label>
        )}

        <div className="pt-2 flex justify-end gap-2">
          <button type="button" className="px-4 py-2 rounded border" onClick={onClose}>
            {t("common.cancel")}
          </button>
          <button type="submit" className="px-4 py-2 rounded bg-black text-white font-semibold hover:bg-gray-800">
            {isEdit ? t("admin.trainers.form.save") : t("admin.trainers.form.create")}
          </button>
        </div>
      </form>
    </Modal>
  );
};

export default TrainerFormModal;