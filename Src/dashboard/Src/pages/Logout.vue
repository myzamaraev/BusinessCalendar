<template>
  <section class="content-page-center">
    <div>
      <button class="btn btn-secondary btn-sm" @click="appLogout">
        Logout from App
      </button>
      <p>or</p>
      <button class="btn btn-dark btn-sm" @click="singleLogout">
        Finish SSO Session
      </button>
    </div>
  </section>
</template>

<script>
export default {
  name: "logout-page",
  methods: {
    async appLogout() {
      await this.$store.dispatch("appLogout");
    },
    singleLogout() {
      this.$store.dispatch("singleLogout");
    },
  },
};
</script>

<style scoped>
p {
    padding: 10px;
    margin: 0px;
}
</style>
