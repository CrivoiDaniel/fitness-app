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
          className="inline-block mt-10 backdrop-blur border border-white/10 rounded-md shadow shadow-yellow-400 py-3 px-5 text-md text-white font-semibold hover:bg-amber-300/80 hover:text-black cursor-pointer transition-all duration-300"
        >
          {t("home.buyNow")}
        </a>
      </div>
    </section>
  );
};

export default Hero;