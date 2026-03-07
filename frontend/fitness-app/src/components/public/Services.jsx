import React from "react";
import { useTranslation } from "react-i18next";

const Services = () => {
  const { t } = useTranslation();

  return (
    <section>
      <div className="flex justify-center items-center italic">
        <h1 className="font-bold text-5xl">{t("home.servicesTitle")}</h1>
      </div>
    </section>
  );
};

export default Services;