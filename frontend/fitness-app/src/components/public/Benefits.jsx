import React from "react";
import { IoMdCheckmark } from "react-icons/io";
import { useTranslation } from "react-i18next";

const Benefits = () => {
  const { t } = useTranslation();

  const benefits = [
    t("benefits.b1"),
    t("benefits.b2"),
    t("benefits.b3"),
    t("benefits.b4"),
    t("benefits.b5"),
    t("benefits.b6"),
    t("benefits.b7"),
    t("benefits.b8")
  ];

  return (
    <section className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
      <div className="flex justify-between">
        <div className="flex flex-col justify-between">
          <h1 className="text-8xl">
            <span className="block italic font-extrabold">{t("home.benefitsTitle1")}</span>
            <span className="pl-20 block italic font-extrabold text-yellow-400/80">
              {t("home.benefitsTitle2")}
            </span>
          </h1>

          <div className="w-1/2 flex gap-5">
            <button className="py-2 px-16 rounded-md bg-black shadow-md shadow-black text-white font-semibold hover:bg-yellow-400/80 hover:text-black transition-colors duration-300 cursor-pointer">
              {t("home.signUp")}
            </button>
            <button className="py-2 px-16 rounded-md shadow-md shadow-black font-semibold hover:bg-yellow-400/80 hover:text-black transition-colors duration-300 cursor-pointer">
              {t("home.location")}
            </button>
          </div>
        </div>

        <div>
          <h2 className="text-3xl font-extrabold italic">
            <span className="block">{t("home.benefitsHeading1")}</span>
            <span className="block">{t("home.benefitsHeading2")}</span>
          </h2>

          <div className="mt-8 space-y-2">
            {benefits.map((benefit) => (
              <div key={benefit} className="flex items-center space-x-2">
                <IoMdCheckmark className="w-4 h-4 text-yellow-400/80" />
                <p>{benefit}</p>
              </div>
            ))}
          </div>
        </div>
      </div>
    </section>
  );
};

export default Benefits;