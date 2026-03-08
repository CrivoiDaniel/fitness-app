import React, { useEffect, useMemo, useState } from "react";
import { useAuth } from "../../../context/AuthContext";
import Pagination from "../../../components/common/Pagination";
import ConfirmDialog from "../../../components/common/ConfirmDialog";
import BenefitFormModal from "./BenefitFormModal";
import { useTranslation } from "react-i18next";

import {
  getAllBenefits,
  createBenefit,
  updateBenefit,
  deleteBenefit,
} from "../../../api/admin/benefits";

const PAGE_SIZE = 15;

const BenefitsPage = () => {
  const { t } = useTranslation();
  const { token } = useAuth();

  const [benefits, setBenefits] = useState([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState("");

  const [page, setPage] = useState(1);

  const [createOpen, setCreateOpen] = useState(false);
  const [editOpen, setEditOpen] = useState(false);
  const [editBenefitObj, setEditBenefitObj] = useState(null);

  const [deleteOpen, setDeleteOpen] = useState(false);
  const [deleteBenefitObj, setDeleteBenefitObj] = useState(null);

  const total = benefits.length;

  const pageItems = useMemo(() => {
    const start = (page - 1) * PAGE_SIZE;
    return benefits.slice(start, start + PAGE_SIZE);
  }, [benefits, page]);

  const fetchBenefits = async () => {
    try {
      setLoading(true);
      setError("");
      const data = await getAllBenefits(token);

      // sort: active first, then name
      const sorted = [...data].sort((a, b) => {
        if (a.isActive !== b.isActive) return a.isActive ? -1 : 1;
        return String(a.name).localeCompare(String(b.name));
      });

      setBenefits(sorted);
      setPage(1);
    } catch (e) {
      setError(e.message || t("admin.benefits.errors.load"));
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchBenefits();
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, []);

  const onCreate = async (dto) => {
    try {
      setLoading(true);
      setError("");
      await createBenefit(token, dto);
      setCreateOpen(false);
      await fetchBenefits();
    } catch (e) {
      setError(e.message || t("admin.benefits.errors.create"));
    } finally {
      setLoading(false);
    }
  };

  const onEdit = (b) => {
    setEditBenefitObj(b);
    setEditOpen(true);
  };

  const onUpdate = async (dto) => {
    try {
      setLoading(true);
      setError("");
      await updateBenefit(token, editBenefitObj.id, dto);
      setEditOpen(false);
      setEditBenefitObj(null);
      await fetchBenefits();
    } catch (e) {
      setError(e.message || t("admin.benefits.errors.edit"));
    } finally {
      setLoading(false);
    }
  };

  const onAskDelete = (b) => {
    setDeleteBenefitObj(b);
    setDeleteOpen(true);
  };

  const onConfirmDelete = async () => {
    try {
      setLoading(true);
      setError("");
      await deleteBenefit(token, deleteBenefitObj.id);
      setDeleteOpen(false);
      setDeleteBenefitObj(null);
      await fetchBenefits();
    } catch (e) {
      setError(e.message || t("admin.benefits.errors.delete"));
    } finally {
      setLoading(false);
    }
  };

  return (
    <div>
      <div className="flex items-end justify-between">
        <div>
          <h1 className="text-3xl font-extrabold">{t("admin.benefits.title")}</h1>
          <p className="text-gray-600 mt-1">
            {t("admin.benefits.total")}: {total}
          </p>
        </div>

        <button
          onClick={() => setCreateOpen(true)}
          className="px-4 py-2 rounded bg-black text-white font-semibold hover:bg-gray-800"
        >
          {t("admin.benefits.add")}
        </button>
      </div>

      {error && (
        <div className="mt-4 p-3 rounded bg-red-50 text-red-700 border">
          {error}
        </div>
      )}

      <div className="mt-4 bg-white border rounded-xl overflow-hidden">
        <div className="grid grid-cols-6 gap-2 px-4 py-3 border-b font-semibold text-sm text-gray-700">
          <div>{t("admin.benefits.columns.id")}</div>
          <div>{t("admin.benefits.columns.key")}</div>
          <div>{t("admin.benefits.columns.displayName")}</div>
          <div>{t("admin.benefits.columns.active")}</div>
          <div className="col-span-2 text-right">{t("admin.benefits.columns.actions")}</div>
        </div>

        {loading ? (
          <div className="p-4 text-gray-600">{t("admin.benefits.loading")}</div>
        ) : pageItems.length === 0 ? (
          <div className="p-4 text-gray-600">{t("admin.benefits.empty")}</div>
        ) : (
          pageItems.map((b) => (
            <div key={b.id} className="grid grid-cols-6 gap-2 px-4 py-3 border-b text-sm items-center">
              <div>{b.id}</div>
              <div className="font-mono text-xs">{b.name}</div>
              <div className="truncate">{b.displayName}</div>
              <div>
                <span className={b.isActive ? "text-green-700" : "text-gray-500"}>
                  {b.isActive ? t("admin.benefits.statusActive") : t("admin.benefits.statusInactive")}
                </span>
              </div>

              <div className="col-span-2 flex justify-end gap-2">
                <button className="px-3 py-1 rounded border" onClick={() => onEdit(b)}>
                  {t("admin.benefits.edit")}
                </button>
                <button
                  className="px-3 py-1 rounded border border-red-300 text-red-700 hover:bg-red-50"
                  onClick={() => onAskDelete(b)}
                >
                  {t("admin.benefits.delete")}
                </button>
              </div>
            </div>
          ))
        )}
      </div>

      <Pagination page={page} pageSize={PAGE_SIZE} total={total} onPageChange={setPage} />

      <BenefitFormModal
        open={createOpen}
        mode="create"
        initial={null}
        onClose={() => setCreateOpen(false)}
        onSubmit={onCreate}
      />

      <BenefitFormModal
        open={editOpen}
        mode="edit"
        initial={editBenefitObj}
        onClose={() => {
          setEditOpen(false);
          setEditBenefitObj(null);
        }}
        onSubmit={onUpdate}
      />

      <ConfirmDialog
        open={deleteOpen}
        title={t("admin.benefits.confirmDelete.title")}
        message={t("admin.benefits.confirmDelete.message", { name: deleteBenefitObj?.displayName || deleteBenefitObj?.name })}
        confirmText={t("admin.benefits.confirmDelete.confirmText")}
        onConfirm={onConfirmDelete}
        onClose={() => {
          setDeleteOpen(false);
          setDeleteBenefitObj(null);
        }}
      />
    </div>
  );
};

export default BenefitsPage;