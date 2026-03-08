import React, { useEffect, useMemo, useState } from "react";
import { useAuth } from "../../../context/AuthContext";
import Pagination from "../../../components/common/Pagination";
import ConfirmDialog from "../../../components/common/ConfirmDialog";
import BenefitPackageFormModal from "./BenefitPackageFormModal";
import { useTranslation } from "react-i18next";

import {
    getAllBenefitPackages,
    getBenefitPackageWithItems,
    createBenefitPackage,
    updateBenefitPackage,
    deleteBenefitPackage,
} from "../../../api/admin/benefitPackages";

import { getAllBenefits } from "../../../api/admin/benefits";

const PAGE_SIZE = 15;

const BenefitPackagesPage = () => {
    const { t } = useTranslation();
    const { token } = useAuth();

    const [packages, setPackages] = useState([]);
    const [benefits, setBenefits] = useState([]);
    const [loadingBenefits, setLoadingBenefits] = useState(false);

    const [loading, setLoading] = useState(false);
    const [error, setError] = useState("");

    const [page, setPage] = useState(1);

    const [createOpen, setCreateOpen] = useState(false);

    const [editOpen, setEditOpen] = useState(false);
    const [editPackage, setEditPackage] = useState(null);

    const [deleteOpen, setDeleteOpen] = useState(false);
    const [deletePackage, setDeletePackage] = useState(null);

    const total = packages.length;

    const pageItems = useMemo(() => {
        const start = (page - 1) * PAGE_SIZE;
        return packages.slice(start, start + PAGE_SIZE);
    }, [packages, page]);

    const fetchPackages = async () => {
        try {
            setLoading(true);
            setError("");
            const data = await getAllBenefitPackages(token);

            // sort: active first then name
            const sorted = [...data].sort((a, b) => {
                if (a.isActive !== b.isActive) return a.isActive ? -1 : 1;
                return String(a.name).localeCompare(String(b.name));
            });

            setPackages(sorted);
            setPage(1);
        } catch (e) {
            setError(e.message || t("admin.benefitPackages.errors.load"));
        } finally {
            setLoading(false);
        }
    };

    const fetchBenefits = async () => {
        try {
            setLoadingBenefits(true);
            const data = await getAllBenefits(token);

            // show active first
            const sorted = [...data].sort((a, b) => {
                if (a.isActive !== b.isActive) return a.isActive ? -1 : 1;
                return String(a.displayName).localeCompare(String(b.displayName));
            });

            setBenefits(sorted);
        } finally {
            setLoadingBenefits(false);
        }
    };

    useEffect(() => {
        fetchPackages();
        fetchBenefits();
        // eslint-disable-next-line react-hooks/exhaustive-deps
    }, []);

    const onCreate = async (dto) => {
        try {
            setLoading(true);
            setError("");
            await createBenefitPackage(token, dto);
            setCreateOpen(false);
            await fetchPackages();
        } catch (e) {
            setError(e.message || t("admin.benefitPackages.errors.create"));
        } finally {
            setLoading(false);
        }
    };

    const onEdit = (p) => {
        setEditPackage(p);
        setEditOpen(true);
    };

    const loadFullPackage = async (id) => {
        return await getBenefitPackageWithItems(token, id);
    };

    const onUpdate = async (dto) => {
        try {
            setLoading(true);
            setError("");
            await updateBenefitPackage(token, editPackage.id, dto);
            setEditOpen(false);
            setEditPackage(null);
            await fetchPackages();
        } catch (e) {
            setError(e.message || t("admin.benefitPackages.errors.edit"));
        } finally {
            setLoading(false);
        }
    };

    const onAskDelete = (p) => {
        setDeletePackage(p);
        setDeleteOpen(true);
    };

    const onConfirmDelete = async () => {
        try {
            setLoading(true);
            setError("");
            await deleteBenefitPackage(token, deletePackage.id);
            setDeleteOpen(false);
            setDeletePackage(null);
            await fetchPackages();
        } catch (e) {
            setError(e.message || t("admin.benefitPackages.errors.delete"));
        } finally {
            setLoading(false);
        }
    };

    return (
        <div>
            <div className="flex items-end justify-between">
                <div>
                    <h1 className="text-3xl font-extrabold">{t("admin.benefitPackages.title")}</h1>
                    <p className="text-gray-600 mt-1">
                        {t("admin.benefitPackages.total")}: {total}
                    </p>
                </div>

                <button
                    onClick={() => setCreateOpen(true)}
                    className="px-4 py-2 rounded bg-black text-white font-semibold hover:bg-gray-800"
                >
                    {t("admin.benefitPackages.add")}
                </button>
            </div>

            {error && <div className="mt-4 p-3 rounded bg-red-50 text-red-700 border">{error}</div>}

            <div className="mt-4 bg-white border rounded-xl overflow-hidden">
                <div className="grid grid-cols-7 gap-2 px-4 py-3 border-b font-semibold text-sm text-gray-700">
                    <div>{t("admin.benefitPackages.columns.id")}</div>
                    <div>{t("admin.benefitPackages.columns.name")}</div>
                    <div>{t("admin.benefitPackages.columns.weekday")}</div>
                    <div>{t("admin.benefitPackages.columns.weekend")}</div>
                    <div>{t("admin.benefitPackages.columns.items")}</div>
                    <div>{t("admin.benefitPackages.columns.active")}</div>
                    <div className="text-right">{t("admin.benefitPackages.columns.actions")}</div>
                </div>

                {loading ? (
                    <div className="p-4 text-gray-600">{t("admin.benefitPackages.loading")}</div>
                ) : pageItems.length === 0 ? (
                    <div className="p-4 text-gray-600">{t("admin.benefitPackages.empty")}</div>
                ) : (
                    pageItems.map((p) => (
                        <div key={p.id} className="grid grid-cols-7 gap-2 px-4 py-3 border-b text-sm items-center">
                            <div>{p.id}</div>
                            <div className="font-semibold truncate">{p.name}</div>
                            <div className="truncate">{p.scheduleWeekday}</div>
                            <div className="truncate">{p.scheduleWeekend}</div>
                            <div>{p.items?.length ?? 0}</div>
                            <div>
                                <span className={p.isActive ? "text-green-700" : "text-gray-500"}>
                                    {p.isActive ? t("admin.benefitPackages.statusActive") : t("admin.benefitPackages.statusInactive")}
                                </span>
                            </div>
                            <div className="flex justify-end gap-2">
                                <button className="px-3 py-1 rounded border" onClick={() => onEdit(p)}>
                                    {t("admin.benefitPackages.edit")}
                                </button>
                                <button
                                    className="px-3 py-1 rounded border border-red-300 text-red-700 hover:bg-red-50"
                                    onClick={() => onAskDelete(p)}
                                >
                                    {t("admin.benefitPackages.delete")}
                                </button>
                            </div>
                        </div>


                    ))
                )}
            </div>

            <Pagination page={page} pageSize={PAGE_SIZE} total={total} onPageChange={setPage} />

            <BenefitPackageFormModal
                open={createOpen}
                mode="create"
                initial={null}
                availableBenefits={benefits}
                loadingBenefits={loadingBenefits}
                onClose={() => setCreateOpen(false)}
                onSubmit={onCreate}
                loadFullPackage={null}
            />

            <BenefitPackageFormModal
                open={editOpen}
                mode="edit"
                initial={editPackage}
                availableBenefits={benefits}
                loadingBenefits={loadingBenefits}
                onClose={() => {
                    setEditOpen(false);
                    setEditPackage(null);
                }}
                onSubmit={onUpdate}
                loadFullPackage={loadFullPackage}
            />

            <ConfirmDialog
                open={deleteOpen}
                title={t("admin.benefitPackages.confirmDelete.title")}
                message={t("admin.benefitPackages.confirmDelete.message", { name: deletePackage?.name })}
                confirmText={t("admin.benefitPackages.confirmDelete.confirmText")}
                onConfirm={onConfirmDelete}
                onClose={() => {
                    setDeleteOpen(false);
                    setDeletePackage(null);
                }}
            />
        </div>
    );
};

export default BenefitPackagesPage;