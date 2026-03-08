import React, { useEffect, useMemo, useState } from "react";
import { useTranslation } from "react-i18next";
import { useAuth } from "../../../context/AuthContext";
import Pagination from "../../../components/common/Pagination";
import ConfirmDialog from "../../../components/common/ConfirmDialog";
import Modal from "../../../components/common/Modal";

import { getAllClients } from "../../../api/admin/clients";
import { getAllSubscriptionPlans } from "../../../api/admin/subscriptionPlans";

import {
  getAllSubscriptions,
  createSubscription,
  updateSubscription,
  cancelSubscription,
  renewSubscription,
  deleteSubscription,
} from "../../../api/admin/subscriptions";

const PAGE_SIZE = 15;
const STATUS_OPTIONS = ["Pending", "Active", "Expired", "Cancelled"];

function toDateInputValue(d) {
  if (!d) return "";
  const dt = typeof d === "string" ? new Date(d) : d;
  if (Number.isNaN(dt.getTime())) return "";
  return dt.toISOString().slice(0, 10);
}

function toIsoFromDateInput(v) {
  if (!v) return null;
  const dt = new Date(v);
  if (Number.isNaN(dt.getTime())) return null;
  // backend primește DateTime; ISO e ok
  return dt.toISOString();
}

const SubscriptionsPage = () => {
  const { t } = useTranslation();
  const { token } = useAuth();

  const [subs, setSubs] = useState([]);
  const [clients, setClients] = useState([]);
  const [plans, setPlans] = useState([]);

  const [loading, setLoading] = useState(false);
  const [error, setError] = useState("");

  const [page, setPage] = useState(1);

  const [createOpen, setCreateOpen] = useState(false);

  const [editOpen, setEditOpen] = useState(false);
  const [editSub, setEditSub] = useState(null);

  const [deleteOpen, setDeleteOpen] = useState(false);
  const [deleteSub, setDeleteSub] = useState(null);

  const total = subs.length;

  const pageItems = useMemo(() => {
    const start = (page - 1) * PAGE_SIZE;
    return subs.slice(start, start + PAGE_SIZE);
  }, [subs, page]);

  const fetchAll = async () => {
    try {
      setLoading(true);
      setError("");

      const [subsData, clientsData, plansData] = await Promise.all([
        getAllSubscriptions(token),
        getAllClients(token),
        getAllSubscriptionPlans(token),
      ]);

      setSubs(subsData || []);
      setClients(clientsData || []);
      setPlans(plansData || []);
      setPage(1);
    } catch (e) {
      setError(e.message || t("admin.subscriptions.errors.load"));
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
      await createSubscription(token, dto);
      setCreateOpen(false);
      await fetchAll();
    } catch (e) {
      setError(e.message || t("admin.subscriptions.errors.create"));
    } finally {
      setLoading(false);
    }
  };

  const onUpdate = async (dto) => {
    try {
      setLoading(true);
      setError("");
      await updateSubscription(token, editSub.id, dto);
      setEditOpen(false);
      setEditSub(null);
      await fetchAll();
    } catch (e) {
      setError(e.message || t("admin.subscriptions.errors.edit"));
    } finally {
      setLoading(false);
    }
  };

  const onCancel = async (s) => {
    try {
      setLoading(true);
      setError("");
      await cancelSubscription(token, s.id);
      await fetchAll();
    } catch (e) {
      setError(e.message || t("admin.subscriptions.errors.cancel"));
    } finally {
      setLoading(false);
    }
  };

  const onRenew = async (s) => {
    try {
      setLoading(true);
      setError("");
      await renewSubscription(token, s.id);
      await fetchAll();
    } catch (e) {
      setError(e.message || t("admin.subscriptions.errors.renew"));
    } finally {
      setLoading(false);
    }
  };

  const onConfirmDelete = async () => {
    try {
      setLoading(true);
      setError("");
      await deleteSubscription(token, deleteSub.id);
      setDeleteOpen(false);
      setDeleteSub(null);
      await fetchAll();
    } catch (e) {
      setError(e.message || t("admin.subscriptions.errors.delete"));
    } finally {
      setLoading(false);
    }
  };

  const deleteLabel = deleteSub
    ? `${deleteSub.clientName || `#${deleteSub.clientId}`} - ${t(`subscription.type.${deleteSub.planType}`, deleteSub.planType)}`
    : "-";

  return (
    <div>
      <div className="flex items-end justify-between">
        <div>
          <h1 className="text-3xl font-extrabold">{t("admin.subscriptions.title")}</h1>
          <p className="text-gray-600 mt-1">
            {t("admin.subscriptions.total")}: {total}
          </p>
        </div>

        <button
          onClick={() => setCreateOpen(true)}
          className="px-4 py-2 rounded bg-black text-white font-semibold hover:bg-gray-800"
        >
          {t("admin.subscriptions.add")}
        </button>
      </div>

      {error && <div className="mt-4 p-3 rounded bg-red-50 text-red-700 border">{error}</div>}

      <div className="mt-4 bg-white border rounded-xl overflow-hidden">
        <div className="grid grid-cols-9 gap-2 px-4 py-3 border-b font-semibold text-sm text-gray-700">
          <div>{t("admin.subscriptions.columns.id")}</div>
          <div>{t("admin.subscriptions.columns.client")}</div>
          <div>{t("admin.subscriptions.columns.plan")}</div>
          <div>{t("admin.subscriptions.columns.status")}</div>
          <div>{t("admin.subscriptions.columns.start")}</div>
          <div>{t("admin.subscriptions.columns.end")}</div>
          <div>{t("admin.subscriptions.columns.autoRenew")}</div>
          <div>{t("admin.subscriptions.columns.payments")}</div>
          <div className="text-right">{t("admin.subscriptions.columns.actions")}</div>
        </div>

        {loading ? (
          <div className="p-4 text-gray-600">{t("common.loading")}</div>
        ) : pageItems.length === 0 ? (
          <div className="p-4 text-gray-600">{t("admin.subscriptions.empty")}</div>
        ) : (
          pageItems.map((s) => (
            <div key={s.id} className="grid grid-cols-9 gap-2 px-4 py-3 border-b text-sm items-center">
              <div>{s.id}</div>
              <div className="truncate">{s.clientName || `#${s.clientId}`}</div>
              <div className="font-semibold">
                {t(`subscription.type.${s.planType}`, s.planType)} #{s.subscriptionPlanId}
              </div>
              <div>{t(`subscription.status.${s.status}`, s.status)}</div>
              <div>{toDateInputValue(s.startDate) || "-"}</div>
              <div>{toDateInputValue(s.endDate) || "-"}</div>
              <div>{s.autoRenew ? t("common.ok") : "-"}</div>
              <div>{s.payments?.length ?? 0}</div>
              <div className="flex justify-end gap-2 flex-wrap">
                <button
                  className="px-3 py-1 rounded border"
                  onClick={() => {
                    setEditSub(s);
                    setEditOpen(true);
                  }}
                >
                  {t("admin.subscriptions.edit")}
                </button>

                <button className="px-3 py-1 rounded border" onClick={() => onCancel(s)}>
                  {t("admin.subscriptions.cancel")}
                </button>

                <button className="px-3 py-1 rounded border" onClick={() => onRenew(s)}>
                  {t("admin.subscriptions.renew")}
                </button>

                <button
                  className="px-3 py-1 rounded border border-red-300 text-red-700 hover:bg-red-50"
                  onClick={() => {
                    setDeleteSub(s);
                    setDeleteOpen(true);
                  }}
                >
                  {t("admin.subscriptions.delete")}
                </button>
              </div>
            </div>
          ))
        )}
      </div>

      <Pagination page={page} pageSize={PAGE_SIZE} total={total} onPageChange={setPage} />

      <Modal open={createOpen} title={t("admin.subscriptions.form.createTitle")} onClose={() => setCreateOpen(false)}>
        <CreateSubscriptionForm
          t={t}
          clients={clients}
          plans={plans}
          loading={loading}
          onClose={() => setCreateOpen(false)}
          onSubmit={onCreate}
        />
      </Modal>

      <Modal
        open={editOpen}
        title={t("admin.subscriptions.form.editTitle")}
        onClose={() => {
          setEditOpen(false);
          setEditSub(null);
        }}
      >
        <EditSubscriptionForm
          t={t}
          sub={editSub}
          loading={loading}
          onClose={() => {
            setEditOpen(false);
            setEditSub(null);
          }}
          onSubmit={onUpdate}
        />
      </Modal>

      <ConfirmDialog
        open={deleteOpen}
        title={t("admin.subscriptions.confirmDelete.title")}
        message={t("admin.subscriptions.confirmDelete.message", { name: deleteLabel })}
        confirmText={t("admin.subscriptions.confirmDelete.confirmText")}
        onConfirm={onConfirmDelete}
        onClose={() => {
          setDeleteOpen(false);
          setDeleteSub(null);
        }}
      />
    </div>
  );
};

const CreateSubscriptionForm = ({ t, clients, plans, onSubmit, onClose, loading }) => {
  const [clientId, setClientId] = useState("");
  const [subscriptionPlanId, setSubscriptionPlanId] = useState("");
  const [startDate, setStartDate] = useState(() => new Date().toISOString().slice(0, 10));
  const [autoRenew, setAutoRenew] = useState(true);

  const canSubmit = clientId && subscriptionPlanId && startDate && !loading;

  return (
    <form
      className="space-y-3"
      onSubmit={(e) => {
        e.preventDefault();
        if (!canSubmit) return;
        onSubmit({
          clientId: Number(clientId),
          subscriptionPlanId: Number(subscriptionPlanId),
          startDate: toIsoFromDateInput(startDate),
          autoRenew: !!autoRenew,
        });
      }}
    >
      <div>
        <label className="text-sm font-semibold">{t("admin.subscriptions.form.client")}</label>
        <select className="w-full border rounded p-2" value={clientId} onChange={(e) => setClientId(e.target.value)}>
          <option value="">{t("admin.subscriptions.form.selectClient")}</option>
          {clients.map((c) => (
            <option key={c.id ?? c.clientId} value={c.id ?? c.clientId}>
              {c.fullName || `${c.firstName ?? ""} ${c.lastName ?? ""}`.trim() || c.email || `#${c.id ?? c.clientId}`}
            </option>
          ))}
        </select>
      </div>

      <div>
        <label className="text-sm font-semibold">{t("admin.subscriptions.form.plan")}</label>
        <select
          className="w-full border rounded p-2"
          value={subscriptionPlanId}
          onChange={(e) => setSubscriptionPlanId(e.target.value)}
        >
          <option value="">{t("admin.subscriptions.form.selectPlan")}</option>
          {plans.map((p) => (
            <option key={p.id} value={p.id}>
              #{p.id} {t(`subscription.type.${p.type}`, p.type)} ({p.price} MDL)
            </option>
          ))}
        </select>
      </div>

      <div>
        <label className="text-sm font-semibold">{t("admin.subscriptions.form.startDate")}</label>
        <input className="w-full border rounded p-2" type="date" value={startDate} onChange={(e) => setStartDate(e.target.value)} />
      </div>

      <label className="flex items-center gap-2">
        <input type="checkbox" checked={autoRenew} onChange={(e) => setAutoRenew(e.target.checked)} />
        <span className="text-sm">{t("admin.subscriptions.form.autoRenew")}</span>
      </label>

      <div className="pt-2 flex justify-end gap-2">
        <button type="button" className="px-4 py-2 rounded border" onClick={onClose}>
          {t("common.cancel")}
        </button>
        <button type="submit" className="px-4 py-2 rounded bg-black text-white font-semibold hover:bg-gray-800" disabled={!canSubmit}>
          {t("admin.subscriptions.form.create")}
        </button>
      </div>
    </form>
  );
};

const EditSubscriptionForm = ({ t, sub, onSubmit, onClose, loading }) => {
  const [status, setStatus] = useState(sub?.status || "Active");
  const [endDate, setEndDate] = useState(toDateInputValue(sub?.endDate));
  const [autoRenew, setAutoRenew] = useState(!!sub?.autoRenew);

  if (!sub) return null;

  const canSubmit = status && !loading;

  return (
    <form
      className="space-y-3"
      onSubmit={(e) => {
        e.preventDefault();
        if (!canSubmit) return;
        onSubmit({
          status,
          endDate: endDate ? toIsoFromDateInput(endDate) : null,
          autoRenew: !!autoRenew,
        });
      }}
    >
      <div>
        <label className="text-sm font-semibold">{t("admin.subscriptions.form.status")}</label>
        <select className="w-full border rounded p-2" value={status} onChange={(e) => setStatus(e.target.value)}>
          {STATUS_OPTIONS.map((x) => (
            <option key={x} value={x}>
              {t(`subscription.status.${x}`, x)}
            </option>
          ))}
        </select>
      </div>

      <div>
        <label className="text-sm font-semibold">{t("admin.subscriptions.form.endDate")}</label>
        <input className="w-full border rounded p-2" type="date" value={endDate} onChange={(e) => setEndDate(e.target.value)} />
      </div>

      <label className="flex items-center gap-2">
        <input type="checkbox" checked={autoRenew} onChange={(e) => setAutoRenew(e.target.checked)} />
        <span className="text-sm">{t("admin.subscriptions.form.autoRenew")}</span>
      </label>

      <div className="pt-2 flex justify-end gap-2">
        <button type="button" className="px-4 py-2 rounded border" onClick={onClose}>
          {t("common.cancel")}
        </button>
        <button type="submit" className="px-4 py-2 rounded bg-black text-white font-semibold hover:bg-gray-800" disabled={!canSubmit}>
          {t("admin.subscriptions.form.save")}
        </button>
      </div>
    </form>
  );
};

export default SubscriptionsPage;