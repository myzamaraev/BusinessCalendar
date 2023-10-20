<template>
  <button class="btn">
    <slot></slot>
  </button>
</template>

<script>
export default {
  name: "cube-button",
};
</script>

<style scoped>
button {
  display: flex;
  align-items: center;
  justify-content: center;
  padding: 0px;
  border: none;
  width: 25px;
  height: 25px;
  border-radius: 5px;
}

button:hover {
  background-color: gainsboro;
}
</style>
