
function getDateOnlyString(date) {
    return date.toJSON().slice(0, 10);
}

export default {
  loadCalendar(context, payload) {
    var url = new URL("/Management/GetCalendar", window.location.origin);
    url.searchParams.append("type", payload.type);
    url.searchParams.append("key", payload.key);
    url.searchParams.append("year", payload.year);

    fetch(url)
      .then((response) => {
        if (!response.ok) {
          throw new Error("request failed! Status: " + response.status);
        }

        return response.json();
      })
      .then((data) => {
        context.commit("setCalendar", data);
      })
      .catch((error) => {
        console.log(error);
      });
  },
  async submitData(context) {
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

    const response = await fetch("/Management/SaveCalendar", {
      method: "PUT",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify(data),
    });

    if (!response.ok) {
        console.error("An error occured while submitting data to the server, try again later..");
        return;
    }

    context.commit("applyChanges");
  },
  cancelChanges(context) {
    context.dispatch("loadCalendar", {
        type: context.state.type,
        key: context.state.key,
        year: context.state.year
    });
  },

  toggleDay(context, payload) {
    context.commit("toggleDay", payload);
  }
};
