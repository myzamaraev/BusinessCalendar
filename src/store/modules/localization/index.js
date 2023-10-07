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

function computeDaysOfWeek() {
  const daysOfWeek = {
    long: [
      "Monday",
      "Tuesday",
      "Wednesday",
      "Thursday",
      "Friday",
      "Saturday",
      "Sunday",
    ],
    short: ["Mon", "Tue", "Wed", "Thu", "Fri", "Sat", "Sun"],
    symbol: ['M', "T", "W", "T", "F", "S", "S"]
  }

  //todo: for future localizations configuration from server should be requested
  //below just an example of changing first day of week to Sunday
  // if (determineUserCountryCode() === "US")
  // {
  //   daysOfWeek.long = [daysOfWeek.long.pop(6), ...daysOfWeek.long];
  //   daysOfWeek.short = [daysOfWeek.short.pop(6), ...daysOfWeek.short];
  //   daysOfWeek.symbol = [daysOfWeek.symbol.pop(6), ...daysOfWeek.symbol];
  // }

  return daysOfWeek;
}

export default {
  namespaced: true,
  state() {
    return {
      countries,
      timezones,
      userCountryCode: determineUserCountryCode(),
      daysOfWeek: computeDaysOfWeek(),
    };
  },
  getters: {
    countries(state) {
      return state.countries;
    },
    userCountryCode(state) {
      return state.userCountryCode;
    },
    
    daysOfWeek(state) {
      return state.daysOfWeek;
    }
  },
};
