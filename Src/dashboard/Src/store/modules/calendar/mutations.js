function getMonthName(date) {
  const options = { month: "long" };
  return new Intl.DateTimeFormat("en-US", options).format(date);
}

function getDayOfWeek(date) {
  const options = { weekday: "long" };
  return new Intl.DateTimeFormat("en-US", options).format(date);
}

export default {
  setCalendar(state, data) {
    const mutatedData = data.dates.reduce((accumulator, currentValue) => {
      const date = new Date(currentValue.date);
      const monthName = getMonthName(date);
      let month = accumulator.find((x) => x.monthName === monthName);
      if (month == null) {
        month = { monthName, dates: [] };
        accumulator.push(month);
      }

      const dayOfMonth = date.getDate();
      if (dayOfMonth === 1) {
        month.firstDayOfWeek = getDayOfWeek(date);
      }

      month.dates.push({
        date,
        dayOfMonth,
        isWorkday: currentValue.isWorkday,
      });
      return accumulator;
    }, []);

    mutatedData.forEach((month) => {
      var dates = month.dates.map((x) => x.date);
      const maxDate = new Date(Math.max(...dates));
      month.lastDayOfWeek = getDayOfWeek(maxDate);
    });
``
    state.type = data.id.type;
    state.key = data.id.key;
    state.year = data.id.year;
    state.months = mutatedData;
    state.hasUnsavedChanges = false;
  },
  toggleDay(state, payload) {
    const month = state.months.find(
      (month) => month.monthName === payload.monthName
    );

    const day = month.dates.find(
      (day) => day.dayOfMonth === payload.dayOfMonth
    );
    day.isWorkday = !day.isWorkday;

    state.hasUnsavedChanges = true;
  },
  applyChanges(state) {
    state.hasUnsavedChanges = false;
  },
  setLoadingState(state, isLoading) {
    state.isLoading = isLoading;
  },
  clearCalendar(state) {
    state.type = null;
    state.key = null;
    state.year = null;
    state.months = [];
    state.hasUnsavedChanges = false;
  },
};
