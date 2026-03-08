import React, { useEffect, useMemo, useState } from "react";
import { useAuth } from "../../../context/AuthContext";
import Pagination from "../../../components/common/Pagination";
import ConfirmDialog from "../../../components/common/ConfirmDialog";
import SubscriptionPlanFormModal from "./SubscriptionPlanFormModal";
import { useTranslation } from "react-i18next";

import {
  getAllSubscriptionPlans,
  createSubscriptionPlan,
  updateSubscriptionPlan,
  deleteSubscriptionPlan,
} from "../../../api/admin/subscriptionPlans";

import { getAllBenefitPackages } from "../../../api/admin/benefitPackages";

const PAGE_SIZE = 15;

const SubscriptionPlansPage = () => {
  const { t } = useTranslation();
  const { token } = useAuth();

  const [plans, setPlans] = useState([]);
  const [packages, setPackages] = useState([]);

  const [loading, setLoading] = useState(false);
  const [loadingPackages, setLoadingPackages] = useState(false);
  const [error, setError] = useState("");

  const [page, setPage] = useState(1);

  const [createOpen, setCreateOpen] = useState(false);

  const [editOpen, setEditOpen] = useState(false);
  const [editPlan, setEditPlan] = useState(null);

  const [deleteOpen, setDeleteOpen] = useState(false);
  const [deletePlan, setDeletePlan] = useState(null);

  const total = plans.length;

  const pageItems = useMemo(() => {
    const start = (page - 1) * PAGE_SIZE;
    return plans.slice(start, start + PAGE_SIZE);
  }, [plans, page]);

  const fetchPackages = async () => {
    try {
      setLoadingPackages(true);
      const data = await getAllBenefitPackages(token);
      setPackages(data);
    } finally {
      setLoadingPackages(false);
    }
  };

  const fetchPlans = async () => {
    try {
      setLoading(true);
      setError("");
      const data = await getAllSubscriptionPlans(token);
      setPlans(data || []);
      setPage(1);
    } catch (e) {
      setError(e.message || t("admin.subscriptionPlans.errors.load"));
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchPackages();
    fetchPlans();
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, []);

  const onCreate = async (dto) => {
    try {
      setLoading(true);
      setError("");
      await createSubscriptionPlan(token, dto);
      setCreateOpen(false);
      await fetchPlans();
    } catch (e) {
      setError(e.message || t("admin.subscriptionPlans.errors.create"));
    } finally {
      setLoading(false);
    }
  };

  const onEdit = (p) => {
    setEditPlan(p);
    setEditOpen(true);
  };

  const onUpdate = async (dto) => {
    try {
      setLoading(true);
      setError("");
      await updateSubscriptionPlan(token, editPlan.id, dto);
      setEditOpen(false);
      setEditPlan(null);
      await fetchPlans();
    } catch (e) {
      setError(e.message || t("admin.subscriptionPlans.errors.edit"));
    } finally {
      setLoading(false);
    }
  };

  const onAskDelete = (p) => {
    setDeletePlan(p);
    setDeleteOpen(true);
  };

  const onConfirmDelete = async () => {
    try {
      setLoading(true);
      setError("");
      await deleteSubscriptionPlan(token, deletePlan.id);
      setDeleteOpen(false);
      setDeletePlan(null);
      await fetchPlans();
    } catch (e) {
      setError(e.message || t("admin.subscriptionPlans.errors.delete"));
    } finally {
      setLoading(false);
    }
  };

  const deleteTypeLabel = t(`subscription.type.${deletePlan?.type}`, deletePlan?.type || "-");

  return (
    <div>
      <div className="flex items-end justify-between">
        <div>
          <h1 className="text-3xl font-extrabold">{t("admin.subscriptionPlans.title")}</h1>
          <p className="text-gray-600 mt-1">
            {t("admin.subscriptionPlans.total")}: {total}
          </p>
        </div>

        <button
          onClick={() => setCreateOpen(true)}
          className="px-4 py-2 rounded bg-black text-white font-semibold hover:bg-gray-800"
        >
          {t("admin.subscriptionPlans.add")}
        </button>
      </div>

      {error && <div className="mt-4 p-3 rounded bg-red-50 text-red-700 border">{error}</div>}

      <div className="mt-4 bg-white border rounded-xl overflow-hidden">
        <div className="grid grid-cols-8 gap-2 px-4 py-3 border-b font-semibold text-sm text-gray-700">
          <div>{t("admin.subscriptionPlans.columns.id")}</div>
          <div>{t("admin.subscriptionPlans.columns.type")}</div>
          <div>{t("admin.subscriptionPlans.columns.duration")}</div>
          <div>{t("admin.subscriptionPlans.columns.price")}</div>
          <div>{t("admin.subscriptionPlans.columns.package")}</div>
          <div>{t("admin.subscriptionPlans.columns.installments")}</div>
          <div>{t("admin.subscriptionPlans.columns.active")}</div>
          <div className="text-right">{t("admin.subscriptionPlans.columns.actions")}</div>
        </div>

        {loading ? (
          <div className="p-4 text-gray-600">{t("common.loading")}</div>
        ) : pageItems.length === 0 ? (
          <div className="p-4 text-gray-600">{t("admin.subscriptionPlans.empty")}</div>
        ) : (
          pageItems.map((p) => (
            <div key={p.id} className="grid grid-cols-8 gap-2 px-4 py-3 border-b text-sm items-center">
              <div>{p.id}</div>
              <div className="font-semibold">{t(`subscription.type.${p.type}`, p.type)}</div>
              <div>{p.durationInMonths}</div>
              <div>{p.price} MDL</div>
              <div className="truncate">{p.benefitPackageName || `#${p.benefitPackageId}`}</div>
              <div>{p.allowInstallments ? `${p.maxInstallments}` : "-"}</div>
              <div>
                <span className={p.isActive ? "text-green-700" : "text-gray-500"}>
                  {p.isActive ? t("admin.subscriptionPlans.statusActive") : t("admin.subscriptionPlans.statusInactive")}
                </span>
              </div>
              <div className="flex justify-end gap-2">
                <button className="px-3 py-1 rounded border" onClick={() => onEdit(p)}>
                  {t("admin.subscriptionPlans.edit")}
                </button>
                <button
                  className="px-3 py-1 rounded border border-red-300 text-red-700 hover:bg-red-50"
                  onClick={() => onAskDelete(p)}
                >
                  {t("admin.subscriptionPlans.delete")}
                </button>
              </div>
            </div>
          ))
        )}
      </div>

      <Pagination page={page} pageSize={PAGE_SIZE} total={total} onPageChange={setPage} />

      <SubscriptionPlanFormModal
        open={createOpen}
        mode="create"
        initial={null}
        benefitPackages={packages}
        loadingPackages={loadingPackages}
        onClose={() => setCreateOpen(false)}
        onSubmit={onCreate}
      />

      <SubscriptionPlanFormModal
        open={editOpen}
        mode="edit"
        initial={editPlan}
        benefitPackages={packages}
        loadingPackages={loadingPackages}
        onClose={() => {
          setEditOpen(false);
          setEditPlan(null);
        }}
        onSubmit={onUpdate}
      />

      <ConfirmDialog
        open={deleteOpen}
        title={t("admin.subscriptionPlans.confirmDelete.title")}
        message={t("admin.subscriptionPlans.confirmDelete.message", {
          name: `${deleteTypeLabel} (${deletePlan?.price ?? "-"} MDL)`,
        })}
        confirmText={t("admin.subscriptionPlans.confirmDelete.confirmText")}
        onConfirm={onConfirmDelete}
        onClose={() => {
          setDeleteOpen(false);
          setDeletePlan(null);
        }}
      />
    </div>
  );
};

export default SubscriptionPlansPage;