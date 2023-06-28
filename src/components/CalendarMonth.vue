<template>
  <div class="calendar-month">
    <h3>{{ monthName }}</h3>
    <div class="days-header flex">
      <span
        v-for="weekday in this.localization.weekdaysShort"
        :key="weekday"
        class="flex-item"
      >
        {{ weekday }}
      </span>
    </div>
    <div class="flex">
      <calendar-date
        v-for="stub in numberOfStubs"
        :key="-stub"
        :dayOfMonth="-stub"
        class="flex-item"
      ></calendar-date>
      <calendar-date
        v-for="date in dates"
        :key="date.dayOfMonth"
        :dayOfMonth="date.dayOfMonth"
        :isWorkday="date.isWorkday"
        @state-change="toggleDate"
        class="flex-item"
      ></calendar-date>
    </div>
  </div>
</template>

<script>
import CalendarDate from "./CalendarDate.vue";

export default {
  name: "calendar-month",
  components: {
    CalendarDate,
  },
  props: {
    monthName: {
      type: String,
      required: true,
    },
    dates: {
      type: Object,
      required: true,
    },
    firstWeekday: {
      type: String,
      required: true,
    },
  },
  data() {
    return {};
  },
  computed: {
    numberOfStubs() {
      return this.localization.weekdays.indexOf(this.firstWeekday);
    },
  },
  methods: {
    toggleDate(id) {
      const date = this.dates.find((x) => x.dayOfMonth == id);
      date.isWorkday = !date.isWorkday;
    },
  },
  inject: ["localization"],
};
</script>

<style scoped>
.flex {
  display: flex;
  flex-direction: row;
  flex-wrap: wrap;
  min-width: 200px;
  width: 200px;
}

.flex-item {
  flex-basis: 10%;
  justify-content: space-between;
  margin: 3px;
}

.calendar-month {
  width: 200px;
  display: block;
  margin: 10px;
}

.days-header {
  font-size: 0.7em;
  text-align: center;
}
</style>
