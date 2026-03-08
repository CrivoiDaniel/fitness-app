import React, { useEffect, useMemo, useState } from "react";
import Modal from "../../../components/common/Modal";
import { useTranslation } from "react-i18next";

const emptyItem = () => ({ benefitId: "", value: "" });

const BenefitPackageFormModal = ({
  open,
  mode,
  initial, // BenefitPackageDto (fără items complete uneori)
  availableBenefits, // [{id,name,displayName,isActive}]
  loadingBenefits,
  onClose,
  onSubmit,
  loadFullPackage, // async (id) => package with items
}) => {
  const { t } = useTranslation();
  const isEdit = mode === "edit";

  const [name, setName] = useState("");
  const [scheduleWeekday, setScheduleWeekday] = useState("");
  const [scheduleWeekend, setScheduleWeekend] = useState("");
  const [isActive, setIsActive] = useState(true);

  const [items, setItems] = useState([emptyItem()]);
  const [submitted, setSubmitted] = useState(false);

  const [loadingPackage, setLoadingPackage] = useState(false);
  const [packageError, setPackageError] = useState("");

  useEffect(() => {
    if (!open) return;

    setSubmitted(false);
    setPackageError("");

    // set default values
    setName(initial?.name ?? "");
    setScheduleWeekday(initial?.scheduleWeekday ?? "");
    setScheduleWeekend(initial?.scheduleWeekend ?? "");
    setIsActive(initial?.isActive ?? true);

    // create mode: one empty item
    if (!isEdit) {
      setItems([emptyItem()]);
      return;
    }

    // edit mode: load full with items
    const id = initial?.id;
    if (!id || !loadFullPackage) {
      setItems([emptyItem()]);
      return;
    }

    (async () => {
      try {
        setLoadingPackage(true);
        const full = await loadFullPackage(id);

        setName(full?.name ?? "");
        setScheduleWeekday(full?.scheduleWeekday ?? "");
        setScheduleWeekend(full?.scheduleWeekend ?? "");
        setIsActive(full?.isActive ?? true);

        const mapped =
          full?.items?.length > 0
            ? full.items.map((it) => ({
                benefitId: it.benefitId,
                value: it.value ?? "",
              }))
            : [emptyItem()];

        setItems(mapped);
      } catch (e) {
        setPackageError(e.message || t("admin.benefitPackages.errors.loadOne"));
        setItems([emptyItem()]);
      } finally {
        setLoadingPackage(false);
      }
    })();
  }, [open, isEdit, initial, loadFullPackage, t]);

  const nameError = useMemo(() => (!name ? t("admin.benefitPackages.form.nameRequired") : ""), [name, t]);
  const weekdayError = useMemo(
    () => (!scheduleWeekday ? t("admin.benefitPackages.form.weekdayRequired") : ""),
    [scheduleWeekday, t]
  );
  const weekendError = useMemo(
    () => (!scheduleWeekend ? t("admin.benefitPackages.form.weekendRequired") : ""),
    [scheduleWeekend, t]
  );

  const itemsError = useMemo(() => {
    if (!items || items.length < 1) return t("admin.benefitPackages.form.itemsMin1");

    for (let i = 0; i < items.length; i++) {
      const it = items[i];
      if (!it.benefitId) return t("admin.benefitPackages.form.itemBenefitRequired");
      if (!it.value) return t("admin.benefitPackages.form.itemValueRequired");
    }

    // optional: prevent duplicate benefitId
    const ids = items.map((x) => Number(x.benefitId));
    const set = new Set(ids);
    if (set.size !== ids.length) return t("admin.benefitPackages.form.itemsNoDuplicates");

    return "";
  }, [items, t]);

  const canSubmit = !nameError && !weekdayError && !weekendError && !itemsError && !loadingBenefits && !loadingPackage;

  const addItem = () => setItems((prev) => [...prev, emptyItem()]);
  const removeItem = (idx) => setItems((prev) => prev.filter((_, i) => i !== idx));

  const setItem = (idx, patch) => {
    setItems((prev) => prev.map((it, i) => (i === idx ? { ...it, ...patch } : it)));
  };

  const handleSubmit = (e) => {
    e.preventDefault();
    setSubmitted(true);
    if (!canSubmit) return;

    const dto = {
      name,
      scheduleWeekday,
      scheduleWeekend,
      isActive: !!isActive,
      items: items.map((it) => ({
        benefitId: Number(it.benefitId),
        value: it.value,
      })),
    };

    // create dto în backend nu are isActive (în CreateBenefitPackageDto), dar nu strică să nu-l trimitem:
    if (!isEdit) delete dto.isActive;

    onSubmit(dto);
  };

  return (
    <Modal
      open={open}
      title={isEdit ? t("admin.benefitPackages.form.editTitle") : t("admin.benefitPackages.form.createTitle")}
      onClose={onClose}
    >
      {packageError && (
        <div className="mb-3 p-3 rounded bg-red-50 text-red-700 border">{packageError}</div>
      )}

      {(loadingBenefits || loadingPackage) && (
        <div className="mb-3 p-3 rounded bg-gray-50 text-gray-700 border">
          {t("common.loading")}
        </div>
      )}

      <form onSubmit={handleSubmit} className="space-y-3">
        <div>
          <label className="text-sm font-semibold">{t("admin.benefitPackages.form.name")}</label>
          <input className="w-full border rounded p-2" value={name} onChange={(e) => setName(e.target.value)} />
          {submitted && nameError && <p className="text-sm text-red-600 mt-1">{nameError}</p>}
        </div>

        <div className="grid grid-cols-2 gap-3">
          <div>
            <label className="text-sm font-semibold">{t("admin.benefitPackages.form.scheduleWeekday")}</label>
            <input
              className="w-full border rounded p-2"
              value={scheduleWeekday}
              onChange={(e) => setScheduleWeekday(e.target.value)}
              placeholder="ex: Mon-Fri 08:00-22:00"
            />
            {submitted && weekdayError && <p className="text-sm text-red-600 mt-1">{weekdayError}</p>}
          </div>

          <div>
            <label className="text-sm font-semibold">{t("admin.benefitPackages.form.scheduleWeekend")}</label>
            <input
              className="w-full border rounded p-2"
              value={scheduleWeekend}
              onChange={(e) => setScheduleWeekend(e.target.value)}
              placeholder="ex: Sat-Sun 10:00-20:00"
            />
            {submitted && weekendError && <p className="text-sm text-red-600 mt-1">{weekendError}</p>}
          </div>
        </div>

        {isEdit && (
          <label className="flex items-center gap-2">
            <input type="checkbox" checked={!!isActive} onChange={(e) => setIsActive(e.target.checked)} />
            <span className="text-sm">{t("admin.benefitPackages.form.active")}</span>
          </label>
        )}

        <div className="pt-2">
          <div className="flex items-center justify-between">
            <div className="font-semibold">{t("admin.benefitPackages.form.items")}</div>
            <button type="button" className="px-3 py-1 rounded border" onClick={addItem}>
              {t("admin.benefitPackages.form.addItem")}
            </button>
          </div>

          {submitted && itemsError && <p className="text-sm text-red-600 mt-2">{itemsError}</p>}

          <div className="mt-3 space-y-2">
            {items.map((it, idx) => (
              <div key={idx} className="grid grid-cols-12 gap-2 items-center">
                <div className="col-span-5">
                  <select
                    className="w-full border rounded p-2"
                    value={it.benefitId}
                    onChange={(e) => setItem(idx, { benefitId: e.target.value })}
                    disabled={loadingBenefits || loadingPackage}
                  >
                    <option value="">{t("admin.benefitPackages.form.selectBenefit")}</option>
                    {availableBenefits.map((b) => (
                      <option key={b.id} value={b.id}>
                        {b.displayName} ({b.name})
                      </option>
                    ))}
                  </select>
                </div>

                <div className="col-span-6">
                  <input
                    className="w-full border rounded p-2"
                    value={it.value}
                    onChange={(e) => setItem(idx, { value: e.target.value })}
                    placeholder={t("admin.benefitPackages.form.valuePlaceholder")}
                    disabled={loadingBenefits || loadingPackage}
                  />
                </div>

                <div className="col-span-1 flex justify-end">
                  <button
                    type="button"
                    className="px-2 py-1 rounded border hover:bg-gray-50 disabled:opacity-50"
                    disabled={items.length <= 1 || loadingBenefits || loadingPackage}
                    onClick={() => removeItem(idx)}
                    title={t("admin.benefitPackages.form.removeItem")}
                  >
                    ×
                  </button>
                </div>
              </div>
            ))}
          </div>
        </div>

        <div className="pt-4 flex justify-end gap-2">
          <button type="button" className="px-4 py-2 rounded border" onClick={onClose}>
            {t("common.cancel")}
          </button>
          <button
            type="submit"
            className="px-4 py-2 rounded bg-black text-white font-semibold hover:bg-gray-800 disabled:opacity-50"
            disabled={!canSubmit}
          >
            {isEdit ? t("admin.benefitPackages.form.save") : t("admin.benefitPackages.form.create")}
          </button>
        </div>
      </form>
    </Modal>
  );
};

export default BenefitPackageFormModal;