import React from "react";
import { useTranslation } from "react-i18next";
import { FiFacebook, FiInstagram, FiTwitter, FiYoutube, FiMail, FiPhone, FiMapPin } from "react-icons/fi";

const Footer = () => {
  const { t } = useTranslation();
  
  return (
    <footer className="bg-slate-950 text-slate-300 pt-20 pb-10">
      <div className="max-w-7xl mx-auto px-6">
        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-12 mb-16">
          {/* Brand info */}
          <div className="space-y-6">
            <div className="flex items-center gap-2">
              <div className="w-10 h-10 bg-yellow-400 rounded-lg flex items-center justify-center text-black font-black italic">F</div>
              <span className="text-2xl font-black text-white italic tracking-tighter uppercase">FitnessApp</span>
            </div>
            <p className="text-slate-500 leading-relaxed">
              {t("home.footerAbout")}
            </p>
            <div className="flex gap-4">
              <a href="#" className="w-10 h-10 rounded-full bg-slate-900 flex items-center justify-center hover:bg-yellow-400 hover:text-black transition-all"><FiFacebook size={18} /></a>
              <a href="#" className="w-10 h-10 rounded-full bg-slate-900 flex items-center justify-center hover:bg-yellow-400 hover:text-black transition-all"><FiInstagram size={18} /></a>
              <a href="#" className="w-10 h-10 rounded-full bg-slate-900 flex items-center justify-center hover:bg-yellow-400 hover:text-black transition-all"><FiTwitter size={18} /></a>
              <a href="#" className="w-10 h-10 rounded-full bg-slate-900 flex items-center justify-center hover:bg-yellow-400 hover:text-black transition-all"><FiYoutube size={18} /></a>
            </div>
          </div>

          {/* Quick Links */}
          <div>
            <h4 className="text-white font-bold mb-6 uppercase tracking-wider">{t("nav.services")}</h4>
            <ul className="space-y-4">
              <li><a href="#" className="hover:text-yellow-400 transition-colors">{t("nav.subscriptions")}</a></li>
              <li><a href="#" className="hover:text-yellow-400 transition-colors">{t("nav.trainers")}</a></li>
              <li><a href="#" className="hover:text-yellow-400 transition-colors">{t("nav.contacts")}</a></li>
            </ul>
          </div>

          {/* Opening Hours */}
          <div>
            <h4 className="text-white font-bold mb-6 uppercase tracking-wider">{t("home.footerHours")}</h4>
            <ul className="space-y-4">
              <li className="flex justify-between">
                <span>{t("common.days.monday")} - {t("common.days.friday")}:</span>
                <span className="text-white">06:00 - 23:00</span>
              </li>
              <li className="flex justify-between">
                <span>{t("common.days.saturday")}:</span>
                <span className="text-white">08:00 - 21:00</span>
              </li>
              <li className="flex justify-between">
                <span>{t("common.days.sunday")}:</span>
                <span className="text-white">09:00 - 19:00</span>
              </li>
            </ul>
          </div>

          {/* Contact */}
          <div>
            <h4 className="text-white font-bold mb-6 uppercase tracking-wider">{t("home.footerContact")}</h4>
            <ul className="space-y-4">
              <li className="flex items-start gap-3">
                <FiMapPin className="mt-1 text-yellow-400" />
                <span>123 Performance Way,<br />Fitness District, NY 10001</span>
              </li>
              <li className="flex items-center gap-3">
                <FiPhone className="text-yellow-400" />
                <span>+1 (555) 000-1234</span>
              </li>
              <li className="flex items-center gap-3">
                <FiMail className="text-yellow-400" />
                <span>hello@fitnessapp.com</span>
              </li>
            </ul>
          </div>
        </div>

        <div className="border-t border-slate-900 pt-8 flex flex-col md:flex-row justify-between items-center gap-4 text-sm text-slate-600">
          <p>© 2026 FitnessApp. All rights reserved.</p>
          <div className="flex gap-8">
            <a href="#" className="hover:text-slate-400 transition-colors">Privacy Policy</a>
            <a href="#" className="hover:text-slate-400 transition-colors">Terms of Service</a>
            <a href="#" className="hover:text-slate-400 transition-colors">Cookie Settings</a>
          </div>
        </div>
      </div>
    </footer>
  );
};

export default Footer;
