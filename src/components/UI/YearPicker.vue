<template>
  <div class="year-input">
    <div class="input-group input-group-sm">
      <button @click="prev" class="btn btn-outline-secondary" type="button">
        &#60;
      </button>
      <span class="input-group-text">Year</span>
      <input
        type="number"
        :value="selectedYear"
        min="1"
        max="9999"
        class="form-control text-center"
        maxlength="4"
        size="4"
        @wheel.prevent
        @touchmove.prevent
        @scroll.prevent
        @focus="onFocus"
        @input.prevent="onInput"
        @change="onChange"
        @keyup.enter="onEnter"
        @keydown.left.prevent="prev"
        @keydown.right.prevent="next"
        @keydown.up.prevent="next"
        @keydown.down.prevent="prev"
      />
      <button @click="next" class="btn btn-outline-secondary" type="button">
        &#62;
      </button>
    </div>
  </div>
</template>

<script>
export default {
  name: "year-picker",
  props: {
    year: {
      type: Number,
      required: true,
    },
  },
  data() {
    return {
      selectedYear: this.year,
      changeTimeout: null,
    };
  },
  emits: ["year-change"],
  methods: {
    changeYear() {
      console.log("Year change: " + this.selectedYear);

      this.$emit("year-change", this.selectedYear);
    },
    delayedYearChange() {
      clearTimeout(this.changeTimeout);
      this.changeTimeout = setTimeout(this.changeYear, 1000);
    },
    prev() {
      this.selectedYear--;
      this.delayedYearChange();
    },
    next() {
      this.selectedYear++;
      this.delayedYearChange();
    },
    onChange(event) {
      if (!event.target.value) {
        this.$forceUpdate();
        return;
      }
      this.selectedYear = Number(event.target.value);
      this.changeYear();
    },
    onInput(event) {
      var enteredValue = event.target.value;
      this.value = event.target.value = enteredValue.substring(0, 4);
    },
    onEnter(event) {
      event.target.blur();
    },
  },
};
</script>

<style scoped>
/* Chrome, Safari, Edge, Opera */
input::-webkit-outer-spin-button,
input::-webkit-inner-spin-button {
  -webkit-appearance: none;
  margin: 0;
}

/* Firefox */
input[type="number"] {
  -moz-appearance: textfield;
  font-weight: 1000;
}

.form-control {
  max-width: 4rem;
}

.form-control:focus {
  /* border-color: rgba(126, 239, 104, 0.8); */
  box-shadow: none;
  outline: 0 none;
}

.year-input .btn {
  visibility: hidden;
}

.year-input:hover .btn,
.year-input:focus-within .btn {
  visibility: visible;
}
</style>
