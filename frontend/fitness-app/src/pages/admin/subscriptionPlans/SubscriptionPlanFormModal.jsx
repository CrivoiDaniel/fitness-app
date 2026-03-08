import React, { useEffect, useMemo, useState } from "react";
import Modal from "../../../components/common/Modal";
import { useTranslation } from "react-i18next";

const TYPES = ["Monthly", "Quarterly", "Yearly"];

const SubscriptionPlanFormModal = ({
  open,
  mode,
  initial,
  benefitPackages,
  loadingPackages,
  onClose,
  onSubmit,
}) => {
  const { t } = useTranslation();
  const isEdit = mode === "edit";

  const [type, setType] = useState("Monthly");
  const [durationInMonths, setDurationInMonths] = useState(1);
  const [price, setPrice] = useState("");
  const [benefitPackageId, setBenefitPackageId] = useState("");
  const [isRecurring, setIsRecurring] = useState(false);
  const [allowInstallments, setAllowInstallments] = useState(false);
  const [maxInstallments, setMaxInstallments] = useState(1);
  const [isActive, setIsActive] = useState(true);

  const [submitted, setSubmitted] = useState(false);

  useEffect(() => {
    if (!open) return;
    setSubmitted(false);

    if (!isEdit) {
      setType("Monthly");
      setDurationInMonths(1);
      setPrice("");
      setBenefitPackageId("");
      setIsRecurring(false);
      setAllowInstallments(false);
      setMaxInstallments(1);
      setIsActive(true);
      return;
    }

    setType(initial?.type ?? "Monthly");
    setDurationInMonths(initial?.durationInMonths ?? 1);
    setPrice(initial?.price ?? "");
    setBenefitPackageId(initial?.benefitPackageId ?? "");
    setIsRecurring(!!initial?.isRecurring);
    setAllowInstallments(!!initial?.allowInstallments);
    setMaxInstallments(initial?.maxInstallments ?? 1);
    setIsActive(initial?.isActive ?? true);
  }, [open, isEdit, initial]);

  const priceError = useMemo(() => {
    if (price === "" || price === null || price === undefined) return t("admin.subscriptionPlans.form.priceRequired");
    const v = Number(price);
    if (Number.isNaN(v) || v <= 0) return t("admin.subscriptionPlans.form.priceInvalid");
    return "";
  }, [price, t]);

  const durationError = useMemo(() => {
    if (isEdit) return "";
    const v = Number(durationInMonths);
    if (Number.isNaN(v) || v < 0 || v > 120) return t("admin.subscriptionPlans.form.durationInvalid");
    return "";
  }, [durationInMonths, isEdit, t]);

  const pkgError = useMemo(() => {
    if (!benefitPackageId) return t("admin.subscriptionPlans.form.packageRequired");
    return "";
  }, [benefitPackageId, t]);

  const maxInstError = useMemo(() => {
    const v = Number(maxInstallments);
    if (Number.isNaN(v) || v < 1 || v > 24) return t("admin.subscriptionPlans.form.maxInstallmentsInvalid");
    return "";
  }, [maxInstallments, t]);

  const canSubmit = !priceError && !durationError && !pkgError && !maxInstError && !loadingPackages;

  const handleSubmit = (e) => {
    e.preventDefault();
    setSubmitted(true);
    if (!canSubmit) return;

    const safeMaxInstallments = allowInstallments ? Number(maxInstallments) : 1;

    if (!isEdit) {
      onSubmit({
        type, // string enum (Monthly/Quarterly/Yearly)
        durationInMonths: Number(durationInMonths),
        price: Number(price),
        benefitPackageId: Number(benefitPackageId),
        isRecurring: !!isRecurring,
        allowInstallments: !!allowInstallments,
        maxInstallments: safeMaxInstallments,
      });
      return;
    }

    onSubmit({
      price: Number(price),
      benefitPackageId: Number(benefitPackageId),
      isRecurring: !!isRecurring,
      allowInstallments: !!allowInstallments,
      maxInstallments: safeMaxInstallments,
      isActive: !!isActive,
    });
  };

  return (
    <Modal
      open={open}
      title={isEdit ? t("admin.subscriptionPlans.form.editTitle") : t("admin.subscriptionPlans.form.createTitle")}
      onClose={onClose}
    >
      <form onSubmit={handleSubmit} className="space-y-3">
        {!isEdit && (
          <div className="grid grid-cols-2 gap-3">
            <div>
              <label className="text-sm font-semibold">{t("admin.subscriptionPlans.form.type")}</label>
              <select className="w-full border rounded p-2" value={type} onChange={(e) => setType(e.target.value)}>
                {TYPES.map((x) => (
                  <option key={x} value={x}>
                    {t(`subscription.type.${x}`, x)}
                  </option>
                ))}
              </select>
            </div>

            <div>
              <label className="text-sm font-semibold">{t("admin.subscriptionPlans.form.duration")}</label>
              <input
                type="number"
                min={0}
                max={120}
                className="w-full border rounded p-2"
                value={durationInMonths}
                onChange={(e) => setDurationInMonths(e.target.value)}
              />
              {submitted && durationError && <p className="text-sm text-red-600 mt-1">{durationError}</p>}
            </div>
          </div>
        )}

        <div className="grid grid-cols-2 gap-3">
          <div>
            <label className="text-sm font-semibold">{t("admin.subscriptionPlans.form.price")}</label>
            <input
              type="number"
              step="0.01"
              min="0"
              className="w-full border rounded p-2"
              value={price}
              onChange={(e) => setPrice(e.target.value)}
            />
            {submitted && priceError && <p className="text-sm text-red-600 mt-1">{priceError}</p>}
          </div>

          <div>
            <label className="text-sm font-semibold">{t("admin.subscriptionPlans.form.package")}</label>
            <select
              className="w-full border rounded p-2"
              value={benefitPackageId}
              onChange={(e) => setBenefitPackageId(e.target.value)}
              disabled={loadingPackages}
            >
              <option value="">{t("admin.subscriptionPlans.form.selectPackage")}</option>
              {benefitPackages.map((p) => (
                <option key={p.id} value={p.id}>
                  {p.name} ({p.items?.length ?? 0})
                </option>
              ))}
            </select>
            {submitted && pkgError && <p className="text-sm text-red-600 mt-1">{pkgError}</p>}
          </div>
        </div>

        <div className="grid grid-cols-2 gap-3">
          <label className="flex items-center gap-2">
            <input type="checkbox" checked={!!isRecurring} onChange={(e) => setIsRecurring(e.target.checked)} />
            <span className="text-sm">{t("admin.subscriptionPlans.form.recurring")}</span>
          </label>

          <label className="flex items-center gap-2">
            <input type="checkbox" checked={!!allowInstallments} onChange={(e) => setAllowInstallments(e.target.checked)} />
            <span className="text-sm">{t("admin.subscriptionPlans.form.installments")}</span>
          </label>
        </div>

        <div>
          <label className="text-sm font-semibold">{t("admin.subscriptionPlans.form.maxInstallments")}</label>
          <input
            type="number"
            min={1}
            max={24}
            className="w-full border rounded p-2"
            value={maxInstallments}
            onChange={(e) => setMaxInstallments(e.target.value)}
            disabled={!allowInstallments}
          />
          {submitted && maxInstError && <p className="text-sm text-red-600 mt-1">{maxInstError}</p>}
        </div>

        {isEdit && (
          <label className="flex items-center gap-2">
            <input type="checkbox" checked={!!isActive} onChange={(e) => setIsActive(e.target.checked)} />
            <span className="text-sm">{t("admin.subscriptionPlans.form.active")}</span>
          </label>
        )}

        <div className="pt-2 flex justify-end gap-2">
          <button type="button" className="px-4 py-2 rounded border" onClick={onClose}>
            {t("common.cancel")}
          </button>
          <button type="submit" className="px-4 py-2 rounded bg-black text-white font-semibold hover:bg-gray-800" disabled={!canSubmit}>
            {isEdit ? t("admin.subscriptionPlans.form.save") : t("admin.subscriptionPlans.form.create")}
          </button>
        </div>
      </form>
    </Modal>
  );
};

export default SubscriptionPlanFormModal;