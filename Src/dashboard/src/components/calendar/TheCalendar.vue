<template>
  <section>
    <div class="header">
      <h2>
        <span v-if="isStateCalendar"
          >{{ countryName(calendarKey) }} {{ calendarType.toLowerCase() }}</span
        >
        <span v-else>{{ calendarKey }}</span>
        calendar
      </h2>
      <YearPicker :year="selectedYear" @year-change="setYear" />
      <router-link v-if="isSettingsView" :to="yearLayoutRoute">
        <button class="btn btn-light btn-sm">&#128197;</button>
      </router-link>
      <router-link v-else :to="settingsRoute">
        <button class="btn btn-light btn-sm">&#9881;</button>
      </router-link>
    </div>
    <router-view v-if="!isLoading"></router-view>
    <b-loader v-else></b-loader>

    <SavePanel
      v-if="hasUnsavedChanges"
      @save="submitData"
      @cancel="cancelChanges"
    ></SavePanel>
  </section>
</template>

<script>
import YearPicker from "../UI/YearPicker.vue";
import SavePanel from "../UI/SavePanel.vue";
import { mapGetters } from "vuex";

export default {
  name: "the-calendar",
  components: {
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
    year: {
      type: String,
      required: false,
    },
  },
  data() {
    return {
      selectedYear: null,
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
    ...mapGetters("localization", ["countries"]),
    settingsRoute() {
      return {
        name: "calendarSettings",
        params: {
          calendarType: this.calendarType,
          calendarKey: this.calendarKey,
        },
      };
    },
    yearLayoutRoute() {
      return {
        name: "calendar",
        params: {
          calendarType: this.calendarType,
          calendarKey: this.calendarKey,
          year: this.selectedYear,
        },
      };
    },
    isSettingsView() {
      return this.$route.name === "calendarSettings";
    },
    calendarIdentifier() {
      return `${this.calendarType}_${this.calendarKey}`;
    },
  },
  methods: {
    async setYear(year) {
      this.selectedYear = year;
      await this.loadCalendar();
    },
    async loadCalendar() {
      await this.$router.push(this.yearLayoutRoute);

      await this.$store.dispatch("calendar/loadCalendar", {
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
  async created() {
    const year = parseInt(this.year);
    this.selectedYear = year > 0 ? year : new Date().getFullYear();
    await this.loadCalendar();
  },
  watch: {
    async calendarIdentifier() {
      await this.loadCalendar();
    },
  },
  provide() {
    return {
      activeEvents: this.activeEvents,
    };
  },
};
</script>

<style scoped>
section {
  text-align: center;
  padding: 10px;
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
