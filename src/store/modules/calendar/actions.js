import { useToast } from "vue-toastification";

const toast = useToast();

function getDateOnlyString(date) {
  return date.toJSON().slice(0, 10);
}

export default {
  async loadCalendar(context, payload) {
    context.commit("setLoadingState", true);

    var url = new URL("/api/v1/Management/GetCalendar", window.location.origin);
    url.searchParams.append("type", payload.type);
    url.searchParams.append("key", payload.key);
    url.searchParams.append("year", payload.year);

    try {
      const response = await fetch(url);
      if (!response.ok) {
        throw new Error("request failed! Status: " + response.status);
      }
      const data = await response.json();
      context.commit("setCalendar", data);

    } catch (error) {
      toast.error("Failed to load calendar. Try again later..")
      console.error(error);
    }

    context.commit("setLoadingState", false);
  },
  async submitData(context) {
    context.commit("setLoadingState", true);

    try {
      const dates = [];
      context.state.months.forEach((month) =>
        dates.push.apply(
          dates,
          month.dates.map((x) => ({
            date: getDateOnlyString(x.date),
            isWorkday: x.isWorkday,
          }))
        )
      );

      const data = {
        type: context.state.type,
        key: context.state.key,
        year: context.state.year,
        dates: dates,
      };

      const response = await fetch("/api/v1/Management/SaveCalendar", {
        method: "PUT",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify(data),
      });

      if (!response.ok) {
        throw new Error("request failed! Status: " + response.status);
      }

      context.commit("applyChanges");
      toast.success("Saved!");
    } catch (error) {
      toast.error("Failed to save. Try again later...");
      console.error(error);
    }

    context.commit("setLoadingState", false);
  },
  cancelChanges(context) {
    context.dispatch("loadCalendar", {
      type: context.state.type,
      key: context.state.key,
      year: context.state.year,
    });
    toast.info("changes cancelled!");
  },
  toggleDay(context, payload) {
    context.commit("toggleDay", payload);
  },
};
