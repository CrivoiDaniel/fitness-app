import React, { useEffect } from "react";
import Subscription from "../../components/public/Subscription";
import Benefits from "../../components/public/Benefits";
import Hero from "../../components/public/Hero";
import Services from "../../components/public/Services";
import { useNavbarTrigger } from "../../context/NavbarTriggerContext";

const Home = () => {
  const { triggerRef, setEnabled } = useNavbarTrigger();

  useEffect(() => {
    setEnabled(true);
    return () => setEnabled(false);
  }, [setEnabled]);

  return (
    <main className="w-full">
      <Hero />

      {/* sentinel imediat după Hero */}
      <div ref={triggerRef} />

      <div className="mt-10">
        <Subscription />
      </div>

      <div className="mt-30">
        <Benefits />
      </div>

      <div className="mt-30">
        <Services />
      </div>
    </main>
  );
};

export default Home;