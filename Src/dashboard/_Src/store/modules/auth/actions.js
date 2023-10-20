import restManager from "@/store/restManager";

export default {
  async tryFetchUserInfo(context) {
    console.log("Fetching user info");

    var url = new URL("bff/v1/Account/UserInfo", window.location.origin);
    var result = await restManager.get(url);
    if (result.isSuccess && result.data) {
      const authInfo = result.data;
      console.log("Retrieved auth information: ", authInfo);

      if (!authInfo.isAuthEnabled) {
        context.commit("disableAuth");
        return;
      }

      if (authInfo.userInfo) {
        context.commit("setLoggedInUser", authInfo.userInfo);
      }
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
  async appLogout() {
    //Redirect to server endpoint required for oidc code flow
    window.location.replace("/bff/v1/Account/LogOut");
  },
  singleLogout() {
    //Redirect to server endpoint required for oidc code flow
    window.location.replace("/bff/v1/Account/SingleLogOut");
  },
};
