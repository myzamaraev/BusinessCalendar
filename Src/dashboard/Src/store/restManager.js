import { useToast } from "vue-toastification";
import Store from "./index.js";

const toast = useToast();

async function handleFailStatus(response) {
  if (response.ok) {
    return;
  }

  switch (response.status) {
    case 401:
      Store.dispatch("clearUserSession");
      break;
    case 403:
      toast.error(
        "Not allowed to change data. Please log-in as a user with bc-manager role"
      );
      break;
    default:
      var json = await parseJson(response);
      console.error("Problem details: ", json);
      throw new Error(
        `Server returned status ${response.status} with details: ${json}`
      );
  }
}

async function parseJson(response) {
  const text = await response.text();
  if (!text || text === "") {
    return null;
  }

  try {
    const json = JSON.parse(text);
    return json;
  } catch (err) {
    throw new Error(
      "Unknown response body format. Json was expected, but received: " + text
    );
  }
}

export default {
  async get(url) {
    try {
      const response = await fetch(url);
      if (!response.ok) {
        await handleFailStatus(response);
        return { isSuccess: false };
      }

      let data = await parseJson(response);
      return {
        isSuccess: true,
        data,
      };
    } catch (error) {
      toast.error("Request failed. Please try again later..");
      return { isSuccess: false };
    }
  },
  async put(url, data) {
    try {
      const response = await fetch(url, {
        method: "PUT",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify(data),
      });

      if (!response.ok) {
        await handleFailStatus(response);
        return { isSuccess: false };
      }

      return { isSuccess: true };
    } catch (error) {
      toast.error("Request failed. Please try again later..");
      return { isSuccess: false };
    }
  },

  async post(url, data) {
    try {
      const response = await fetch(url, {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify(data),
      });

      if (!response.ok) {
        await handleFailStatus(response);
        return { isSuccess: false };
      }

      return { isSuccess: true };
    } catch (error) {
      toast.error("Request failed. Please try again later..");
      return { isSuccess: false };
    }
  },

  async delete(url) {
    try {
      const response = await fetch(url, {
        method: "DELETE",
      });

      if (!response.ok) {
        await handleFailStatus(response);
        return { isSuccess: false };
      }

      return { isSuccess: true };
    } catch (error) {
      toast.error("Request failed. Please try again later..");
      return { isSuccess: false };
    }
  },
};
