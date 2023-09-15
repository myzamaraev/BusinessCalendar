export default {
  isAuthEnabled(state) {
    return state.isAuthEnabled;
  },
  userName(state) {
    return state.userName;
  },
  isAuthenticated(state) {
    return state.isAuthenticated || !state.isAuthEnabled;
  },
  isManager(state) {
    return state.roles.indexOf("bc-manager") > 0;
  },
  roles(state) {
    return state.roles;
  },
};
