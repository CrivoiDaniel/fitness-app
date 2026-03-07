import i18n from "i18next";
import { initReactI18next } from "react-i18next";

import ro from "./locales/ro.json";
import en from "./locales/en.json";
import ru from "./locales/ru.json";

const STORAGE_KEY = "lang";
const saved = localStorage.getItem(STORAGE_KEY);
const defaultLng = saved || "ro";

i18n.use(initReactI18next).init({
  debug: false,
  resources: {
    ro: { translation: ro },
    en: { translation: en },
    ru: { translation: ru },
  },
  lng: defaultLng,
  fallbackLng: "ro",
  interpolation: { escapeValue: false },
  react: {
    useSuspense: false
  }
});

export function setLanguage(lng) {
  i18n.changeLanguage(lng);
  localStorage.setItem(STORAGE_KEY, lng);
}

export default i18n;