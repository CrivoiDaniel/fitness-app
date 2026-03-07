import React, { useMemo, useState } from "react";
import { NavLink, useNavigate } from "react-router-dom";
import { useAuth } from "../../context/AuthContext";
import { useTranslation } from "react-i18next";
import { FiLogOut } from "react-icons/fi";

const cx = (...arr) => arr.filter(Boolean).join(" ");

const SidebarItem = ({ collapsed, to, end, icon, label, onClick }) => {
  return (
    <NavLink
      to={to}
      end={end}
      onClick={onClick}
      className={({ isActive }) =>
        cx(
          "group relative flex items-center gap-3 rounded-xl px-3 py-2",
          "transition-colors",
          isActive ? "bg-white/10 text-white" : "text-white/80 hover:bg-white/10 hover:text-white"
        )
      }
    >
      <div className="w-10 flex justify-center text-xl">{icon}</div>

      <div className={cx("overflow-hidden whitespace-nowrap transition-all duration-200", collapsed ? "opacity-0 w-0" : "opacity-100")}>
        <span className="text-sm font-semibold">{label}</span>
      </div>

      {collapsed && (
        <div
          className={cx(
            "pointer-events-none absolute left-[64px] top-1/2 -translate-y-1/2",
            "opacity-0 group-hover:opacity-100 transition-opacity duration-150",
            "bg-black text-white text-xs font-semibold px-3 py-2 rounded-lg shadow-lg",
            "border border-white/10 whitespace-nowrap"
          )}
        >
          {label}
        </div>
      )}
    </NavLink>
  );
};

const Sidebar = ({ items, brand = "Dashboard" }) => {
  const { t } = useTranslation();
  const { user, logout } = useAuth();
  const navigate = useNavigate();

  const [hovered, setHovered] = useState(false);

  const collapsed = !hovered;
  const widthClass = hovered ? "w-64" : "w-16";

  const onLogout = () => {
    logout();
    navigate("/login", { replace: true });
  };

  const userLabel = useMemo(() => {
    const fn = user?.firstName ?? "";
    const ln = user?.lastName ?? "";
    const full = `${fn} ${ln}`.trim();
    return full || user?.email || "";
  }, [user]);

  return (
    <aside
      onMouseEnter={() => setHovered(true)}
      onMouseLeave={() => setHovered(false)}
      className={cx(
        "h-screen sticky top-0",
        "bg-[#0B0F19] text-white",
        "border-r border-white/10",
        "transition-all duration-200 ease-out",
        widthClass
      )}
    >
      <div className="h-16 flex items-center px-3 border-b border-white/10">
        <div className="w-10 h-10 rounded-xl bg-yellow-400/80 flex items-center justify-center font-extrabold text-black">
          F
        </div>

        <div className={cx("ml-3 overflow-hidden transition-all duration-200", collapsed ? "opacity-0 w-0" : "opacity-100")}>
          <div className="font-extrabold leading-tight">{brand}</div>
          <div className="text-xs text-white/60 truncate max-w-[180px]">{userLabel}</div>
        </div>
      </div>

      <nav className="p-2 space-y-1">
        {items.map((it) => (
          <SidebarItem
            key={it.to}
            collapsed={collapsed}
            to={it.to}
            end={it.end}
            icon={it.icon}
            label={t(it.labelKey)}
            onClick={() => setHovered(false)}
          />
        ))}
      </nav>

      <div className="absolute bottom-0 left-0 right-0 p-2 border-t border-white/10">
        <button
          onClick={onLogout}
          className={cx(
            "group relative w-full flex items-center gap-3 rounded-xl px-3 py-2",
            "text-white/80 hover:bg-white/10 hover:text-white transition-colors"
          )}
        >
          <div className="w-10 flex justify-center text-xl">
            <FiLogOut />
          </div>

          <div className={cx("overflow-hidden whitespace-nowrap transition-all duration-200", collapsed ? "opacity-0 w-0" : "opacity-100")}>
            <span className="text-sm font-semibold">{t("common.logout")}</span>
          </div>

          {collapsed && (
            <div
              className={cx(
                "pointer-events-none absolute left-[64px] top-1/2 -translate-y-1/2",
                "opacity-0 group-hover:opacity-100 transition-opacity duration-150",
                "bg-black text-white text-xs font-semibold px-3 py-2 rounded-lg shadow-lg",
                "border border-white/10 whitespace-nowrap"
              )}
            >
              {t("common.logout")}
            </div>
          )}
        </button>
      </div>
    </aside>
  );
};

export default Sidebar;