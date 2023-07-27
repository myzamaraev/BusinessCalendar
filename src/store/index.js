import { createStore } from "vuex"
import calendarModule from "./modules/calendar/index.js"
import navIdentifiersModule from "./modules/navIdentifiers/index.js"
import localizationModule from "./modules/localization/index.js"

const store = createStore({
    modules: {
        calendar: calendarModule,
        navIdentifiers: navIdentifiersModule,
        localization: localizationModule,
    }
});

export default store;