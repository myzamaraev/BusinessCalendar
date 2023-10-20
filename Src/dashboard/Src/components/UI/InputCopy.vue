<template>
  <div class="input-group input-group-sm" 
      @dbclick.prevent="copyToClipboard">
    <span class="input-group-text">API Identifier</span>
    <input
      type="text"
      class="form-control"
      disabled
      @click.prevent
      :value="value"
    />
    <button :class="['btn', btnType]" @click="copyToClipboard">
      <svg
        xmlns="http://www.w3.org/2000/svg"
        width="1rem"
        height="1rem"
        viewBox="0 0 24 24"
        fill="currentColor"
      >
        <path v-if="isSuccess"
          d="M20.285 2l-11.285 11.567-5.286-5.011-3.714 3.716 9 8.728 15-15.285z"
        />
        <path v-else
          d="M22 6v16h-16v-16h16zm2-2h-20v20h20v-20zm-24 17v-21h21v2h-19v19h-2z"
        />
        
      </svg>
    </button>
  </div>
</template>

<script>
export default {
  name: "input-copy",
  props: {
    name: {
      type: String,
      required: false,
      default: "",
    },
    value: {
      type: String,
      required: false,
      default: "",
    },
  },
  data() {
    return {
      isSuccess: false,
      successTimeout: null,
    };
  },
  computed: {
    btnType() {
      return this.isSuccess ? "btn-success" : "btn-light";
    }
  },
  methods: {
    copyToClipboard() {
      navigator.clipboard.writeText(this.value);
      this.animateSuccess();
    },
    animateSuccess() {
      clearTimeout(this.successTimeout);
      this.isSuccess = true;
      this.successTimeout = setTimeout(() => {this.isSuccess = false}, 1000);
    }
  },
};
</script>

<style scoped>
input {
  font-family:Consolas,Monaco,Lucida Console,Liberation Mono,DejaVu Sans Mono,Bitstream Vera Sans Mono,Courier New, monospace;
}

btn {
  transition: all 1s ease-in;
}


</style>




