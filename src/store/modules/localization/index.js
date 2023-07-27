import countries from "./countries.json";
import timezones from "./timezones.js";

function determineUserCountryCode() {
  const timezone = Intl.DateTimeFormat().resolvedOptions().timeZone;

  if (timezone === "" || !timezone) {
    return null;
  }

  const countryCode = timezones[timezone].c[0];
  return countryCode;
}

export default {
  namespaced: true,
  state() {
    return {
      countries,
      timezones,
      userCountryCode: determineUserCountryCode(),
    };
  },
  getters: {
    countries(state) {
      return state.countries;
    },
    userCountryCode(state) {
      return state.userCountryCode;
    },
  },
};
