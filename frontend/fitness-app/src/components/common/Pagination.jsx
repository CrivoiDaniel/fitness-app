import React from "react";
import { useTranslation } from "react-i18next";

const Pagination = ({ page, pageSize, total, onPageChange }) => {
  const { t } = useTranslation();
  const totalPages = Math.max(1, Math.ceil(total / pageSize));

  return (
    <div className="flex items-center justify-between mt-4">
      <p className="text-sm text-gray-600">
        {t("common.page")} {page} {t("common.of")} {totalPages} ({t("common.total")}: {total})
      </p>

      <div className="flex gap-2">
        <button
          className="px-3 py-2 rounded border disabled:opacity-50"
          disabled={page <= 1}
          onClick={() => onPageChange(page - 1)}
        >
          {t("common.back")}
        </button>
        <button
          className="px-3 py-2 rounded border disabled:opacity-50"
          disabled={page >= totalPages}
          onClick={() => onPageChange(page + 1)}
        >
          {t("common.next")}
        </button>
      </div>
    </div>
  );
};

export default Pagination;