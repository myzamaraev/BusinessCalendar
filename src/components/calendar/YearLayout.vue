<template>
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
    </form>
</template>

<script>
import CalendarMonth from "./CalendarMonth.vue";
import { mapGetters } from "vuex";

export default {
  name: "year-layout",
  components: {
    CalendarMonth,
  },
  data() {
    return {
      activeEvents: {
        mouseDown: false,
      },
    };
  },
  computed: {
    ...mapGetters("calendar", ["months"])
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
    submitData() {
      this.$store.dispatch("calendar/submitData");
    },
    cancelChanges() {
      this.$store.dispatch("calendar/cancelChanges");
    }
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
  padding: 0px;
  padding-bottom: 140px;
}

.flex {
  display: flex;
  flex-direction: row;
  flex-wrap: wrap;
  justify-content: center;
}
</style>