function getMonthName(date) {
  const options = { month: "long" };
  return new Intl.DateTimeFormat("en-US", options).format(date);
}

function getWeekday(date) {
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
        month.firstWeekday = getWeekday(date);
      }

      month.dates.push({
        date,
        dayOfMonth,
        isWorkday: currentValue.isWorkday,
      });
      return accumulator;
    }, []);

    mutatedData.forEach(month =>{
        var dates = month.dates.map(x => x.date);
        const maxDate = new Date(Math.max(...dates));
        month.lastWeekday = getWeekday(maxDate);
    });

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

    const day = month.dates.find((day) => day.dayOfMonth === payload.dayOfMonth);
    day.isWorkday = !day.isWorkday;

    state.hasUnsavedChanges = true;
  },
  applyChanges(state)
  {
    state.hasUnsavedChanges = false;
  }
};
