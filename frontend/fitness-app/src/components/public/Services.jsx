import React from "react";
import { useTranslation } from "react-i18next";
import { FiTarget, FiUsers, FiSunrise, FiZap, FiActivity, FiAward } from "react-icons/fi";

const ServiceCard = ({ icon: Icon, title, description }) => (
  <div className="group p-8 bg-slate-950 rounded-3xl border border-slate-900 shadow-xl hover:shadow-yellow-400/10 hover:-translate-y-2 transition-all duration-300">
    <div className="w-14 h-14 bg-yellow-400 text-black rounded-2xl flex items-center justify-center mb-6 group-hover:bg-white transition-colors duration-300">
      <Icon size={28} />
    </div>
    <h3 className="text-xl font-bold text-white mb-3 uppercase italic tracking-tight">{title}</h3>
    <p className="text-slate-400 leading-relaxed font-medium">{description}</p>
  </div>
);

const Services = () => {
  const { t } = useTranslation();

  const servicesList = [
    { icon: FiTarget, title: t("home.servicesList.personal.title"), description: t("home.servicesList.personal.desc") },
    { icon: FiUsers, title: t("home.servicesList.group.title"), description: t("home.servicesList.group.desc") },
    { icon: FiSunrise, title: t("home.servicesList.yoga.title"), description: t("home.servicesList.yoga.desc") },
    { icon: FiZap, title: t("home.servicesList.crossfit.title"), description: t("home.servicesList.crossfit.desc") },
    { icon: FiActivity, title: t("home.servicesList.nutrition.title"), description: t("home.servicesList.nutrition.desc") },
    { icon: FiAward, title: t("home.servicesList.recovery.title"), description: t("home.servicesList.recovery.desc") }
  ];

  return (
    <section className="py-24 px-6 max-w-7xl mx-auto bg-white">
      <div className="text-center mb-16">
        <h2 className="text-4xl md:text-6xl font-black text-black mb-4 tracking-tighter uppercase italic">
          {t("home.servicesTitle")}
        </h2>
        <div className="w-24 h-2 bg-yellow-400 mx-auto mb-6"></div>
        <p className="text-slate-500 text-lg max-w-2xl mx-auto font-bold uppercase tracking-tight">
          {t("home.servicesSubtitle")}
        </p>
      </div>

      <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-8">
        {servicesList.map((service, index) => (
          <ServiceCard key={index} {...service} />
        ))}
      </div>
    </section>
  );
};

export default Services;