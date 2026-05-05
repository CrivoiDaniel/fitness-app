import React from "react";
import { useTranslation } from "react-i18next";

const Equipment = () => {
  const { t } = useTranslation();
  const brands = [
    { name: "Technogym", type: "Cardio & Strength" },
    { name: "Rogue", type: "Crossfit & Free Weights" },
    { name: "Matrix", type: "Digital Experience" },
    { name: "Life Fitness", type: "Performance Training" }
  ];

  return (
    <section className="py-24 bg-white text-slate-900 overflow-hidden relative">
      {/* Decorative background element */}
      <div className="absolute top-0 right-0 w-1/3 h-full bg-yellow-400/5 skew-x-12 translate-x-20" />
      
      <div className="max-w-7xl mx-auto px-6 relative z-10">
        <div className="flex flex-col lg:flex-row items-center justify-between gap-16">
          <div className="lg:w-1/2">
            <span className="text-yellow-500 font-bold tracking-widest uppercase text-sm mb-4 block">
              Pro-Level Performance
            </span>
            <h2 className="text-4xl md:text-6xl font-black mb-8 leading-tight italic uppercase tracking-tighter">
              {t("home.equipmentTitle")} <br />
              <span className="text-transparent bg-clip-text bg-gradient-to-r from-yellow-400 to-yellow-600">
                {t("home.equipmentSubtitle")}
              </span>
            </h2>
            <p className="text-slate-500 text-lg mb-10 leading-relaxed font-medium">
              {t("home.equipmentDescription")}
            </p>
            
            <div className="grid grid-cols-2 gap-6">
              {brands.map((brand, i) => (
                <div key={i} className="border-l-4 border-yellow-400 pl-4 py-2 bg-slate-50 hover:bg-black hover:text-white transition-all duration-300 rounded-r-xl shadow-sm">
                  <h4 className="font-bold text-xl">{brand.name}</h4>
                  <span className="text-slate-500 text-sm">{brand.type}</span>
                </div>
              ))}
            </div>
          </div>
          
          <div className="lg:w-1/2 relative">
             <div className="relative rounded-[3rem] overflow-hidden shadow-2xl border border-slate-100 group">
                <img 
                  src="https://images.unsplash.com/photo-1540497077202-7c8a3999166f?ixlib=rb-4.0.3&auto=format&fit=crop&w=1000&q=80" 
                  alt="Gym Equipment" 
                  className="w-full h-auto object-cover group-hover:scale-105 transition-transform duration-700 grayscale hover:grayscale-0"
                />
                <div className="absolute inset-0 bg-gradient-to-t from-black/40 via-transparent to-transparent opacity-60" />
             </div>
             
             {/* Floating badge */}
             <div className="absolute -bottom-6 -left-6 bg-black p-8 rounded-3xl shadow-2xl hidden md:block border border-yellow-400/30">
                <p className="text-3xl font-black italic text-yellow-400 tracking-tighter uppercase">TOP BRANDS</p>
                <p className="text-white/60 text-xs uppercase tracking-widest mt-1">Only elite equipment</p>
             </div>
          </div>
        </div>
      </div>
    </section>
  );
};

export default Equipment;
