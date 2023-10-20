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
  },
  actions,
  mutations,
};
