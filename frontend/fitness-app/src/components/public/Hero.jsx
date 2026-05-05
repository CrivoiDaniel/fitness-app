import React from "react";
import { useTranslation } from "react-i18next";

const Hero = () => {
  const { t } = useTranslation();

  return (
    <section className="relative w-full min-h-screen bg-[url(./assets/home.png)] bg-cover bg-center flex items-center justify-center overflow-hidden">
      <div className="absolute inset-0 bg-black/50"></div>

      <div className="relative text-center z-10">
        <h2 className="text-5xl text-white font-bold italic">{t("home.heroTitle")}</h2>

        <div className="relative flex justify-center mt-8">
          <p className="absolute -left-20 top-2 line-through text-white/80 text-2xl italic">
            {t("home.oldPrice")}
          </p>
          <p className="text-white text-7xl font-bold italic">{t("home.newPrice")}</p>
        </div>

        <a
          href="#subscriptions"
          className="inline-block mt-10 bg-yellow-400 border-2 border-yellow-400 rounded-lg shadow-lg shadow-yellow-400/20 py-4 px-10 text-lg text-black font-black uppercase italic hover:bg-black hover:text-yellow-400 transition-all duration-300"
        >
          {t("home.buyNow")}
        </a>
      </div>
    </section>
  );
};

export default Hero;