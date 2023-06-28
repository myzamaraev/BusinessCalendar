<template>
  <h2>
    {{ calendarKey }}
    <span v-if="isStateCalendar">{{ calendarType.toLowerCase() }}</span>
    calendar
  </h2>
  <year-list
    :calendarType="calendarType"
    :calendarKey="calendarKey"
    :selectedYear="selectedYear"
    @year-change="setYear"
  ></year-list>
  <form @submit.prevent="submitData">
    <div
      @mousedown="onMouseDown"
      @mouseup="onMouseUp"
      @mouseleave="onMouseLeave"
    >
      <div class="flex">
        <calendar-month
          v-for="month in months"
          v-bind="month"
          :key="month.monthName"
        ></calendar-month>
      </div>
    </div>
    <button>Save</button>
  </form>
</template>

<script>
import CalendarMonth from "./CalendarMonth.vue";
import YearList from "./YearList.vue";

export default {
  name: "the-calendar",
  components: {
    CalendarMonth,
    YearList,
  },
  props: {
    calendarType: {
      type: String,
      required: true,
    },
    calendarKey: {
      type: String,
      required: true,
    },
    initialYear: {
      type: Number,
      required: false,
      default: new Date().getFullYear(),
    },
  },
  data() {
    return {
      selectedYear: this.initialYear,
      activeEvents: {
        mouseDown: false,
      },
      months: [],
      localization: {
        firstDayOfWeek: "Monday",
        weekdays: [
          "Monday",
          "Tuesday",
          "Wednesday",
          "Thursday",
          "Friday",
          "Saturday",
          "Sunday",
        ],
        weekdaysShort: ["Mon", "Tue", "Wed", "Thu", "Fri", "Sat", "Sun"],
      },
    };
  },
  computed: {
    isStateCalendar() {
      return this.calendarType === "State";
    },
  },
  methods: {
    onMouseDown() {
      this.activeEvents.mouseDown = true;
    },
    onMouseUp() {
      this.activeEvents.mouseDown = false;
    },
    onMouseLeave() {
      this.activeEvents.mouseDown = false;
    },
    getMonthName(date) {
      const options = { month: "long" };
      return new Intl.DateTimeFormat("en-US", options).format(date);
    },
    getFirstWeekday(date) {
      const options = { weekday: "long" };
      return new Intl.DateTimeFormat("en-US", options).format(date);
    },
    getDateOnlyString(date) {
      return date.toJSON().slice(0, 10);
    },
    mutateCalendar(data) {
      return data.dates.reduce((accumulator, currentValue) => {
        const date = new Date(currentValue.date);
        const monthName = this.getMonthName(date);
        let month = accumulator.find((x) => x.monthName === monthName);
        if (month == null) {
          month = { monthName, dates: [] };
          accumulator.push(month);
        }

        const dayOfMonth = date.getDate();
        if (dayOfMonth === 1) {
          month.firstWeekday = this.getFirstWeekday(date);
        }

        month.dates.push({
          date,
          dayOfMonth,
          isWorkday: currentValue.isWorkday,
        });
        return accumulator;
      }, []);
    },
    setYear(year) {
      this.selectedYear = year;
      this.loadCalendar();
    },
    submitData() {
      const dates = [];

      this.months.forEach((month) =>
        dates.push.apply(
          dates,
          month.dates.map((x) => ({
            date: this.getDateOnlyString(x.date),
            isWorkday: x.isWorkday,
          }))
        )
      );

      const data = {
        type: this.calendarType,
        key: this.calendarKey,
        year: this.selectedYear,
        dates: dates,
      };

      fetch("/Management/SaveCalendar", {
        method: "PUT",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify(data),
      });
    },
    loadCalendar() {
      var url = new URL("/Management/GetCalendar", window.location.origin);
      url.searchParams.append("type", this.calendarType);
      url.searchParams.append("key", this.calendarKey);
      url.searchParams.append("year", this.selectedYear);

      fetch(url)
        .then((response) => {
          if (!response.ok) {
            throw new Error("request failed! Status: " + response.status);
          }

          return response.json();
        })
        .then((data) => {
          this.months = this.mutateCalendar(data);
        })
        .catch((error) => {
          console.log(error);
        });
    },
  },
  mounted() {
    this.loadCalendar();
  },
  watch: {
    calendarType() {
      this.loadCalendar();
    },
    calendarKey() {
      this.loadCalendar();
    },
  },
  provide() {
    return {
      activeEvents: this.activeEvents,
      localization: this.localization,
    };
  },
};
</script>

<style scoped>
@media screen and (min-width: 1000px) {
  form {
    max-width: 1000px;
  }
}

form {
  display: block;
  margin: auto;
}

.flex {
  display: flex;
  flex-direction: row;
  flex-wrap: wrap;
  justify-content: center;
}
</style>
