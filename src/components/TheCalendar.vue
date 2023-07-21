<template>
  <section>
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
    <form @submit.prevent>
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
      <save-panel v-if="hasUnsavedChanges" @save="submitData" @cancel="cancelChanges"></save-panel>
    </form>
  </section>
</template>

<script>
import CalendarMonth from "./CalendarMonth.vue";
import YearList from "./YearList.vue";
import SavePanel from "./UI/SavePanel.vue";
import { mapGetters } from "vuex";

export default {
  name: "the-calendar",
  components: {
    CalendarMonth,
    YearList,
    SavePanel,
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
        weekdaysSymbol: ['M', "T", "W", "T", "F", "S", "S"]
      },
    };
  },
  computed: {
    isStateCalendar() {
      return this.calendarType === "State";
    },
    ...mapGetters("calendar", ["months", "hasUnsavedChanges"])
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
    setYear(year) {
      this.selectedYear = year;
      this.loadCalendar();
    },
    loadCalendar() {
      this.$store.dispatch("calendar/loadCalendar", {
        type: this.calendarType,
        key: this.calendarKey,
        year: this.selectedYear,
      });
    },
    submitData() {
      this.$store.dispatch("calendar/submitData");
    },
    cancelChanges() {
      this.$store.dispatch("calendar/cancelChanges");
    }
  },
  created () {
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

section {
  padding: 40px;
  padding-bottom: 100px;
}
</style>
