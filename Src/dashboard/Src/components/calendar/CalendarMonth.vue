<template>
  <div class="calendar-month">
    <h5>{{ monthName }}</h5>
    <div class="flex-month">
      <div
        v-for="weekday in this.daysOfWeek.symbol"
        :key="weekday"
        class="days-header"
      >
       <span>{{ weekday }}</span>
      </div>

      <div v-for="stub in stubsBefore" :key="-stub"></div>

      <div v-for="date in dates" :key="date.dayOfMonth">
        <calendar-date
        :dayOfMonth="date.dayOfMonth"
        :isWorkday="date.isWorkday"
        @state-change="toggleDate"
      ></calendar-date>
      </div>

      <div v-for="stub in stubsAfter" :key="-stub"></div>
    </div>
  </div>
</template>

<script>
import { mapGetters } from "vuex";
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
    firstDayOfWeek: {
      type: String,
      required: true,
    },
    lastDayOfWeek: {
      type: String,
      required: true
    }
  },
  data() {
    return {};
  },
  computed: {
    stubsBefore() {
      return this.daysOfWeek.long.indexOf(this.firstDayOfWeek);
    },
    stubsAfter() {
      return 7 - this.daysOfWeek.long.indexOf(this.lastDayOfWeek) - 1;
    },
    ...mapGetters("localization", ["daysOfWeek"])
  },
  methods: {
    toggleDate(id) {
      this.$store.dispatch("calendar/toggleDay", {
        monthName: this.monthName,
        dayOfMonth: id
      })
    },
  }
};
</script>

<style scoped>

.calendar-month {
  margin: 10px;
}

.flex-month {
  display: flex;
  flex-direction: row;
  flex-grow: 0;
  flex-shrink: 0;
  flex-wrap: wrap;
  justify-content: space-between;
  width: 200px;
}

.flex-month>* {
  flex: 0 0 13%;
  text-align: center;
  padding: 0;
  height: 13%;
  margin: 2px 0 2px 0;
  /* border: 1px solid black; */
}

.days-header {
  overflow: hidden;    
  font-size: 0.8rem;
}


</style>
