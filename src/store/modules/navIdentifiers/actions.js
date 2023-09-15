import { useToast } from "vue-toastification";
import restManager from "@/store/restManager";

const toast = useToast();

export default {
  async init(context) {
    context.commit("reset");
    context.dispatch("loadIdentifiers");
  },

  async loadMore(context) {
    context.commit("nextPage");
    context.dispatch("loadIdentifiers");
  },
  async loadIdentifiers(context) {
    var url = new URL(
      "/bff/v1/CalendarIdentifier/List",
      window.location.origin
    );
    url.searchParams.append("page", context.state.page);
    url.searchParams.append("pageSize", context.state.pageSize);

    const result = await restManager.get(url);
    if (result.isSuccess) {
      context.commit("appendData", result.data);
      if (result.data.length < context.state.pageSize) {
        context.commit("endOfList");
      }
    }
  },
  async create(context, payload) {
    const url = new URL("/bff/v1/CalendarIdentifier", window.location.origin);

    const result = await restManager.post(url, {
      type: payload.type,
      key: payload.key,
    });
    if (result.isSuccess) {
      toast.success("New calendar created!");
      context.dispatch("init");
    }

    return result.isSuccess;
  },
  async delete(context, payload) {
    const url = new URL(
      "/bff/v1/CalendarIdentifier/" + payload.identifier,
      window.location.origin
    );

    var result = await restManager.delete(url);
    if (result.isSuccess) {
      toast.success("Calendar identifier deleted!");
      context.dispatch("init");
    }

    return result.isSuccess;
  },
};
