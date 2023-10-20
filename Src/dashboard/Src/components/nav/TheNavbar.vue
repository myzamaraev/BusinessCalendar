<template>
  <nav>
    <router-link to="/" class="logo-link">
      <div class="logo">
        <img src="../../assets/logo.png" />
        <h3>&nbsp;Calendar</h3>
      </div></router-link
    >

    <UserInfo v-if="isAuthEnabled"></UserInfo>
    <NavIdentifiersSection v-if="isAuthenticated"></NavIdentifiersSection>
  </nav>
</template>

<script>
import NavIdentifiersSection from "./NavIdentifiersSection.vue";
import UserInfo from "./UserInfo.vue";

export default {
  name: "the-navbar",
  components: {
    NavIdentifiersSection,
    UserInfo,
  },
  computed: {
    isAuthenticated() {
      return this.$store.getters.isAuthenticated;
    },
    isAuthEnabled() {
      return this.$store.getters.isAuthEnabled;
    },
  },
};
</script>

<style scoped>
nav {
  color: #000 !important;
  background-color: #f1f1f1 !important;
}

@media screen and (min-width: 500px) {
  nav {
    width: 250px;
    height: 100%;
    position: fixed;
    max-width: 1000px;
    overflow: hidden;
    overflow-y: auto;
  }
}

.logo {
  display: flex;
  align-items: center;
  justify-content: center;
  width: 100%;
  padding: 10px;
}

.logo-link {
  text-decoration: none;
}

.logo:hover {
  background-color: gainsboro;
}

img {
  width: 50px;
  height: 50px;
  float: left;
}

h3 {
  margin: 0;
  letter-spacing: 0.1em;
  color: #22294e;
  font-weight: 1000;
}
</style>
