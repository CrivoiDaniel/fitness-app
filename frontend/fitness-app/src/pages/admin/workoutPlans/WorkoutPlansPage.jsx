import React, { useEffect, useMemo, useState } from "react";
import { useTranslation } from "react-i18next";

import { useAuth } from "../../../context/AuthContext";
import Pagination from "../../../components/common/Pagination";

import { getAllClients } from "../../../api/admin/clients";
import { getAllTrainers } from "../../../api/admin/trainers";

import {
  getAllWorkoutPlans,
  createWorkoutPlan,
  cloneWorkoutPlan,
} from "../../../api/admin/workoutPlans";

import WorkoutPlanFormModal from "./WorkoutPlanFormModal";
import CloneWorkoutPlanModal from "./CloneWorkoutPlanModal";

const PAGE_SIZE = 15;

function safeText(x) {
  if (x === null || x === undefined) return "-";
  const s = String(x).trim();
  return s ? s : "-";
}

const WorkoutPlansPage = () => {
  const { t } = useTranslation();
  const { token } = useAuth();

  const [plans, setPlans] = useState([]);
  const [clients, setClients] = useState([]);
  const [trainers, setTrainers] = useState([]);

  const [loading, setLoading] = useState(false);
  const [error, setError] = useState("");

  const [page, setPage] = useState(1);

  const [createOpen, setCreateOpen] = useState(false);

  const [editOpen, setEditOpen] = useState(false);
  const [editPlan, setEditPlan] = useState(null);

  const [cloneOpen, setCloneOpen] = useState(false);
  const [cloneSource, setCloneSource] = useState(null);

  const total = plans.length;

  const pageItems = useMemo(() => {
    const start = (page - 1) * PAGE_SIZE;
    return plans.slice(start, start + PAGE_SIZE);
  }, [plans, page]);

  const fetchAll = async () => {
    try {
      setLoading(true);
      setError("");

      const [p, c, tr] = await Promise.all([
        getAllWorkoutPlans(token),
        getAllClients(token),
        getAllTrainers(token),
      ]);

      setPlans(p || []);
      setClients(c || []);
      setTrainers(tr || []);
      setPage(1);
    } catch (e) {
      setError(e.message || t("admin.workoutPlans.errors.load"));
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchAll();
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, []);

  const onCreate = async (dto) => {
    try {
      setLoading(true);
      setError("");
      await createWorkoutPlan(token, dto);
      setCreateOpen(false);
      await fetchAll();
    } catch (e) {
      setError(e.message || t("admin.workoutPlans.errors.create"));
    } finally {
      setLoading(false);
    }
  };

  const onEditAsNew = async (dto) => {
    try {
      setLoading(true);
      setError("");
      await createWorkoutPlan(token, dto);
      setEditOpen(false);
      setEditPlan(null);
      await fetchAll();
    } catch (e) {
      setError(e.message || t("admin.workoutPlans.errors.edit"));
    } finally {
      setLoading(false);
    }
  };

  const onClone = async (dto) => {
    try {
      setLoading(true);
      setError("");
      await cloneWorkoutPlan(token, dto);
      setCloneOpen(false);
      setCloneSource(null);
      await fetchAll();
    } catch (e) {
      setError(e.message || t("admin.workoutPlans.errors.clone"));
    } finally {
      setLoading(false);
    }
  };

  return (
    // IMPORTANT: scroll pe pagină
    <div className="h-[calc(100vh-48px)] overflow-y-auto">
      {/* IMPORTANT: header sticky */}
      <div className="sticky top-0 z-20 bg-gray-100 pb-3">
        <div className="flex items-end justify-between pt-1">
          <div>
            <h1 className="text-3xl font-extrabold">{t("admin.workoutPlans.title")}</h1>
            <p className="text-gray-600 mt-1">
              {t("admin.workoutPlans.total")}: {total}
            </p>
          </div>

          <button
            onClick={() => setCreateOpen(true)}
            className="px-4 py-2 rounded bg-black text-white font-semibold hover:bg-gray-800"
          >
            {t("admin.workoutPlans.add")}
          </button>
        </div>

        {error && <div className="mt-3 p-3 rounded bg-red-50 text-red-700 border">{error}</div>}
      </div>

      {/* conținutul */}
      <div className="mt-2 bg-white border rounded-xl overflow-hidden">
        <div className="grid grid-cols-10 gap-2 px-4 py-3 border-b font-semibold text-sm text-gray-700">
          <div>{t("admin.workoutPlans.columns.id")}</div>
          <div className="col-span-2">{t("admin.workoutPlans.columns.name")}</div>
          <div>{t("admin.workoutPlans.columns.client")}</div>
          <div>{t("admin.workoutPlans.columns.trainer")}</div>
          <div>{t("admin.workoutPlans.columns.goal")}</div>
          <div>{t("admin.workoutPlans.columns.difficulty")}</div>
          <div>{t("admin.workoutPlans.columns.duration")}</div>
          <div>{t("admin.workoutPlans.columns.exercises")}</div>
          <div className="text-right">{t("admin.workoutPlans.columns.actions")}</div>
        </div>

        {loading ? (
          <div className="p-4 text-gray-600">{t("common.loading")}</div>
        ) : pageItems.length === 0 ? (
          <div className="p-4 text-gray-600">{t("admin.workoutPlans.empty")}</div>
        ) : (
          pageItems.map((p) => (
            <div key={p.id} className="grid grid-cols-10 gap-2 px-4 py-3 border-b text-sm items-center">
              <div>{p.id}</div>

              <div className="col-span-2">
                <div className="font-semibold truncate">{safeText(p.name)}</div>
                <div className="text-xs text-gray-500 truncate">{safeText(p.description)}</div>
              </div>

              <div className="truncate">{safeText(p.clientName)}</div>
              <div className="truncate">{safeText(p.trainerName)}</div>

              <div>{t(`workouts.goal.${p.goal}`, p.goal)}</div>
              <div>{t(`workouts.difficulty.${p.difficulty}`, p.difficulty)}</div>

              <div>{p.durationWeeks}w</div>
              <div>{p.exercises?.length ?? 0}</div>

              <div className="flex justify-end gap-2 flex-wrap">
                <button
                  className="px-3 py-1 rounded border"
                  onClick={() => {
                    setEditPlan(p);
                    setEditOpen(true);
                  }}
                >
                  {t("admin.workoutPlans.edit")}
                </button>

                <button
                  className="px-3 py-1 rounded border"
                  onClick={() => {
                    setCloneSource(p);
                    setCloneOpen(true);
                  }}
                >
                  {t("admin.workoutPlans.cloneBtn")}
                </button>
              </div>
            </div>
          ))
        )}
      </div>

      {/* pagination tot în scroll */}
      <div className="pb-6">
        <Pagination page={page} pageSize={PAGE_SIZE} total={total} onPageChange={setPage} />
      </div>

      <WorkoutPlanFormModal
        open={createOpen}
        mode="create"
        initial={null}
        clients={clients}
        trainers={trainers}
        onClose={() => setCreateOpen(false)}
        onSubmit={onCreate}
      />

      <WorkoutPlanFormModal
        open={editOpen}
        mode="edit"
        initial={editPlan}
        clients={clients}
        trainers={trainers}
        onClose={() => {
          setEditOpen(false);
          setEditPlan(null);
        }}
        onSubmit={onEditAsNew}
      />

      <CloneWorkoutPlanModal
        open={cloneOpen}
        sourcePlan={cloneSource}
        clients={clients}
        trainers={trainers}
        onClose={() => {
          setCloneOpen(false);
          setCloneSource(null);
        }}
        onSubmit={onClone}
      />
    </div>
  );
};

export default WorkoutPlansPage;