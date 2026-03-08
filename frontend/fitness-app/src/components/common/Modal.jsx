import React from "react";

const Modal = ({ open, title, onClose, children }) => {
  if (!open) return null;

  return (
    <div className="fixed inset-0 z-[100] flex items-center justify-center px-4">
      <div className="absolute inset-0 bg-black/60" onClick={onClose} />

      {/* IMPORTANT: max height + flex-col */}
      <div className="relative w-full max-w-xl rounded-xl bg-white shadow-lg max-h-[85vh] flex flex-col">
        <div className="flex items-center justify-between border-b px-5 py-4 flex-none">
          <h2 className="text-lg font-bold">{title}</h2>
          <button onClick={onClose} className="px-2 py-1 rounded hover:bg-gray-100">
            ✕
          </button>
        </div>

        {/* IMPORTANT: scroll only aici */}
        <div className="p-5 overflow-y-auto">
          {children}
        </div>
      </div>
    </div>
  );
};

export default Modal;