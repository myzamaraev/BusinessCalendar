<template>
  <section>
    <TheNavbar></TheNavbar>
    <div class="main row">
      <div class="mx-auto col-11 col-sm-10 col-lg-10">
        <router-view></router-view>
      </div>
    </div>
  </section>
</template>

<script>
import TheNavbar from "./components/TheNavbar.vue";

export default {
  components: {
    TheNavbar,
  },
  name: "App",
  data() {
    return {};
  },
};
</script>

<style>
#app {
  font-family: Avenir, Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  color: #2c3e50;
}

html,
body {
  width: 100%;
  height: 100%;
}

.main {
  padding: 10px;
}

@media screen and (min-width: 500px) {
  .main {
    padding-left: 250px;
  }
}

.page {
  margin: 10px
}
</style>
