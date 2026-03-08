import React, { useEffect, useMemo, useState } from "react";
import { useTranslation } from "react-i18next";
import { useAuth } from "../../../context/AuthContext";
import Pagination from "../../../components/common/Pagination";
import ConfirmDialog from "../../../components/common/ConfirmDialog";
import Modal from "../../../components/common/Modal";

import { getAllSubscriptions } from "../../../api/admin/subscriptions";
import {
  getAllPayments,
  createPayment,
  updatePayment,
  markPaymentSuccess,
  markPaymentFailed,
  deletePayment,
} from "../../../api/admin/payments";

const PAGE_SIZE = 15;
const STATUS_OPTIONS = ["Pending", "Success", "Failed"];

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
  return dt.toISOString();
}

const PaymentsPage = () => {
  const { t } = useTranslation();
  const { token } = useAuth();

  const [payments, setPayments] = useState([]);
  const [subs, setSubs] = useState([]);

  const [loading, setLoading] = useState(false);
  const [error, setError] = useState("");

  const [page, setPage] = useState(1);

  const [createOpen, setCreateOpen] = useState(false);

  const [editOpen, setEditOpen] = useState(false);
  const [editPayment, setEditPayment] = useState(null);

  const [deleteOpen, setDeleteOpen] = useState(false);
  const [deletePaymentState, setDeletePaymentState] = useState(null);

  const total = payments.length;

  const pageItems = useMemo(() => {
    const start = (page - 1) * PAGE_SIZE;
    return payments.slice(start, start + PAGE_SIZE);
  }, [payments, page]);

  const fetchAll = async () => {
    try {
      setLoading(true);
      setError("");

      const [pData, sData] = await Promise.all([
        getAllPayments(token),
        getAllSubscriptions(token),
      ]);

      setPayments(pData || []);
      setSubs(sData || []);
      setPage(1);
    } catch (e) {
      setError(e.message || t("admin.payments.errors.load"));
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
      await createPayment(token, dto);
      setCreateOpen(false);
      await fetchAll();
    } catch (e) {
      setError(e.message || t("admin.payments.errors.create"));
    } finally {
      setLoading(false);
    }
  };

  const onUpdate = async (dto) => {
    try {
      setLoading(true);
      setError("");
      await updatePayment(token, editPayment.id, dto);
      setEditOpen(false);
      setEditPayment(null);
      await fetchAll();
    } catch (e) {
      setError(e.message || t("admin.payments.errors.edit"));
    } finally {
      setLoading(false);
    }
  };

  const onMarkSuccess = async (p) => {
    try {
      setLoading(true);
      setError("");
      await markPaymentSuccess(token, p.id, p.transactionId);
      await fetchAll();
    } catch (e) {
      setError(e.message || t("admin.payments.errors.markSuccess"));
    } finally {
      setLoading(false);
    }
  };

  const onMarkFailed = async (p) => {
    try {
      setLoading(true);
      setError("");
      await markPaymentFailed(token, p.id);
      await fetchAll();
    } catch (e) {
      setError(e.message || t("admin.payments.errors.markFailed"));
    } finally {
      setLoading(false);
    }
  };

  const onConfirmDelete = async () => {
    try {
      setLoading(true);
      setError("");
      await deletePayment(token, deletePaymentState.id);
      setDeleteOpen(false);
      setDeletePaymentState(null);
      await fetchAll();
    } catch (e) {
      setError(e.message || t("admin.payments.errors.delete"));
    } finally {
      setLoading(false);
    }
  };

  const deleteLabel = deletePaymentState ? `#${deletePaymentState.id} (${deletePaymentState.amount} MDL)` : "-";

  const subById = useMemo(() => {
    const m = new Map();
    subs.forEach((s) => m.set(s.id, s));
    return m;
  }, [subs]);

  return (
    <div>
      <div className="flex items-end justify-between">
        <div>
          <h1 className="text-3xl font-extrabold">{t("admin.payments.title")}</h1>
          <p className="text-gray-600 mt-1">
            {t("admin.payments.total")}: {total}
          </p>
        </div>

        <button
          onClick={() => setCreateOpen(true)}
          className="px-4 py-2 rounded bg-black text-white font-semibold hover:bg-gray-800"
        >
          {t("admin.payments.add")}
        </button>
      </div>

      {error && <div className="mt-4 p-3 rounded bg-red-50 text-red-700 border">{error}</div>}

      <div className="mt-4 bg-white border rounded-xl overflow-hidden">
        <div className="grid grid-cols-9 gap-2 px-4 py-3 border-b font-semibold text-sm text-gray-700">
          <div>{t("admin.payments.columns.id")}</div>
          <div>{t("admin.payments.columns.subscriptionId")}</div>
          <div>{t("admin.payments.columns.client")}</div>
          <div>{t("admin.payments.columns.amount")}</div>
          <div>{t("admin.payments.columns.date")}</div>
          <div>{t("admin.payments.columns.status")}</div>
          <div>{t("admin.payments.columns.installment")}</div>
          <div>{t("admin.payments.columns.transactionId")}</div>
          <div className="text-right">{t("admin.payments.columns.actions")}</div>
        </div>

        {loading ? (
          <div className="p-4 text-gray-600">{t("common.loading")}</div>
        ) : pageItems.length === 0 ? (
          <div className="p-4 text-gray-600">{t("admin.payments.empty")}</div>
        ) : (
          pageItems.map((p) => {
            const sub = subById.get(p.subscriptionId);
            const clientName = sub?.clientName || "-";
            return (
              <div key={p.id} className="grid grid-cols-9 gap-2 px-4 py-3 border-b text-sm items-center">
                <div>{p.id}</div>
                <div>#{p.subscriptionId}</div>
                <div className="truncate">{clientName}</div>
                <div>{p.amount} MDL</div>
                <div>{toDateInputValue(p.paymentDate) || "-"}</div>
                <div>{t(`payment.status.${p.status}`, p.status)}</div>
                <div>{p.installmentNumber}</div>
                <div className="truncate">{p.transactionId || "-"}</div>
                <div className="flex justify-end gap-2 flex-wrap">
                  <button
                    className="px-3 py-1 rounded border"
                    onClick={() => {
                      setEditPayment(p);
                      setEditOpen(true);
                    }}
                  >
                    {t("admin.payments.edit")}
                  </button>

                  <button className="px-3 py-1 rounded border" onClick={() => onMarkSuccess(p)}>
                    {t("admin.payments.markSuccess")}
                  </button>

                  <button className="px-3 py-1 rounded border" onClick={() => onMarkFailed(p)}>
                    {t("admin.payments.markFailed")}
                  </button>

                  <button
                    className="px-3 py-1 rounded border border-red-300 text-red-700 hover:bg-red-50"
                    onClick={() => {
                      setDeletePaymentState(p);
                      setDeleteOpen(true);
                    }}
                  >
                    {t("admin.payments.delete")}
                  </button>
                </div>
              </div>
            );
          })
        )}
      </div>

      <Pagination page={page} pageSize={PAGE_SIZE} total={total} onPageChange={setPage} />

      <Modal open={createOpen} title={t("admin.payments.form.createTitle")} onClose={() => setCreateOpen(false)}>
        <CreatePaymentForm
          t={t}
          subs={subs}
          loading={loading}
          onClose={() => setCreateOpen(false)}
          onSubmit={onCreate}
        />
      </Modal>

      <Modal
        open={editOpen}
        title={t("admin.payments.form.editTitle")}
        onClose={() => {
          setEditOpen(false);
          setEditPayment(null);
        }}
      >
        <EditPaymentForm
          t={t}
          payment={editPayment}
          loading={loading}
          onClose={() => {
            setEditOpen(false);
            setEditPayment(null);
          }}
          onSubmit={onUpdate}
        />
      </Modal>

      <ConfirmDialog
        open={deleteOpen}
        title={t("admin.payments.confirmDelete.title")}
        message={t("admin.payments.confirmDelete.message", { name: deleteLabel })}
        confirmText={t("admin.payments.confirmDelete.confirmText")}
        onConfirm={onConfirmDelete}
        onClose={() => {
          setDeleteOpen(false);
          setDeletePaymentState(null);
        }}
      />
    </div>
  );
};

const CreatePaymentForm = ({ t, subs, onSubmit, onClose, loading }) => {
  const [subscriptionId, setSubscriptionId] = useState("");
  const [amount, setAmount] = useState("");
  const [paymentDate, setPaymentDate] = useState(() => new Date().toISOString().slice(0, 10));
  const [installmentNumber, setInstallmentNumber] = useState(1);
  const [transactionId, setTransactionId] = useState("");

  const canSubmit = subscriptionId && amount && paymentDate && !loading;

  return (
    <form
      className="space-y-3"
      onSubmit={(e) => {
        e.preventDefault();
        if (!canSubmit) return;
        onSubmit({
          subscriptionId: Number(subscriptionId),
          amount: Number(amount),
          paymentDate: toIsoFromDateInput(paymentDate),
          installmentNumber: Number(installmentNumber) || 1,
          transactionId: transactionId ? transactionId : null,
        });
      }}
    >
      <div>
        <label className="text-sm font-semibold">{t("admin.payments.form.subscription")}</label>
        <select
          className="w-full border rounded p-2"
          value={subscriptionId}
          onChange={(e) => setSubscriptionId(e.target.value)}
        >
          <option value="">{t("admin.payments.form.selectSubscription")}</option>
          {subs.map((s) => (
            <option key={s.id} value={s.id}>
              #{s.id} - {s.clientName} - {s.planType}
            </option>
          ))}
        </select>
      </div>

      <div>
        <label className="text-sm font-semibold">{t("admin.payments.form.amount")}</label>
        <input className="w-full border rounded p-2" type="number" step="0.01" value={amount} onChange={(e) => setAmount(e.target.value)} />
      </div>

      <div>
        <label className="text-sm font-semibold">{t("admin.payments.form.paymentDate")}</label>
        <input className="w-full border rounded p-2" type="date" value={paymentDate} onChange={(e) => setPaymentDate(e.target.value)} />
      </div>

      <div>
        <label className="text-sm font-semibold">{t("admin.payments.form.installmentNumber")}</label>
        <input className="w-full border rounded p-2" type="number" min={1} max={100} value={installmentNumber} onChange={(e) => setInstallmentNumber(e.target.value)} />
      </div>

      <div>
        <label className="text-sm font-semibold">{t("admin.payments.form.transactionId")}</label>
        <input className="w-full border rounded p-2" value={transactionId} onChange={(e) => setTransactionId(e.target.value)} />
      </div>

      <div className="pt-2 flex justify-end gap-2">
        <button type="button" className="px-4 py-2 rounded border" onClick={onClose}>
          {t("common.cancel")}
        </button>
        <button type="submit" className="px-4 py-2 rounded bg-black text-white font-semibold hover:bg-gray-800" disabled={!canSubmit}>
          {t("admin.payments.form.create")}
        </button>
      </div>
    </form>
  );
};

const EditPaymentForm = ({ t, payment, onSubmit, onClose, loading }) => {
  const [status, setStatus] = useState(payment?.status || "Pending");
  const [transactionId, setTransactionId] = useState(payment?.transactionId || "");

  if (!payment) return null;

  const canSubmit = status && !loading;

  return (
    <form
      className="space-y-3"
      onSubmit={(e) => {
        e.preventDefault();
        if (!canSubmit) return;
        onSubmit({
          status,
          transactionId: transactionId ? transactionId : null,
        });
      }}
    >
      <div>
        <label className="text-sm font-semibold">{t("admin.payments.form.status")}</label>
        <select className="w-full border rounded p-2" value={status} onChange={(e) => setStatus(e.target.value)}>
          {STATUS_OPTIONS.map((x) => (
            <option key={x} value={x}>
              {t(`payment.status.${x}`, x)}
            </option>
          ))}
        </select>
      </div>

      <div>
        <label className="text-sm font-semibold">{t("admin.payments.form.transactionId")}</label>
        <input className="w-full border rounded p-2" value={transactionId} onChange={(e) => setTransactionId(e.target.value)} />
      </div>

      <div className="pt-2 flex justify-end gap-2">
        <button type="button" className="px-4 py-2 rounded border" onClick={onClose}>
          {t("common.cancel")}
        </button>
        <button type="submit" className="px-4 py-2 rounded bg-black text-white font-semibold hover:bg-gray-800" disabled={!canSubmit}>
          {t("admin.payments.form.save")}
        </button>
      </div>
    </form>
  );
};

export default PaymentsPage;