<template>
  <div>
    <router-link
      class="nav-group-item"
      :to="to"
      :class="[{ active: isActive }]"
    >
      <div class="content">
        <slot></slot>
      </div>
    </router-link>
  </div>
</template>

<script>
export default {
  name: "nav-item",
  props: {
    to: {
      type: Array[(String(), Object())],
      required: false,
      default: "",
    },
  },
  methods: {},
  computed: {
    isActive() {
      if (this.to === "") {
        return false;
      }

      const navRoute = this.$router.resolve(this.to).fullPath;
      const currentPath = this.$route.path;
      return new RegExp("^" + navRoute).test(currentPath);
    },
  },
};
</script>

<style scoped>
.nav-group-item {
  position: relative;
  display: block;
  color: #212529;
  text-decoration: none;
  background-color: #fff;
  border: 1px solid rgba(0, 0, 0, 0.125);
  transform: translateX(-2px);
  transition: all 0.2s ease-out;
  transform: translateX(-2px);
  transition: all 0.5s ease;
}

.content {
  padding: 0.5rem 1rem;
}

.nav-group-item.active {
  border: 1px solid #007fd4;
  border-left: 10px solid #007fd4;
  background-color: #e9f6ff;
}

.nav-group-item.active:hover {
  background-color: #cde9fc;
}

.nav-group-item:hover {
  background-color: #dcdcdc;
}
</style>
