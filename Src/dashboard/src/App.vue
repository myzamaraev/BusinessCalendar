<template>
  <section>
    <TheNavbar></TheNavbar>
    <div class="main mx-auto col-11 col-sm-10 col-lg-10">
      <div v-if="isAuthenticated">
        <router-view></router-view>
      </div>
      <div v-else class="content-page-center">
        <h5>
          Please <b class="text-danger">Log In</b> to start working with
          calendars
        </h5>
      </div>
    </div>
  </section>
</template>

<script>
import TheNavbar from "./components/nav/TheNavbar.vue";

export default {
  components: {
    TheNavbar,
  },
  name: "App",
  data() {
    return {};
  },
  computed: {
    isAuthenticated() {
      return this.$store.getters.isAuthenticated;
    },
  },
};
</script>

<style>
:root {
  --blue: #007fd4;
}

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

.content-page-center {
  display: flex;
  align-items: center;
  justify-content: center;
  height: 70vh;
  text-align: center;
}

@media screen and (min-width: 500px) {
  .main {
    padding-left: 250px;
  }
}
</style>
