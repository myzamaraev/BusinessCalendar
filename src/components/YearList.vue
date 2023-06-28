<template>
  <div>
    <button @click="prevFive">&#8592;</button>
    <button
      v-for="year in availableYears"
      v-bind:key="year"
      :id="year"
      @click="setYear"
      :class="{ selected: isYearSelected(year) }"
    >
      {{ year }}
    </button>
    <button @click="nextFive">&#8594;</button>
  </div>
</template>

<script>
export default {
  name: "year-list",
  props: {
    calendarType: {
      type: String,
      required: true,
    },
    calendarKey: {
      type: String,
      required: true,
    },
    selectedYear: {
      type: Number,
      required: true,
    },
  },
  emits: ["year-change"],
  data() {
    return {
      availableYears: [],
    };
  },
  methods: {
    isYearSelected(year) {
      return year === this.selectedYear;
    },
    setYear(event) {
      this.$emit("year-change", Number(event.target.id));
    },
    prevFive() {
      let minYear = Math.min(...this.availableYears);
      this.availableYears = this.yearsBetween(minYear - 5, minYear - 1);
    },
    nextFive() {
      let maxYear = Math.max(...this.availableYears);
      this.availableYears = this.yearsBetween(maxYear + 1, maxYear + 5);
    },
    yearsBetween(from, till) {
      let years = [];
      for (let year = from; year <= till; year++) {
        years.push(year);
      }
      return years;
    },
  },
  mounted() {
    this.availableYears = this.yearsBetween(
      this.selectedYear - 2,
      this.selectedYear + 2
    );
  },
};
</script>

<style scoped>
button {
  border-width: 1px;
  border-radius: 0;
  background-color: rgb(228, 228, 228);
  width: 80px;
  -webkit-appearance: none;
}

.selected {
  background-color: aquamarine;
}

ul {
  list-style-type: none;
}

li {
  display: inline;
}

div {
  display: block;
}
</style>
