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
    };
  },
  getters: {
    months(state) {
      return state.months;
    },
    hasUnsavedChanges(state) {
        return state.hasUnsavedChanges;
    },
    id(state) {
      return {
        type: state.type,
        key: state.key,
        year: state.year,
      };
    },
  },
  actions,
  mutations,
};
