import React, { useEffect, useMemo, useState } from "react";
import { useAuth } from "../../../context/AuthContext";
import Pagination from "../../../components/common/Pagination";
import ConfirmDialog from "../../../components/common/ConfirmDialog";
import ClientFormModal from "../../../pages/admin/clients/ClientFormModal";
import CreatedUserModal from "../../../pages/admin/CreatedUserModal";
import { useTranslation } from "react-i18next";

import {
  getAllClients,
  createClient,
  updateClient,
  deleteClient,
} from "../../../api/admin/clients";

const PAGE_SIZE = 15;

const ClientsPage = () => {
  const { t } = useTranslation();
  const { token } = useAuth();

  const [clients, setClients] = useState([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState("");

  const [page, setPage] = useState(1);
  const [createOpen, setCreateOpen] = useState(false);

  const [createdOpen, setCreatedOpen] = useState(false);
  const [createdUser, setCreatedUser] = useState(null);

  const [editOpen, setEditOpen] = useState(false);
  const [editClient, setEditClient] = useState(null);

  const [deleteOpen, setDeleteOpen] = useState(false);
  const [deleteClientObj, setDeleteClientObj] = useState(null);

  const total = clients.length;

  const pageItems = useMemo(() => {
    const start = (page - 1) * PAGE_SIZE;
    return clients.slice(start, start + PAGE_SIZE);
  }, [clients, page]);

  const fetchClients = async () => {
    try {
      setLoading(true);
      setError("");
      const data = await getAllClients(token);
      setClients(data);
      setPage(1);
    } catch (e) {
      setError(e.message || t("admin.clients.errors.load"));
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchClients();
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, []);

  const onCreate = async (dto) => {
    try {
      setLoading(true);
      setError("");

      const resp = await createClient(token, dto);

      setCreateOpen(false);
      setCreatedUser(resp);
      setCreatedOpen(true);

      await fetchClients();
    } catch (e) {
      setError(e.message || t("admin.clients.errors.create"));
    } finally {
      setLoading(false);
    }
  };

  const onEdit = (c) => {
    setEditClient(c);
    setEditOpen(true);
  };

  const onUpdate = async (dto) => {
    try {
      setLoading(true);
      setError("");
      await updateClient(token, editClient.userId, dto);
      setEditOpen(false);
      setEditClient(null);
      await fetchClients();
    } catch (e) {
      setError(e.message || t("admin.clients.errors.edit"));
    } finally {
      setLoading(false);
    }
  };

  const onAskDelete = (c) => {
    setDeleteClientObj(c);
    setDeleteOpen(true);
  };

  const onConfirmDelete = async () => {
    try {
      setLoading(true);
      setError("");
      await deleteClient(token, deleteClientObj.userId);
      setDeleteOpen(false);
      setDeleteClientObj(null);
      await fetchClients();
    } catch (e) {
      setError(e.message || t("admin.clients.errors.delete"));
    } finally {
      setLoading(false);
    }
  };

  const deleteName = `${deleteClientObj?.firstName ?? ""} ${deleteClientObj?.lastName ?? ""}`.trim();

  return (
    <div>
      <div className="flex items-end justify-between">
        <div>
          <h1 className="text-3xl font-extrabold">{t("admin.clients.title")}</h1>
          <p className="text-gray-600 mt-1">
            {t("admin.clients.total")}: {total}
          </p>
        </div>

        <button
          onClick={() => setCreateOpen(true)}
          className="px-4 py-2 rounded bg-black text-white font-semibold hover:bg-gray-800"
        >
          {t("admin.clients.add")}
        </button>
      </div>

      {error && (
        <div className="mt-4 p-3 rounded bg-red-50 text-red-700 border">
          {error}
        </div>
      )}

      <div className="mt-4 bg-white border rounded-xl overflow-hidden">
        <div className="grid grid-cols-6 gap-2 px-4 py-3 border-b font-semibold text-sm text-gray-700">
          <div>{t("admin.clients.columns.id")}</div>
          <div>{t("admin.clients.columns.name")}</div>
          <div>{t("admin.clients.columns.email")}</div>
          <div>{t("admin.clients.columns.phone")}</div>
          <div>{t("admin.clients.columns.status")}</div>
          <div className="text-right">{t("admin.clients.columns.actions")}</div>
        </div>

        {loading ? (
          <div className="p-4 text-gray-600">{t("admin.clients.loading")}</div>
        ) : pageItems.length === 0 ? (
          <div className="p-4 text-gray-600">{t("admin.clients.empty")}</div>
        ) : (
          pageItems.map((c) => (
            <div
              key={c.userId}
              className="grid grid-cols-6 gap-2 px-4 py-3 border-b text-sm items-center"
            >
              <div>{c.userId}</div>
              <div>
                {c.firstName} {c.lastName}
              </div>
              <div className="truncate">{c.email}</div>
              <div>{c.phoneNumber ?? "-"}</div>
              <div>
                <span className={c.isActive ? "text-green-700" : "text-gray-500"}>
                  {c.isActive ? t("admin.clients.statusActive") : t("admin.clients.statusInactive")}
                </span>
              </div>
              <div className="flex justify-end gap-2">
                <button className="px-3 py-1 rounded border" onClick={() => onEdit(c)}>
                  {t("admin.clients.edit")}
                </button>
                <button
                  className="px-3 py-1 rounded border border-red-300 text-red-700 hover:bg-red-50"
                  onClick={() => onAskDelete(c)}
                >
                  {t("admin.clients.delete")}
                </button>
              </div>
            </div>
          ))
        )}
      </div>

      <Pagination page={page} pageSize={PAGE_SIZE} total={total} onPageChange={setPage} />

      <ClientFormModal
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

      <ClientFormModal
        open={editOpen}
        mode="edit"
        initial={editClient}
        onClose={() => {
          setEditOpen(false);
          setEditClient(null);
        }}
        onSubmit={onUpdate}
      />

      <ConfirmDialog
        open={deleteOpen}
        title={t("admin.clients.confirmDelete.title")}
        message={t("admin.clients.confirmDelete.message", { name: deleteName })}
        confirmText={t("admin.clients.confirmDelete.confirmText")}
        onConfirm={onConfirmDelete}
        onClose={() => {
          setDeleteOpen(false);
          setDeleteClientObj(null);
        }}
      />
    </div>
  );
};

export default ClientsPage;