import React, { useEffect, useMemo, useState } from "react";
import Modal from "../../../components/common/Modal";
import { useTranslation } from "react-i18next";

const GOALS = [
  "WeightLoss",
  "MuscleGain",
  "Endurance",
  "Strength",
  "Flexibility",
  "GeneralFitness",
];

const DIFFICULTIES = ["Beginner", "Intermediate", "Advanced"];

const DAYS = [
  { key: "Monday", label: "common.days.monday" },
  { key: "Tuesday", label: "common.days.tuesday" },
  { key: "Wednesday", label: "common.days.wednesday" },
  { key: "Thursday", label: "common.days.thursday" },
  { key: "Friday", label: "common.days.friday" },
  { key: "Saturday", label: "common.days.saturday" },
  { key: "Sunday", label: "common.days.sunday" },
];

const emptyExercise = () => ({
  name: "",
  sets: 3,
  reps: 10,
  durationSeconds: "",
  notes: "",
});

function normalizeDaysFromResponse(arr) {
  // backend response: WorkoutDays: ["Monday", ...]
  const s = new Set((arr || []).map((x) => String(x)));
  return DAYS.filter((d) => s.has(d.key)).map((d) => d.key);
}

function mapResponseToCreateDto(plan) {
  // IMPORTANT: backend CreateWorkoutPlanRequest expects:
  // { name, clientId, goal, difficulty, durationWeeks, workoutDays: List<DayOfWeek>, sessionDurationMinutes, trainerId?, description?, restDays?, specialNotes?, exercises: [...] }
  // plan response has Goal/Difficulty as strings already.
  return {
    name: plan?.name ?? "",
    clientId: plan?.clientId ?? "",
    goal: plan?.goal ?? "GeneralFitness",
    difficulty: plan?.difficulty ?? "Beginner",
    durationWeeks: plan?.durationWeeks ?? 8,
    workoutDays: normalizeDaysFromResponse(plan?.workoutDays),
    sessionDurationMinutes: plan?.sessionDurationMinutes ?? 45,
    trainerId: plan?.trainerId ?? "",
    description: plan?.description ?? "",
    restDays: plan?.restDaysBetweenSessions ?? "",
    specialNotes: plan?.specialNotes ?? "",
    exercises:
      plan?.exercises?.length > 0
        ? plan.exercises.map((e) => ({
            name: e.name ?? "",
            sets: e.sets ?? 3,
            reps: e.reps ?? 10,
            durationSeconds: e.durationSeconds ?? "",
            notes: e.notes ?? "",
          }))
        : [emptyExercise()],
  };
}

const WorkoutPlanFormModal = ({ open, mode, initial, clients, trainers, onClose, onSubmit }) => {
  const { t } = useTranslation();
  const isEdit = mode === "edit";

  const [name, setName] = useState("");
  const [clientId, setClientId] = useState("");
  const [trainerId, setTrainerId] = useState("");

  const [goal, setGoal] = useState("GeneralFitness");
  const [difficulty, setDifficulty] = useState("Beginner");

  const [durationWeeks, setDurationWeeks] = useState(8);
  const [workoutDays, setWorkoutDays] = useState(["Monday", "Wednesday", "Friday"]);
  const [sessionDurationMinutes, setSessionDurationMinutes] = useState(45);

  const [description, setDescription] = useState("");
  const [restDays, setRestDays] = useState("");
  const [specialNotes, setSpecialNotes] = useState("");

  const [exercises, setExercises] = useState([emptyExercise()]);

  const [submitted, setSubmitted] = useState(false);

  useEffect(() => {
    if (!open) return;

    setSubmitted(false);

    if (!isEdit) {
      setName("");
      setClientId("");
      setTrainerId("");
      setGoal("GeneralFitness");
      setDifficulty("Beginner");
      setDurationWeeks(8);
      setWorkoutDays(["Monday", "Wednesday", "Friday"]);
      setSessionDurationMinutes(45);
      setDescription("");
      setRestDays("");
      setSpecialNotes("");
      setExercises([emptyExercise()]);
      return;
    }

    // edit mode: prefill from response (but remember: we don't have PUT; we will create a new plan)
    const dto = mapResponseToCreateDto(initial);
    setName(dto.name);
    setClientId(dto.clientId ? String(dto.clientId) : "");
    setTrainerId(dto.trainerId ? String(dto.trainerId) : "");
    setGoal(dto.goal);
    setDifficulty(dto.difficulty);
    setDurationWeeks(dto.durationWeeks);
    setWorkoutDays(dto.workoutDays);
    setSessionDurationMinutes(dto.sessionDurationMinutes);
    setDescription(dto.description ?? "");
    setRestDays(dto.restDays ?? "");
    setSpecialNotes(dto.specialNotes ?? "");
    setExercises(dto.exercises?.length ? dto.exercises : [emptyExercise()]);
  }, [open, isEdit, initial]);

  const nameError = useMemo(() => {
    if (!name || String(name).trim().length < 3) return t("admin.workoutPlans.form.nameRequired");
    return "";
  }, [name, t]);

  const clientError = useMemo(() => {
    if (!clientId) return t("admin.workoutPlans.form.clientRequired");
    return "";
  }, [clientId, t]);

  const durationError = useMemo(() => {
    const v = Number(durationWeeks);
    if (Number.isNaN(v) || v < 1 || v > 52) return t("admin.workoutPlans.form.durationInvalid");
    return "";
  }, [durationWeeks, t]);

  const daysError = useMemo(() => {
    if (!workoutDays || workoutDays.length < 1) return t("admin.workoutPlans.form.daysRequired");
    return "";
  }, [workoutDays, t]);

  const sessionError = useMemo(() => {
    const v = Number(sessionDurationMinutes);
    if (Number.isNaN(v) || v < 15 || v > 240) return t("admin.workoutPlans.form.sessionInvalid");
    return "";
  }, [sessionDurationMinutes, t]);

  const exercisesError = useMemo(() => {
    if (!exercises || exercises.length < 1) return t("admin.workoutPlans.form.exercisesMin1");

    for (const e of exercises) {
      if (!e.name || !String(e.name).trim()) return t("admin.workoutPlans.form.exerciseNameRequired");

      const sets = Number(e.sets);
      const reps = Number(e.reps);
      if (Number.isNaN(sets) || sets < 1 || sets > 100) return t("admin.workoutPlans.form.exerciseSetsInvalid");
      if (Number.isNaN(reps) || reps < 1 || reps > 1000) return t("admin.workoutPlans.form.exerciseRepsInvalid");

      if (e.durationSeconds !== "" && e.durationSeconds !== null && e.durationSeconds !== undefined) {
        const ds = Number(e.durationSeconds);
        if (Number.isNaN(ds) || ds < 1 || ds > 3600) return t("admin.workoutPlans.form.exerciseDurationInvalid");
      }
    }
    return "";
  }, [exercises, t]);

  const canSubmit = !nameError && !clientError && !durationError && !daysError && !sessionError && !exercisesError;

  const toggleDay = (key) => {
    setWorkoutDays((prev) => {
      const s = new Set(prev);
      if (s.has(key)) s.delete(key);
      else s.add(key);
      return Array.from(s);
    });
  };

  const addExercise = () => setExercises((prev) => [...prev, emptyExercise()]);
  const removeExercise = (idx) => setExercises((prev) => prev.filter((_, i) => i !== idx));

  const updateExercise = (idx, patch) => {
    setExercises((prev) => prev.map((e, i) => (i === idx ? { ...e, ...patch } : e)));
  };

  const handleSubmit = (e) => {
    e.preventDefault();
    setSubmitted(true);
    if (!canSubmit) return;

    onSubmit({
      name: String(name).trim(),
      clientId: Number(clientId),
      goal,
      difficulty,
      durationWeeks: Number(durationWeeks),
      workoutDays: workoutDays, // backend expects List<DayOfWeek> => strings are ok because of JsonStringEnumConverter
      sessionDurationMinutes: Number(sessionDurationMinutes),
      trainerId: trainerId ? Number(trainerId) : null,
      description: description ? String(description) : null,
      restDays: restDays === "" ? null : Number(restDays),
      specialNotes: specialNotes ? String(specialNotes) : null,
      exercises: exercises.map((ex) => ({
        name: String(ex.name).trim(),
        sets: Number(ex.sets),
        reps: Number(ex.reps),
        durationSeconds: ex.durationSeconds === "" ? null : Number(ex.durationSeconds),
        notes: ex.notes ? String(ex.notes) : null,
      })),
    });
  };

  return (
    <Modal
      open={open}
      title={isEdit ? t("admin.workoutPlans.form.editTitle") : t("admin.workoutPlans.form.createTitle")}
      onClose={onClose}
    >
      <form onSubmit={handleSubmit} className="space-y-3">
        {isEdit && (
          <div className="p-3 rounded border bg-yellow-50 text-yellow-900 text-sm">
            {t("admin.workoutPlans.form.editHint")}
          </div>
        )}

        <div>
          <label className="text-sm font-semibold">{t("admin.workoutPlans.form.name")}</label>
          <input className="w-full border rounded p-2" value={name} onChange={(e) => setName(e.target.value)} />
          {submitted && nameError && <p className="text-sm text-red-600 mt-1">{nameError}</p>}
        </div>

        <div className="grid grid-cols-2 gap-3">
          <div>
            <label className="text-sm font-semibold">{t("admin.workoutPlans.form.client")}</label>
            <select className="w-full border rounded p-2" value={clientId} onChange={(e) => setClientId(e.target.value)}>
              <option value="">{t("admin.workoutPlans.form.selectClient")}</option>
              {clients.map((c) => (
                <option key={c.id ?? c.clientId} value={c.id ?? c.clientId}>
                  {c.fullName || `${c.firstName ?? ""} ${c.lastName ?? ""}`.trim() || c.email || `#${c.id ?? c.clientId}`}
                </option>
              ))}
            </select>
            {submitted && clientError && <p className="text-sm text-red-600 mt-1">{clientError}</p>}
          </div>

          <div>
            <label className="text-sm font-semibold">{t("admin.workoutPlans.form.trainer")}</label>
            <select className="w-full border rounded p-2" value={trainerId} onChange={(e) => setTrainerId(e.target.value)}>
              <option value="">{t("admin.workoutPlans.form.noTrainer")}</option>
              {trainers.map((tr) => (
                <option key={tr.id ?? tr.trainerId} value={tr.id ?? tr.trainerId}>
                  {tr.fullName || `${tr.firstName ?? ""} ${tr.lastName ?? ""}`.trim() || tr.email || `#${tr.id ?? tr.trainerId}`}
                </option>
              ))}
            </select>
          </div>
        </div>

        <div className="grid grid-cols-2 gap-3">
          <div>
            <label className="text-sm font-semibold">{t("admin.workoutPlans.form.goal")}</label>
            <select className="w-full border rounded p-2" value={goal} onChange={(e) => setGoal(e.target.value)}>
              {GOALS.map((g) => (
                <option key={g} value={g}>
                  {t(`workouts.goal.${g}`, g)}
                </option>
              ))}
            </select>
          </div>

          <div>
            <label className="text-sm font-semibold">{t("admin.workoutPlans.form.difficulty")}</label>
            <select className="w-full border rounded p-2" value={difficulty} onChange={(e) => setDifficulty(e.target.value)}>
              {DIFFICULTIES.map((d) => (
                <option key={d} value={d}>
                  {t(`workouts.difficulty.${d}`, d)}
                </option>
              ))}
            </select>
          </div>
        </div>

        <div className="grid grid-cols-3 gap-3">
          <div>
            <label className="text-sm font-semibold">{t("admin.workoutPlans.form.durationWeeks")}</label>
            <input
              type="number"
              min={1}
              max={52}
              className="w-full border rounded p-2"
              value={durationWeeks}
              onChange={(e) => setDurationWeeks(e.target.value)}
            />
            {submitted && durationError && <p className="text-sm text-red-600 mt-1">{durationError}</p>}
          </div>

          <div>
            <label className="text-sm font-semibold">{t("admin.workoutPlans.form.sessionMinutes")}</label>
            <input
              type="number"
              min={15}
              max={240}
              className="w-full border rounded p-2"
              value={sessionDurationMinutes}
              onChange={(e) => setSessionDurationMinutes(e.target.value)}
            />
            {submitted && sessionError && <p className="text-sm text-red-600 mt-1">{sessionError}</p>}
          </div>

          <div>
            <label className="text-sm font-semibold">{t("admin.workoutPlans.form.restDays")}</label>
            <input
              type="number"
              min={0}
              max={7}
              className="w-full border rounded p-2"
              value={restDays}
              onChange={(e) => setRestDays(e.target.value)}
              placeholder="0-7"
            />
          </div>
        </div>

        <div>
          <label className="text-sm font-semibold">{t("admin.workoutPlans.form.days")}</label>
          <div className="mt-2 grid grid-cols-2 gap-2">
            {DAYS.map((d) => (
              <label key={d.key} className="flex items-center gap-2">
                <input type="checkbox" checked={workoutDays.includes(d.key)} onChange={() => toggleDay(d.key)} />
                <span className="text-sm">{t(d.label, d.key)}</span>
              </label>
            ))}
          </div>
          {submitted && daysError && <p className="text-sm text-red-600 mt-1">{daysError}</p>}
        </div>

        <div>
          <label className="text-sm font-semibold">{t("admin.workoutPlans.form.description")}</label>
          <textarea className="w-full border rounded p-2 min-h-[90px]" value={description} onChange={(e) => setDescription(e.target.value)} />
        </div>

        <div>
          <label className="text-sm font-semibold">{t("admin.workoutPlans.form.specialNotes")}</label>
          <textarea className="w-full border rounded p-2 min-h-[70px]" value={specialNotes} onChange={(e) => setSpecialNotes(e.target.value)} />
        </div>

        <div className="border rounded p-3">
          <div className="flex items-center justify-between">
            <div className="font-semibold">{t("admin.workoutPlans.form.exercises")}</div>
            <button type="button" className="px-3 py-1 rounded border hover:bg-gray-50" onClick={addExercise}>
              {t("admin.workoutPlans.form.addExercise")}
            </button>
          </div>

          {submitted && exercisesError && <p className="text-sm text-red-600 mt-2">{exercisesError}</p>}

          <div className="mt-3 space-y-3">
            {exercises.map((ex, idx) => (
              <div key={idx} className="rounded border p-3">
                <div className="flex justify-between items-center">
                  <div className="text-sm font-semibold">
                    {t("admin.workoutPlans.form.exercise")} #{idx + 1}
                  </div>
                  {exercises.length > 1 && (
                    <button
                      type="button"
                      className="px-3 py-1 rounded border border-red-300 text-red-700 hover:bg-red-50"
                      onClick={() => removeExercise(idx)}
                    >
                      {t("admin.workoutPlans.form.removeExercise")}
                    </button>
                  )}
                </div>

                <div className="mt-2 grid grid-cols-2 gap-3">
                  <div>
                    <label className="text-xs text-gray-600">{t("admin.workoutPlans.form.exerciseName")}</label>
                    <input
                      className="w-full border rounded p-2"
                      value={ex.name}
                      onChange={(e) => updateExercise(idx, { name: e.target.value })}
                    />
                  </div>

                  <div>
                    <label className="text-xs text-gray-600">{t("admin.workoutPlans.form.exerciseDurationSeconds")}</label>
                    <input
                      type="number"
                      min={1}
                      max={3600}
                      className="w-full border rounded p-2"
                      value={ex.durationSeconds}
                      onChange={(e) => updateExercise(idx, { durationSeconds: e.target.value })}
                      placeholder={t("admin.workoutPlans.form.optional")}
                    />
                  </div>
                </div>

                <div className="mt-2 grid grid-cols-2 gap-3">
                  <div>
                    <label className="text-xs text-gray-600">{t("admin.workoutPlans.form.exerciseSets")}</label>
                    <input
                      type="number"
                      min={1}
                      max={100}
                      className="w-full border rounded p-2"
                      value={ex.sets}
                      onChange={(e) => updateExercise(idx, { sets: e.target.value })}
                    />
                  </div>

                  <div>
                    <label className="text-xs text-gray-600">{t("admin.workoutPlans.form.exerciseReps")}</label>
                    <input
                      type="number"
                      min={1}
                      max={1000}
                      className="w-full border rounded p-2"
                      value={ex.reps}
                      onChange={(e) => updateExercise(idx, { reps: e.target.value })}
                    />
                  </div>
                </div>

                <div className="mt-2">
                  <label className="text-xs text-gray-600">{t("admin.workoutPlans.form.exerciseNotes")}</label>
                  <input
                    className="w-full border rounded p-2"
                    value={ex.notes}
                    onChange={(e) => updateExercise(idx, { notes: e.target.value })}
                    placeholder={t("admin.workoutPlans.form.optional")}
                  />
                </div>
              </div>
            ))}
          </div>
        </div>

        <div className="pt-2 flex justify-end gap-2">
          <button type="button" className="px-4 py-2 rounded border" onClick={onClose}>
            {t("common.cancel")}
          </button>
          <button type="submit" className="px-4 py-2 rounded bg-black text-white font-semibold hover:bg-gray-800" disabled={!canSubmit}>
            {isEdit ? t("admin.workoutPlans.form.save") : t("admin.workoutPlans.form.create")}
          </button>
        </div>
      </form>
    </Modal>
  );
};

export default WorkoutPlanFormModal;