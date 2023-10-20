import { useToast } from "vue-toastification";
import restManager from "../../restManager";

const toast = useToast();

function getDateOnlyString(date) {
  return date.toJSON().slice(0, 10);
}

export default {
  async loadCalendar(context, payload) {
    context.commit("setLoadingState", true);

    var url = new URL("/bff/v1/Management/GetCalendar", window.location.origin);
    url.searchParams.append("type", payload.type);
    url.searchParams.append("key", payload.key);
    url.searchParams.append("year", payload.year);

    const result = await restManager.get(url);
    if (result.isSuccess) {
      context.commit("setCalendar", result.data);
    }

    context.commit("setLoadingState", false);
  },
  async submitData(context) {
    context.commit("setLoadingState", true);

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

    const result = await restManager.put(
      "/bff/v1/Management/SaveCalendar",
      data
    );
    if (result.isSuccess) {
      context.commit("applyChanges");
      toast.success("Saved!");
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
