<template>
  <section v-if="isAuthenticated" class="user-info-section">
    <div class="user-info">
      <div class="user-icon">
        <svg
          fill="none"
          height="16px"
          width="16px"
          aria-hidden="true"
          xmlns="http://www.w3.org/2000/svg"
          viewBox="0 0 14 18"
        >
          <path
            stroke="currentColor"
            stroke-linecap="round"
            stroke-linejoin="round"
            stroke-width="1.5"
            d="M7 8a3.5 3.5 0 1 0 0-7 3.5 3.5 0 0 0 0 7Zm-2 3h4a4 4 0 0 1 4 4v2H1v-2a4 4 0 0 1 4-4Z"
          />
        </svg>
      </div>
      <span class="user-name">{{ userName }}</span>
    </div>

    <CubeButton @click="logout">
      <svg
        fill="#000000"
        height="16px"
        width="16px"
        version="1.1"
        id="Capa_1"
        xmlns="http://www.w3.org/2000/svg"
        xmlns:xlink="http://www.w3.org/1999/xlink"
        viewBox="0 0 471.2 471.2"
        xml:space="preserve"
      >
        <g id="SVGRepo_bgCarrier" stroke-width="0"></g>
        <g
          id="SVGRepo_tracerCarrier"
          stroke-linecap="round"
          stroke-linejoin="round"
        ></g>
        <g id="SVGRepo_iconCarrier">
          <g>
            <g>
              <path
                stroke="currentColor"
                d="M227.619,444.2h-122.9c-33.4,0-60.5-27.2-60.5-60.5V87.5c0-33.4,27.2-60.5,60.5-60.5h124.9c7.5,0,13.5-6,13.5-13.5 s-6-13.5-13.5-13.5h-124.9c-48.3,0-87.5,39.3-87.5,87.5v296.2c0,48.3,39.3,87.5,87.5,87.5h122.9c7.5,0,13.5-6,13.5-13.5 S235.019,444.2,227.619,444.2z"
              ></path>
              <path
                stroke="currentColor"
                d="M450.019,226.1l-85.8-85.8c-5.3-5.3-13.8-5.3-19.1,0c-5.3,5.3-5.3,13.8,0,19.1l62.8,62.8h-273.9c-7.5,0-13.5,6-13.5,13.5 s6,13.5,13.5,13.5h273.9l-62.8,62.8c-5.3,5.3-5.3,13.8,0,19.1c2.6,2.6,6.1,4,9.5,4s6.9-1.3,9.5-4l85.8-85.8 C455.319,239.9,455.319,231.3,450.019,226.1z"
              ></path>
            </g>
          </g>
        </g>
      </svg>
    </CubeButton>
  </section>
  <section v-else class="login-section content-center">
    <button type="button" class="btn btn-dark" @click="login">Log in</button>
  </section>
</template>

<script>
import CubeButton from "../UI/CubeButton.vue";

export default {
  name: "user-info",
  components: {
    CubeButton,
  },
  computed: {
    userName() {
      return this.$store.getters.userName;
    },
    roles() {
      return this.$store.getters.roles;
    },
    isAuthenticated() {
      return this.$store.getters.isAuthenticated;
    },
  },
  methods: {
    async login() {
      await this.$store.dispatch("login");
    },
    async logout() {
      await this.$router.push({ name: "logout"})
    },
  },
};
</script>

<style scoped>
.login-section {
  display: flex;
  justify-content: center;
  align-items: center;
  padding: 10px;
}

.user-info-section {
  display: flex;
  justify-content: space-between;
  margin: 0 0px;
  padding: 10px;
  background-color: rgb(255, 255, 255);
}

.user-info {
  display: flex;
  align-items: center;
}

.user-icon {
  margin: 0 5px;
}

.user-name {
  font-weight: 500;
  width: 160px;
  color: #007fd4;
  font-family: monospace, monospace;
  white-space: nowrap;
  overflow: hidden;
  text-overflow: ellipsis;
}

.logout-btn {
  display: flex;
  align-items: center;
  justify-content: center;
  padding: 0px;
  border: none;
  width: 25px;
  height: 25px;
  border-radius: 5px;
}

.logout-btn:hover {
  background-color: gainsboro;
}
</style>
