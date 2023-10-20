import actions from "./actions.js";
import mutations from "./mutations.js";
import getters from "./getters.js";

export default {
  state() {
    return {
      isAuthEnabled: true,
      isAuthenticated: false,
      userName: null,
      roles: [],
    };
  },
  actions,
  mutations,
  getters,
};
