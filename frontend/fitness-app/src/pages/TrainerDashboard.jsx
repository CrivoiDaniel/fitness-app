import React from "react";
import { useAuth } from "../context/AuthContext";
import { useTranslation } from "react-i18next";
import { Link } from "react-router-dom";
import { 
  LuUsers, 
  LuCalendar, 
  LuDumbbell, 
  LuSettings, 
  LuLogOut,
  LuChevronRight,
  LuPlus
} from "react-icons/lu";

const TrainerDashboard = () => {
  const { t } = useTranslation();
  const { user, logout } = useAuth();

  const cards = [
    {
      title: "Clienții Mei",
      description: "Gestionează lista de clienți și urmărește progresul lor.",
      icon: <LuUsers size={32} />,
      link: "/dashboard/admin/clients", // Refolosim pagina de admin pentru acum
      color: "bg-blue-500"
    },
    {
      title: "Calendar & Programări",
      description: "Vizualizează sesiunile programate și timpul liber.",
      icon: <LuCalendar size={32} />,
      link: "/dashboard/trainer/calendar",
      color: "bg-emerald-500"
    },
    {
      title: "Editor Planuri (Command)",
      description: "Creează programe de antrenament cu suport Undo/Redo.",
      icon: <LuDumbbell size={32} />,
      link: "/dashboard/trainer/workout-editor",
      color: "bg-indigo-600",
      featured: true
    }
  ];

  return (
    <div className="min-h-screen bg-neutral-950 text-white p-6 lg:p-12 font-sans">
      <div className="max-w-6xl mx-auto">
        
        {/* HEADER */}
        <header className="flex flex-col md:flex-row md:items-center justify-between gap-8 mb-16">
          <div>
            <div className="flex items-center gap-2 text-indigo-500 font-bold tracking-tight mb-2 uppercase text-xs">
              <span className="w-8 h-[2px] bg-indigo-500"></span>
              Trainer Portal
            </div>
            <h1 className="text-4xl lg:text-5xl font-black italic uppercase tracking-tighter">
              Salut, {user?.firstName}!
            </h1>
            <p className="text-white/40 mt-2 font-medium">Ești gata să schimbi vieți astăzi? {user?.email}</p>
          </div>

          <button 
            onClick={logout}
            className="flex items-center gap-2 px-6 py-3 rounded-2xl bg-white/5 border border-white/10 hover:bg-white/10 transition-all font-bold text-sm text-red-400"
          >
            <LuLogOut size={18} />
            {t("common.logout")}
          </button>
        </header>

        {/* DASHBOARD GRID */}
        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
          {cards.map((card, i) => (
            <Link 
              key={i}
              to={card.link}
              className={`group relative overflow-hidden rounded-[32px] p-8 border border-white/10 transition-all hover:scale-[1.02] hover:shadow-2xl hover:shadow-indigo-500/10 ${
                card.featured ? "bg-white/5 border-indigo-500/30 ring-1 ring-indigo-500/20" : "bg-white/5"
              }`}
            >
              <div className={`w-14 h-14 rounded-2xl ${card.color} flex items-center justify-center mb-8 shadow-lg`}>
                {card.icon}
              </div>
              
              <h3 className="text-2xl font-black italic uppercase mb-2 group-hover:text-indigo-400 transition-colors">
                {card.title}
              </h3>
              <p className="text-white/50 text-sm font-medium leading-relaxed mb-8">
                {card.description}
              </p>

              <div className="flex items-center justify-between pt-6 border-t border-white/10">
                <span className="text-[10px] font-black uppercase tracking-widest text-indigo-500">Accesează Acum</span>
                <LuChevronRight size={20} className="text-indigo-500 group-hover:translate-x-1 transition-all" />
              </div>

              {card.featured && (
                <div className="absolute top-6 right-6 px-3 py-1 bg-indigo-600 rounded-full text-[10px] font-black uppercase tracking-tighter">
                  pattern: command
                </div>
              )}
            </Link>
          ))}

          {/* ADD NEW CARD PLACEHOLDER */}
          <div className="rounded-[32px] p-8 border border-dashed border-white/10 flex flex-col items-center justify-center text-white/20 hover:text-white/40 hover:border-white/20 transition-all cursor-pointer">
             <LuPlus size={48} strokeWidth={1} className="mb-4" />
             <span className="font-black uppercase text-xs tracking-widest">Adaugă Widget</span>
          </div>
        </div>

        {/* STATS SECTION */}
        <section className="mt-16 grid grid-cols-1 md:grid-cols-4 gap-6">
           {[
             { label: "Clienți Activi", value: "12" },
             { label: "Sesiuni Azi", value: "4" },
             { label: "Venit Lună", value: "5.400 MDL" },
             { label: "Rating", value: "4.9/5" }
           ].map((stat, i) => (
             <div key={i} className="bg-white/5 rounded-2xl p-6 border border-white/5">
                <div className="text-[10px] font-black text-white/40 uppercase tracking-widest mb-1">{stat.label}</div>
                <div className="text-2xl font-black italic text-indigo-400">{stat.value}</div>
             </div>
           ))}
        </section>

      </div>
    </div>
  );
};

export default TrainerDashboard;