<template>
  <div
    :class="['calendar-date', { holiday: isHoliday, hidden: dayOfMonth < 0 }]"
    @mousedown="toggleDay"
    @[isMultiToggable&&`mouseenter`]="toggleDay"
  >
    {{ dayOfMonth }}
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
  width: 24px;
  height: 24px;
  line-height: 24px;
  margin: auto;
  border-radius: 50%;
  background-color: rgb(239, 239, 239);
  position: relative;
  -webkit-transition: background-color 0.1s linear;
  transition: background-color 0.1s linear;
  cursor: pointer;
  -webkit-user-select: none;
  -ms-user-select: none;
  user-select: none;
  font-size: 10px;
  font-weight: 520;
}

.calendar-date.hidden {
  visibility: hidden;
}

.calendar-date:hover {
  background-color: #d8d8d8;
}

.calendar-date.holiday {
  background-color: red;
  color: white;
}

.calendar-date.holiday:hover {
  background-color: #ff5959;
}
</style>
