import React, { useEffect, useMemo, useState } from "react";
import Modal from "../../../components/common/Modal";
import { useTranslation } from "react-i18next";

function isValidKey(name) {
  return /^[a-z_]+$/.test(name);
}

const BenefitFormModal = ({ open, mode, initial, onClose, onSubmit }) => {
  const { t } = useTranslation();
  const isEdit = mode === "edit";

  const [name, setName] = useState("");
  const [displayName, setDisplayName] = useState("");
  const [description, setDescription] = useState("");
  const [isActive, setIsActive] = useState(true);

  const [submitted, setSubmitted] = useState(false);

  useEffect(() => {
    if (!open) return;
    setSubmitted(false);

    setName(initial?.name ?? "");
    setDisplayName(initial?.displayName ?? "");
    setDescription(initial?.description ?? "");
    setIsActive(initial?.isActive ?? true);
  }, [open, initial]);

  const nameError = useMemo(() => {
    if (isEdit) return "";
    if (!name) return t("admin.benefits.form.nameRequired");
    if (!isValidKey(name)) return t("admin.benefits.form.nameInvalid");
    return "";
  }, [name, isEdit, t]);

  const displayNameError = useMemo(() => {
    if (!displayName) return t("admin.benefits.form.displayNameRequired");
    return "";
  }, [displayName, t]);

  const canSubmit = !nameError && !displayNameError;

  const handleSubmit = (e) => {
    e.preventDefault();
    setSubmitted(true);
    if (!canSubmit) return;

    if (isEdit) {
      onSubmit({
        displayName,
        description: description || null,
        isActive: !!isActive,
      });
      return;
    }

    onSubmit({
      name,
      displayName,
      description: description || null,
    });
  };

  return (
    <Modal
      open={open}
      title={isEdit ? t("admin.benefits.form.editTitle") : t("admin.benefits.form.createTitle")}
      onClose={onClose}
    >
      <form onSubmit={handleSubmit} className="space-y-3">
        {!isEdit && (
          <div>
            <label className="text-sm font-semibold">{t("admin.benefits.form.name")}</label>
            <input
              className="w-full border rounded p-2"
              value={name}
              onChange={(e) => setName(e.target.value)}
              placeholder="ex: pool_access"
            />
            {submitted && nameError && <p className="text-sm text-red-600 mt-1">{nameError}</p>}
            <p className="text-xs text-gray-500 mt-1">{t("admin.benefits.form.nameHint")}</p>
          </div>
        )}

        <div>
          <label className="text-sm font-semibold">{t("admin.benefits.form.displayName")}</label>
          <input
            className="w-full border rounded p-2"
            value={displayName}
            onChange={(e) => setDisplayName(e.target.value)}
          />
          {submitted && displayNameError && <p className="text-sm text-red-600 mt-1">{displayNameError}</p>}
        </div>

        <div>
          <label className="text-sm font-semibold">{t("admin.benefits.form.description")}</label>
          <textarea
            className="w-full border rounded p-2 min-h-[90px]"
            value={description}
            onChange={(e) => setDescription(e.target.value)}
          />
        </div>

        {isEdit && (
          <label className="flex items-center gap-2">
            <input type="checkbox" checked={!!isActive} onChange={(e) => setIsActive(e.target.checked)} />
            <span className="text-sm">{t("admin.benefits.form.active")}</span>
          </label>
        )}

        <div className="pt-2 flex justify-end gap-2">
          <button type="button" className="px-4 py-2 rounded border" onClick={onClose}>
            {t("common.cancel")}
          </button>
          <button type="submit" className="px-4 py-2 rounded bg-black text-white font-semibold hover:bg-gray-800">
            {isEdit ? t("admin.benefits.form.save") : t("admin.benefits.form.create")}
          </button>
        </div>
      </form>
    </Modal>
  );
};

export default BenefitFormModal;