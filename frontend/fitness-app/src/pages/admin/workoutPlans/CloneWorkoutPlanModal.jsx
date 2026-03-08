import React, { useEffect, useMemo, useState } from "react";
import Modal from "../../../components/common/Modal";
import { useTranslation } from "react-i18next";

const CloneWorkoutPlanModal = ({ open, sourcePlan, clients, trainers, onClose, onSubmit }) => {
  const { t } = useTranslation();

  const [targetClientId, setTargetClientId] = useState("");
  const [newName, setNewName] = useState("");
  const [newTrainerId, setNewTrainerId] = useState("");

  const [submitted, setSubmitted] = useState(false);

  useEffect(() => {
    if (!open) return;

    setSubmitted(false);
    setTargetClientId("");
    setNewTrainerId("");
    setNewName(sourcePlan?.name ? `${sourcePlan.name} (Copy)` : "");
  }, [open, sourcePlan]);

  const clientError = useMemo(() => {
    if (!targetClientId) return t("admin.workoutPlans.clone.clientRequired");
    return "";
  }, [targetClientId, t]);

  const nameError = useMemo(() => {
    if (newName && String(newName).length > 200) return t("admin.workoutPlans.clone.nameTooLong");
    return "";
  }, [newName, t]);

  const canSubmit = !clientError && !nameError;

  const handleSubmit = (e) => {
    e.preventDefault();
    setSubmitted(true);
    if (!canSubmit) return;

    onSubmit({
      sourceWorkoutPlanId: Number(sourcePlan.id),
      targetClientId: Number(targetClientId),
      newName: newName ? String(newName) : null,
      newTrainerId: newTrainerId ? Number(newTrainerId) : null,
    });
  };

  return (
    <Modal open={open} title={t("admin.workoutPlans.clone.title")} onClose={onClose}>
      {!sourcePlan ? (
        <div className="text-gray-600">—</div>
      ) : (
        <form onSubmit={handleSubmit} className="space-y-3">
          <div className="text-sm text-gray-600">
            {t("admin.workoutPlans.clone.source")}: <span className="font-semibold">#{sourcePlan.id}</span> —{" "}
            <span className="font-semibold">{sourcePlan.name}</span>
          </div>

          <div>
            <label className="text-sm font-semibold">{t("admin.workoutPlans.clone.targetClient")}</label>
            <select
              className="w-full border rounded p-2"
              value={targetClientId}
              onChange={(e) => setTargetClientId(e.target.value)}
            >
              <option value="">{t("admin.workoutPlans.clone.selectClient")}</option>
              {clients.map((c) => (
                <option key={c.id ?? c.clientId} value={c.id ?? c.clientId}>
                  {c.fullName || `${c.firstName ?? ""} ${c.lastName ?? ""}`.trim() || c.email || `#${c.id ?? c.clientId}`}
                </option>
              ))}
            </select>
            {submitted && clientError && <p className="text-sm text-red-600 mt-1">{clientError}</p>}
          </div>

          <div>
            <label className="text-sm font-semibold">{t("admin.workoutPlans.clone.newName")}</label>
            <input className="w-full border rounded p-2" value={newName} onChange={(e) => setNewName(e.target.value)} />
            {submitted && nameError && <p className="text-sm text-red-600 mt-1">{nameError}</p>}
          </div>

          <div>
            <label className="text-sm font-semibold">{t("admin.workoutPlans.clone.newTrainer")}</label>
            <select
              className="w-full border rounded p-2"
              value={newTrainerId}
              onChange={(e) => setNewTrainerId(e.target.value)}
            >
              <option value="">{t("admin.workoutPlans.clone.keepTrainer")}</option>
              {trainers.map((tr) => (
                <option key={tr.id ?? tr.trainerId} value={tr.id ?? tr.trainerId}>
                  {tr.fullName || `${tr.firstName ?? ""} ${tr.lastName ?? ""}`.trim() || tr.email || `#${tr.id ?? tr.trainerId}`}
                </option>
              ))}
            </select>
          </div>

          <div className="pt-2 flex justify-end gap-2">
            <button type="button" className="px-4 py-2 rounded border" onClick={onClose}>
              {t("common.cancel")}
            </button>
            <button type="submit" className="px-4 py-2 rounded bg-black text-white font-semibold hover:bg-gray-800" disabled={!canSubmit}>
              {t("admin.workoutPlans.clone.clone")}
            </button>
          </div>
        </form>
      )}
    </Modal>
  );
};

export default CloneWorkoutPlanModal;