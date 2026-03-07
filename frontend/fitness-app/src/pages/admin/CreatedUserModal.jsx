import React, { useMemo } from "react";
import Modal from "../../components/common/Modal";
import { useTranslation } from "react-i18next";

const CreatedUserModal = ({ open, data, onClose }) => {
  const { t } = useTranslation();

  const email = data?.email ?? "";
  const temp = data?.temporaryPassword ?? "";

  const infoText = useMemo(() => {
    if (!data) return "";
    return t("admin.createdUser.template", { email, password: temp });
  }, [data, t, email, temp]);

  if (!open || !data) return null;

  const copy = async (text) => {
    try {
      await navigator.clipboard.writeText(text);
    } catch {}
  };

  return (
    <Modal open={open} title={t("admin.createdUser.title")} onClose={onClose}>
      <p className="text-gray-700">{t("admin.createdUser.hint")}</p>

      <div className="mt-4 grid gap-3">
        <div className="border rounded p-3">
          <div className="text-xs text-gray-500">{t("admin.createdUser.email")}</div>
          <div className="font-semibold break-all">{email}</div>
          <button className="mt-2 px-3 py-1 rounded border hover:bg-gray-50" onClick={() => copy(email)}>
            {t("admin.createdUser.copyEmail")}
          </button>
        </div>

        <div className="border rounded p-3">
          <div className="text-xs text-gray-500">{t("admin.createdUser.tempPassword")}</div>
          <div className="font-semibold break-all">{temp}</div>
          <button className="mt-2 px-3 py-1 rounded border hover:bg-gray-50" onClick={() => copy(temp)}>
            {t("admin.createdUser.copyPassword")}
          </button>
        </div>

        <div className="border rounded p-3">
          <div className="text-xs text-gray-500">{t("admin.createdUser.fullMessage")}</div>
          <pre className="mt-2 whitespace-pre-wrap text-sm text-gray-700">{infoText}</pre>
          <button className="mt-2 px-3 py-1 rounded border hover:bg-gray-50" onClick={() => copy(infoText)}>
            {t("admin.createdUser.copyMessage")}
          </button>
        </div>
      </div>

      <div className="mt-6 flex justify-end">
        <button onClick={onClose} className="px-4 py-2 rounded bg-black text-white font-semibold hover:bg-gray-800">
          {t("common.ok")}
        </button>
      </div>
    </Modal>
  );
};

export default CreatedUserModal;