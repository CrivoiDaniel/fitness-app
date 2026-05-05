import React from "react";
import { useTranslation } from "react-i18next";
import { FiCheckCircle, FiClock, FiHeart, FiTrendingUp, FiUsers, FiAward } from "react-icons/fi";

const StatsSurprise = () => {
  const { t } = useTranslation();
  const stats = [
    { icon: FiUsers, value: "1000+", label: t("home.statsMembers"), color: "text-yellow-400" },
    { icon: FiAward, value: "50+", label: t("home.statsCoaches"), color: "text-yellow-400" },
    { icon: FiClock, value: "24/7", label: t("home.statsAccess"), color: "text-yellow-400" },
    { icon: FiHeart, value: "100%", label: t("home.statsSatisfaction"), color: "text-yellow-400" }
  ];

  const features = [
    t("benefits.b1"),
    t("benefits.b2"),
    t("benefits.b3"),
    t("benefits.b4")
  ];

  return (
    <section className="py-24 bg-white text-black">
      <div className="max-w-7xl mx-auto px-6">
        <div className="grid grid-cols-2 md:grid-cols-4 gap-8 mb-20">
          {stats.map((stat, i) => (
            <div key={i} className="text-center group bg-black p-8 rounded-3xl shadow-xl hover:shadow-yellow-400/10 transition-all">
              <div className={`text-4xl md:text-5xl font-black mb-2 transition-transform duration-300 group-hover:scale-110 ${stat.color}`}>
                {stat.value}
              </div>
              <p className="text-slate-500 font-medium uppercase tracking-widest text-xs">
                {stat.label}
              </p>
            </div>
          ))}
        </div>

        <div className="bg-slate-950 rounded-[3rem] p-8 md:p-16 flex flex-col md:flex-row items-center gap-12 shadow-2xl border border-slate-900">
          <div className="md:w-1/2">
            <h2 className="text-3xl md:text-5xl font-black text-white mb-6 leading-tight uppercase italic tracking-tighter">
              {t("home.whyTitle")}
            </h2>
            <p className="text-slate-400 text-lg mb-8 font-medium">
              {t("home.whySubtitle")}
            </p>
            <div className="space-y-4">
              {features.map((f, i) => (
                <div key={i} className="flex items-center gap-3">
                  <div className="w-6 h-6 rounded-full bg-yellow-400 flex items-center justify-center text-black">
                    <FiCheckCircle size={16} />
                  </div>
                  <span className="text-slate-200 font-bold uppercase tracking-tight text-sm">{f}</span>
                </div>
              ))}
            </div>
          </div>
          
          <div className="md:w-1/2 grid grid-cols-2 gap-4">
            <div className="space-y-4 pt-12">
              <div className="h-64 rounded-2xl bg-[url('https://images.unsplash.com/photo-1571019613454-1cb2f99b2d8b?ixlib=rb-4.0.3&auto=format&fit=crop&w=600&q=80')] bg-cover bg-center shadow-lg grayscale hover:grayscale-0 transition-all duration-500" />
              <div className="h-40 rounded-2xl bg-yellow-400 flex items-center justify-center text-black p-6 shadow-lg shadow-yellow-400/20">
                <p className="text-xl font-black italic text-center uppercase tracking-tighter leading-none">Results guaranteed by experts</p>
              </div>
            </div>
            <div className="space-y-4">
               <div className="h-40 rounded-2xl bg-black flex items-center justify-center text-white p-6 border border-slate-800">
                <FiTrendingUp size={48} className="text-yellow-400" />
              </div>
              <div className="h-64 rounded-2xl bg-[url('https://images.unsplash.com/photo-1593079831268-3381b0db4a77?ixlib=rb-4.0.3&auto=format&fit=crop&w=600&q=80')] bg-cover bg-center shadow-lg grayscale hover:grayscale-0 transition-all duration-500" />
            </div>
          </div>
        </div>
      </div>
    </section>
  );
};

export default StatsSurprise;
