import actions from "./actions";
import mutations from "./mutations";

export default {
  namespaced: true,
  state() {
    return {
      type: null,
      key: null,
      year: null,
      months: [],
      hasUnsavedChanges: false,
      isLoading: false,
      
      localization: {
        firstDayOfWeek: "Monday",
        weekdays: [
          "Monday",
          "Tuesday",
          "Wednesday",
          "Thursday",
          "Friday",
          "Saturday",
          "Sunday",
        ],
        weekdaysShort: ["Mon", "Tue", "Wed", "Thu", "Fri", "Sat", "Sun"],
        weekdaysSymbol: ['M', "T", "W", "T", "F", "S", "S"]
      }
    };
  },
  getters: {
    months(state) {
      return state.months;
    },
    hasUnsavedChanges(state) {
        return state.hasUnsavedChanges;
    },
    isLoading(state) {
      return state.isLoading;
    },
    id(state) {
      return {
        type: state.type,
        key: state.key,
        year: state.year,
      };
    },
    identifier(state)
    {
      return `${state.type}_${state.key}`
    },
    localization(state) {
      return state.localization;
    }
  },
  actions,
  mutations,
};
