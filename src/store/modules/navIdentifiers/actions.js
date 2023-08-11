import { useToast } from "vue-toastification";

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
    var url = new URL("/CalendarIdentifier/List", window.location.origin);
    url.searchParams.append("page", context.state.page);
    url.searchParams.append("pageSize", context.state.pageSize);

    try {
      const response = await fetch(url);
      if (!response.ok) {
        throw new Error("request failed! Status: " + response.status);
      }

      const data = await response.json();

      context.commit("appendData", data);

      if (data.length < context.state.pageSize) {
        context.commit("endOfList");
      }
    } catch (error) {
      toast.error("Failed to load calendar identifiers..");
      console.log(error);
    }
  },
  async create(context, payload) {
    const url = new URL("/CalendarIdentifier", window.location.origin);

    try {
      const response = await fetch(url, {
        method: "Post",
        headers: {
          "Content-type": "application/json",
        },
        body: JSON.stringify({ type: payload.type, key: payload.key }),
      });

      if (!response.ok) {
        throw new Error("request failed! Status: " + response.status);
      }
      
      context.dispatch("init");
      toast.success("New calendar created!");
      return true;

    } catch (error) {
      toast.error("Failed to save new calendar identifier..");
      console.log(error);
      return false;
    }
  },
  async delete(context, payload)
  {
    const url = new URL("/CalendarIdentifier/"+payload.identifier, window.location.origin);
    
    try {
      const response = await fetch(url, {
        method: "DELETE",
      });

      if (!response.ok) {
        throw new Error("request failed! Status: " + response.status);
      }

      toast.success("Calendar identifier deleted!");
      context.dispatch("init");
      return true;

    } catch (error) {
      toast.error("Failed to delete calendar identifier..");
      console.log(error);
      return false;
    }
  }
};
