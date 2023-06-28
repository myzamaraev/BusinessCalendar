<template>
  <div
    :class="['calendar-date', { holiday: isHoliday, hidden: dayOfMonth < 0 }]"
    @mousedown="toggleDay"
    @[isMultiToggable&&`mouseenter`]="toggleDay"
  >
    <span>{{ dayOfMonth }}</span>
  </div>
</template>

<script>
export default {
  name: "calendar-date",
  props: {
    dayOfMonth: {
      type: Number,
      required: true,
    },
    isWorkday: {
      type: Boolean,
      required: false,
      default: true,
    },
  },
  emits: ["state-change"],
  data() {
    return {
      isWorkdayInternal: this.isWorkday,
    };
  },
  computed: {
    isHoliday() {
      return !this.isWorkday;
    },
    isMultiToggable() {
      return (
        this.activeEvents.mouseDown &&
        this.activeEvents.multiToggleSource === this.isWorkday
      );
    },
  },
  methods: {
    toggleDay() {
      this.activeEvents.multiToggleSource = this.isWorkday;

      this.isWorkdayInternal = !this.isWorkdayInternal;
      this.$emit("state-change", this.dayOfMonth);
    },
  },
  inject: ["activeEvents"],
};
</script>

<style scoped>
.calendar-date {
  display: block;
  margin: 3px;
  text-align: center;
  border-radius: 50%;
  border: 1px solid #000;
  font: 12px Arial, sans-serif;
  padding: 2px 0 2px 0;
  -webkit-user-select: none; /* Safari */
  -ms-user-select: none; /* IE 10 and IE 11 */
  user-select: none; /* Standard syntax */
}

.calendar-date.hidden {
  visibility: hidden;
}

.calendar-date:hover {
  background-color: #e6e6e6;
}

.calendar-date.holiday {
  background-color: red;
}

.calendar-date.holiday:hover {
  background-color: #ff6666;
}

.custom-hours {
  border-radius: 50%;
  width: 20px;
  height: 20px;
  background-color: blue;
  margin-top: -10px;
}
</style>
