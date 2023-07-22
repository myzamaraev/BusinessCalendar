<template>
  <section>
    <div class="header">
      <h2>
        {{ calendarKey }}
        <span v-if="isStateCalendar">{{ calendarType.toLowerCase() }}</span>
        calendar
      </h2>
      <year-picker :year="selectedYear" @year-change="setYear"></year-picker>
    </div>
    <year-layout v-if="!isLoading"></year-layout>
    <b-loader v-else></b-loader>
    <save-panel
      v-if="hasUnsavedChanges"
      @save="submitData"
      @cancel="cancelChanges"
    ></save-panel>
  </section>
</template>

<script>
import YearLayout from "./YearLayout.vue";
import YearPicker from "./YearPicker.vue";
import SavePanel from "../UI/SavePanel.vue";
import { mapGetters } from "vuex";

export default {
  name: "the-calendar",
  components: {
    YearLayout,
    YearPicker,
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
    };
  },
  computed: {
    isStateCalendar() {
      return this.calendarType === "State";
    },
    ...mapGetters("calendar", ["hasUnsavedChanges", "isLoading"]),
  },
  methods: {
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
    },
  },
  created() {
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
section {
  text-align: center;
}

.header {
  display: flex;
  flex-wrap: wrap;
  justify-content: center;
  align-items: center;
}

.header > * {
  margin: 10px;
}
</style>
