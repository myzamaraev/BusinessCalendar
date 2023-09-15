import { useToast } from "vue-toastification";
const toast = useToast();

export default {
  async tryFetchUserInfo(context) {
    console.log("Fetching user info");

    var url = new URL("bff/v1/Account/UserInfo", window.location.origin);
    try {
      const response = await fetch(url);
      if (!response.ok) {
        if (response.status == 401) {
          context.dispatch("clearUserSession");
          return;
        } else {
          throw new Error("request failed! Status: " + response.status);
        }
      }

      const authInfo = await response.json();
      console.log("Retrieved auth information: ", authInfo);

      if (!authInfo.isAuthEnabled) {
        context.commit("disableAuth");
        return;
      }

      if (authInfo.userInfo) {
        context.commit("setLoggedInUser", authInfo.userInfo);
      }
    } catch (error) {
      toast.error("Failed to load user");
      console.log(error);
    }
  },
  clearUserSession(context) {
    context.commit("setLoggedInUser", null);
  },
  login() {
    //Redirect to server endpoint required for oidc code flow
    window.location.replace(
      "/bff/v1/Account/Login?redirectUri=" + window.location
    );
  },
  logout() {
    //Redirect to server endpoint required for oidc code flow
    window.location.replace("/bff/v1/Account/Logout");
  },
};
