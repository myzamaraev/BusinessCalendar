import { createStore } from "vuex";
import calendarModule from "./modules/calendar/index.js";
import navIdentifiersModule from "./modules/navIdentifiers/index.js";
import localizationModule from "./modules/localization/index.js";
import authMOdule from "./modules/auth/index.js";

const store = createStore({
  modules: {
    calendar: calendarModule,
    navIdentifiers: navIdentifiersModule,
    localization: localizationModule,
    auth: authMOdule,
  },
});

await store.dispatch("tryFetchUserInfo");

export default store;
