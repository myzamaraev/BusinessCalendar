<template>
  <section>
    <router-view></router-view>
  </section>
</template>

<script>
export default {
  components: {},
  name: "App",
  data() {
    return {};
  },
};
</script>

<style>
.years-list {
  width: 10%;
  float: left;
}

.the-calendar {
  width: 90%;
  float: right;
}

#app {
  font-family: Avenir, Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  text-align: center;
  color: #2c3e50;
  margin-top: 60px;
}

html,
body {
  width: 100%;
  height: 100%;
}
</style>
