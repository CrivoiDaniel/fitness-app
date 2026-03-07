import React, { useEffect, useRef, useState } from "react";
import logo from "../../assets/logo.png";
import { Link } from "react-router-dom";
import { useNavbarTrigger } from "../../context/NavbarTriggerContext";
import { useTranslation } from "react-i18next";
import i18n, { setLanguage } from "../../i18n";

const Navbar = () => {
  const { t } = useTranslation();
  const { triggerRef, enabled } = useNavbarTrigger();
  const [afterHero, setAfterHero] = useState(false);
  const [visible, setVisible] = useState(true);
  const lastScrollY = useRef(0);

  const [langOpen, setLangOpen] = useState(false);
  const currentLang = i18n.language || "ro";

  useEffect(() => {
    if (!enabled) {
      setAfterHero(true);
      return;
    }
    if (!triggerRef?.current) return;
    const observer = new IntersectionObserver(
      ([entry]) => setAfterHero(!entry.isIntersecting),
      { root: null, threshold: 0 }
    );
    observer.observe(triggerRef.current);
    return () => observer.disconnect();
  }, [enabled, triggerRef]);

  useEffect(() => {
    const handleScroll = () => {
      const currentScrollY = window.scrollY;

      if (currentScrollY <= 10) setVisible(true);
      else if (currentScrollY > lastScrollY.current) setVisible(false);
      else setVisible(true);

      lastScrollY.current = currentScrollY;
    };

    window.addEventListener("scroll", handleScroll, { passive: true });
    return () => window.removeEventListener("scroll", handleScroll);
  }, []);

  useEffect(() => {
    const onDocClick = (e) => {
      // închide dropdown când dai click în afară
      if (!e.target.closest?.("[data-lang-dropdown]")) setLangOpen(false);
    };
    document.addEventListener("click", onDocClick);
    return () => document.removeEventListener("click", onDocClick);
  }, []);

  return (
    <nav
      className={[
        "fixed top-0 left-0 right-0 z-50 py-2 transition-all duration-300",
        visible ? "translate-y-0" : "-translate-y-full",
      ].join(" ")}
    >
      <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
        <div
          className={[
            "flex justify-between items-center rounded-full px-6 py-1 border transition-all duration-300",
            afterHero
              ? "bg-black/85 border-white/10 shadow-lg shadow-black/40 backdrop-blur"
              : "bg-white/0 border-white/10 backdrop-blur",
          ].join(" ")}
        >
          <div>
            <Link to="/">
              <img src={logo} alt="logo" className="w-40 h-12" />
            </Link>
          </div>

          <div className="flex items-center gap-6">
            <Link to="/subscription" className="text-white text-lg font-bold tracking-wide hover:text-yellow-300">
              {t("nav.subscriptions")}
            </Link>
            <Link to="/trainers" className="text-white text-lg font-bold tracking-wide hover:text-yellow-300">
              {t("nav.trainers")}
            </Link>
            <Link to="/contacts" className="text-white text-lg font-bold tracking-wide hover:text-yellow-300">
              {t("nav.contacts")}
            </Link>
            <Link to="/services" className="text-white text-lg font-bold tracking-wide hover:text-yellow-300">
              {t("nav.services")}
            </Link>
          </div>

          <div className="flex items-center gap-4">
            {/* Language dropdown */}
            <div className="relative" data-lang-dropdown>
              <button
                type="button"
                onClick={() => setLangOpen((v) => !v)}
                className="text-white font-bold tracking-wide hover:text-yellow-300 border border-white/10 rounded-full px-4 py-2 backdrop-blur"
                aria-haspopup="menu"
                aria-expanded={langOpen}
              >
                {currentLang.toUpperCase()}
              </button>

              {langOpen && (
                <div className="absolute right-0 mt-2 w-28 rounded-xl border border-white/10 bg-black/90 backdrop-blur shadow-lg overflow-hidden">
                  {["ro", "en", "ru"].map((lng) => (
                    <button
                      key={lng}
                      className={[
                        "w-full text-left px-4 py-2 text-white hover:bg-white/10",
                        currentLang === lng ? "font-extrabold" : "font-semibold",
                      ].join(" ")}
                      onClick={() => {
                        setLanguage(lng);
                        setLangOpen(false);
                      }}
                    >
                      {lng.toUpperCase()}
                    </button>
                  ))}
                </div>
              )}
            </div>

            <Link to="/login" className="text-white font-bold tracking-wide hover:text-yellow-300">
              {t("nav.login")}
            </Link>
          </div>
        </div>
      </div>
    </nav>
  );
};

export default Navbar;