import React from "react";
import { IoMdCheckmark } from "react-icons/io";
import { useTranslation } from "react-i18next";

import monthLogo from "../../assets/month-logo.png";
import threeMonthLogo from "../../assets/3month-logo.png";
import sixMonthLogo from "../../assets/6month-logo.png";

const typeToImage = {
  Monthly: monthLogo,
  Quarterly: threeMonthLogo,
  Yearly: sixMonthLogo,
};

const CardSubscription = ({ plan, index }) => {
  const { t } = useTranslation();

  const imgSrc = typeToImage[plan.type] ?? monthLogo;
  const typeText = t(`subscription.type.${plan.type}`, plan.type);

  return (
    <div
      className={`relative bg-white h-[95%] shadow-2xl shadow-black/20 p-6 flex flex-col gap-4 transition-all duration-300
      ${index === 1 ? "-mt-10 scale-105 z-10" : "mt-10"}`}
    >
      <div className="absolute flex flex-col justify-between left-0 top-0 w-full py-2 border border-yellow-400/80 bg-yellow-400/80" />

      <div>
        <div className="absolute -top-12 right-2">
          <img src={imgSrc} alt={`${typeText} logo`} className="h-55 w-auto" />
        </div>

        <div className="flex items-baseline justify-between mt-5">
          <h3 className="text-3xl font-extrabold italic">{typeText}</h3>
        </div>
      </div>

      <div className="mt-10">
        <div className="mt-8 text-4xl font-extrabold italic text-center">
          {plan.price} MDL
        </div>

        <div className="mt-10">
          {plan.benefits && plan.benefits.length > 0 ? (
            <ul className="pl-5 space-y-1">
              {plan.benefits.map((b, idx) => (
                <li key={`${b.name}-${idx}`}>
                  <div className="flex space-x-2">
                    <IoMdCheckmark className="w-4 h-4 text-yellow-400/80" />
                    {b.displayName}
                  </div>
                </li>
              ))}
            </ul>
          ) : (
            <p className="text-sm text-red-600">{t("home.noBenefits")}</p>
          )}
        </div>
      </div>

      <button className="mt-10 bg-white text-black py-2 rounded-md shadow-md shadow-yellow-400 font-semibold hover:bg-black hover:text-white transition-colors duration-300 cursor-pointer">
        {t("home.purchase")}
      </button>
    </div>
  );
};

export default CardSubscription;