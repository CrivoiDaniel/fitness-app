import React, { createContext, useContext, useRef, useState } from "react";

const NavbarTriggerContext = createContext(null);

export const NavbarTriggerProvider = ({ children }) => {
  const triggerRef = useRef(null);
  const [enabled, setEnabled] = useState(false); // activ doar pe paginile care au Hero

  return (
    <NavbarTriggerContext.Provider value={{ triggerRef, enabled, setEnabled }}>
      {children}
    </NavbarTriggerContext.Provider>
  );
};

export const useNavbarTrigger = () => {
  const ctx = useContext(NavbarTriggerContext);
  if (!ctx) throw new Error("useNavbarTrigger must be used inside NavbarTriggerProvider");
  return ctx;
};