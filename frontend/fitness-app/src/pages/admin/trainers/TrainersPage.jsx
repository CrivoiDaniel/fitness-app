import React, { useEffect, useMemo, useState } from "react";
import { useAuth } from "../../../context/AuthContext";
import Pagination from "../../../components/common/Pagination";
import ConfirmDialog from "../../../components/common/ConfirmDialog";
import TrainerFormModal from "./TrainerFormModal";
import CreatedUserModal from "../../../pages/admin/CreatedUserModal";
import { useTranslation } from "react-i18next";

import {
  getAllTrainers,
  createTrainer,
  updateTrainer,
  deleteTrainer,
} from "../../../api/admin/trainers";

const PAGE_SIZE = 15;

const TrainersPage = () => {
  const { t } = useTranslation();
  const { token } = useAuth();

  const [trainers, setTrainers] = useState([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState("");

  const [page, setPage] = useState(1);
  const [createOpen, setCreateOpen] = useState(false);

  const [createdOpen, setCreatedOpen] = useState(false);
  const [createdUser, setCreatedUser] = useState(null);

  const [editOpen, setEditOpen] = useState(false);
  const [editTrainer, setEditTrainer] = useState(null);

  const [deleteOpen, setDeleteOpen] = useState(false);
  const [deleteTrainerObj, setDeleteTrainerObj] = useState(null);

  const total = trainers.length;

  const pageItems = useMemo(() => {
    const start = (page - 1) * PAGE_SIZE;
    return trainers.slice(start, start + PAGE_SIZE);
  }, [trainers, page]);

  const fetchTrainers = async () => {
    try {
      setLoading(true);
      setError("");
      const data = await getAllTrainers(token);
      setTrainers(data);
      setPage(1);
    } catch (e) {
      setError(e.message || t("admin.trainers.errors.load"));
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchTrainers();
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, []);

  const onCreate = async (dto) => {
    try {
      setLoading(true);
      setError("");

      const resp = await createTrainer(token, dto);

      setCreateOpen(false);
      setCreatedUser(resp);
      setCreatedOpen(true);

      await fetchTrainers();
    } catch (e) {
      setError(e.message || t("admin.trainers.errors.create"));
    } finally {
      setLoading(false);
    }
  };

  const onEdit = (tr) => {
    setEditTrainer(tr);
    setEditOpen(true);
  };

  const onUpdate = async (dto) => {
    try {
      setLoading(true);
      setError("");

      await updateTrainer(token, editTrainer.userId, dto);

      setEditOpen(false);
      setEditTrainer(null);
      await fetchTrainers();
    } catch (e) {
      setError(e.message || t("admin.trainers.errors.edit"));
    } finally {
      setLoading(false);
    }
  };

  const onAskDelete = (tr) => {
    setDeleteTrainerObj(tr);
    setDeleteOpen(true);
  };

  const onConfirmDelete = async () => {
    try {
      setLoading(true);
      setError("");
      await deleteTrainer(token, deleteTrainerObj.userId);
      setDeleteOpen(false);
      setDeleteTrainerObj(null);
      await fetchTrainers();
    } catch (e) {
      setError(e.message || t("admin.trainers.errors.delete"));
    } finally {
      setLoading(false);
    }
  };

  const deleteName = `${deleteTrainerObj?.firstName ?? ""} ${deleteTrainerObj?.lastName ?? ""}`.trim();

  return (
    <div>
      <div className="flex items-end justify-between">
        <div>
          <h1 className="text-3xl font-extrabold">{t("admin.trainers.title")}</h1>
          <p className="text-gray-600 mt-1">
            {t("admin.trainers.total")}: {total}
          </p>
        </div>

        <button
          onClick={() => setCreateOpen(true)}
          className="px-4 py-2 rounded bg-black text-white font-semibold hover:bg-gray-800"
        >
          {t("admin.trainers.add")}
        </button>
      </div>

      {error && (
        <div className="mt-4 p-3 rounded bg-red-50 text-red-700 border">
          {error}
        </div>
      )}

      <div className="mt-4 bg-white border rounded-xl overflow-hidden">
        <div className="grid grid-cols-7 gap-2 px-4 py-3 border-b font-semibold text-sm text-gray-700">
          <div>{t("admin.trainers.columns.id")}</div>
          <div>{t("admin.trainers.columns.name")}</div>
          <div>{t("admin.trainers.columns.email")}</div>
          <div>{t("admin.trainers.columns.phone")}</div>
          <div>{t("admin.trainers.columns.specialization")}</div>
          <div>{t("admin.trainers.columns.years")}</div>
          <div className="text-right">{t("admin.trainers.columns.actions")}</div>
        </div>

        {loading ? (
          <div className="p-4 text-gray-600">{t("admin.trainers.loading")}</div>
        ) : pageItems.length === 0 ? (
          <div className="p-4 text-gray-600">{t("admin.trainers.empty")}</div>
        ) : (
          pageItems.map((tr) => (
            <div
              key={tr.userId}
              className="grid grid-cols-7 gap-2 px-4 py-3 border-b text-sm items-center"
            >
              <div>{tr.userId}</div>
              <div>
                {tr.firstName} {tr.lastName}
              </div>
              <div className="truncate">{tr.email}</div>
              <div>{tr.phoneNumber ?? "-"}</div>
              <div className="truncate">{tr.specialization ?? "-"}</div>
              <div>{tr.yearsOfExperience ?? 0}</div>
              <div className="flex justify-end gap-2">
                <button className="px-3 py-1 rounded border" onClick={() => onEdit(tr)}>
                  {t("admin.trainers.edit")}
                </button>
                <button
                  className="px-3 py-1 rounded border border-red-300 text-red-700 hover:bg-red-50"
                  onClick={() => onAskDelete(tr)}
                >
                  {t("admin.trainers.delete")}
                </button>
              </div>
            </div>
          ))
        )}
      </div>

      <Pagination page={page} pageSize={PAGE_SIZE} total={total} onPageChange={setPage} />

      <TrainerFormModal
        open={createOpen}
        mode="create"
        initial={null}
        onClose={() => setCreateOpen(false)}
        onSubmit={onCreate}
      />

      <CreatedUserModal
        open={createdOpen}
        data={createdUser}
        onClose={() => {
          setCreatedOpen(false);
          setCreatedUser(null);
        }}
      />

      <TrainerFormModal
        open={editOpen}
        mode="edit"
        initial={editTrainer}
        onClose={() => {
          setEditOpen(false);
          setEditTrainer(null);
        }}
        onSubmit={onUpdate}
      />

      <ConfirmDialog
        open={deleteOpen}
        title={t("admin.trainers.confirmDelete.title")}
        message={t("admin.trainers.confirmDelete.message", { name: deleteName })}
        confirmText={t("admin.trainers.confirmDelete.confirmText")}
        onConfirm={onConfirmDelete}
        onClose={() => {
          setDeleteOpen(false);
          setDeleteTrainerObj(null);
        }}
      />
    </div>
  );
};

export default TrainersPage;