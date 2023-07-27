<template>
  <section>
    <div class="header">
      <h2>
        
        <span v-if="isStateCalendar">{{ countryName(calendarKey) }} {{ calendarType.toLowerCase() }}</span>
        <span v-else>{{ calendarKey }}</span>
        calendar
      </h2>
      <YearPicker :year="selectedYear" @year-change="setYear" />
      
      <InputCopy name="API Idntifier" :value="identifier" />
    </div>
    <YearLayout v-if="!isLoading"></YearLayout>
    <b-loader v-else></b-loader>


    <SavePanel
      v-if="hasUnsavedChanges"
      @save="submitData"
      @cancel="cancelChanges"
    ></SavePanel>
  </section>
</template>

<script>
import YearLayout from "./YearLayout.vue";
import YearPicker from "../UI/YearPicker.vue";
import SavePanel from "../UI/SavePanel.vue";
import { mapGetters } from "vuex";
import InputCopy from "../UI/InputCopy.vue";

export default {
  name: "the-calendar",
  components: {
    YearLayout,
    YearPicker,
    SavePanel,
    InputCopy,
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
    ...mapGetters("calendar", ["hasUnsavedChanges", "isLoading", "identifier"]),
    ...mapGetters("localization", ["countries"]),
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
    countryName(countryCode) {
      const country = this.countries.find((x) => x.code === countryCode);
      return country ? country.name : countryCode;
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
