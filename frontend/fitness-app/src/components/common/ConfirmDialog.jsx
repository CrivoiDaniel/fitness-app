import React from "react";
import Modal from "./Modal";
import { useTranslation } from "react-i18next";

const ConfirmDialog = ({
  open,
  title, // optional
  message, // optional
  confirmText, // optional
  cancelText, // optional
  danger = true,
  onConfirm,
  onClose,
}) => {
  const { t } = useTranslation();

  return (
    <Modal
      open={open}
      title={title ?? t("common.confirm")}
      onClose={onClose}
    >
      <p className="text-gray-700">{message ?? "—"}</p>

      <div className="mt-6 flex justify-end gap-3">
        <button className="px-4 py-2 rounded border" onClick={onClose}>
          {cancelText ?? t("common.cancel")}
        </button>
        <button
          className={[
            "px-4 py-2 rounded font-semibold text-white",
            danger ? "bg-red-600 hover:bg-red-700" : "bg-black hover:bg-gray-800",
          ].join(" ")}
          onClick={onConfirm}
        >
          {confirmText ?? t("common.ok")}
        </button>
      </div>
    </Modal>
  );
};

export default ConfirmDialog;