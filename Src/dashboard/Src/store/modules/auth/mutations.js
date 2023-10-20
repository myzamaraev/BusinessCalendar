export default {
  setLoggedInUser(state, payload) {
    if (!payload) {
      state.userName = null;
      state.roles = null;
      state.isAuthenticated = false;
      return;
    }

    state.userName = payload.userName;
    state.roles = payload.roles;
    state.isAuthenticated = true;

    console.log("Logged in: ", payload);
  },

  disableAuth(state) {
    state.isAuthEnabled = false;
    state.isAuthenticated = false;
    state.userName = null;
    state.roles = [];

    console.log("Auth is turned off on the server");
  },
};
