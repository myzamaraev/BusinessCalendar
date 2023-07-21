import { createStore } from "vuex"
import calendarModule from "./modules/calendar/index.js"

const store = createStore({
    modules: {
        calendar: calendarModule
    }
});

export default store;