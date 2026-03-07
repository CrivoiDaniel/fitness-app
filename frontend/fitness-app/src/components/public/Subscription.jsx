import React, { useEffect, useState } from "react";
import CardSubscription from "./CardSubscription";
import { getPublicSubscriptionPlans } from "../../api/public/subscriptions";
import { useTranslation } from "react-i18next";

const Subscription = () => {
  const { t } = useTranslation();
  const [plans, setPlans] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    let cancelled = false;

    async function load() {
      try {
        setLoading(true);
        const data = await getPublicSubscriptionPlans();
        if (!cancelled) setPlans(data);
      } catch (e) {
        if (!cancelled) setError(e.message || t("errors.unknown"));
      } finally {
        if (!cancelled) setLoading(false);
      }
    }

    load();
    return () => {
      cancelled = true;
    };
  }, [t]);

  return (
    <section>
      <div className="flex flex-col items-center justify-center">
        <p className="text-black text-xl">{t("home.subsTitleSmall")}</p>
        <div className="mt-2 flex flex-col items-center justify-center gap-y-0">
          <h2 className="text-5xl italic font-extrabold leading-none m-0">{t("home.subsTitle1")}</h2>
          <h2 className="text-5xl italic font-extrabold leading-none m-0">{t("home.subsTitle2")}</h2>
        </div>
        <p className="text-black text-xl">{t("home.subsSubtitle")}</p>
      </div>

      <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 mt-25">
        {loading && <p>{t("home.loadingSubscriptions")}</p>}
        {error && <p className="text-red-600">{error}</p>}

        {!loading && !error && (
          <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-8">
            {plans.map((p, index) => (
              <CardSubscription key={p.id} plan={p} index={index} />
            ))}
          </div>
        )}
      </div>
    </section>
  );
};

export default Subscription;