<template>
  <router-link class="nav-group-item"
    :to="to"
    :class="[{ active: isActive }]"
  >
  <div class="content">
    <slot></slot>
  </div>
  </router-link>
</template>

<script>
export default {
  name: "nav-item",
  props: {
    to: {
      type: Array [ String(), Object() ],
      required: false,
      default: "",
    },
  },
  methods: {
  },
  computed: {
    isActive()
    {
      if (this.to === "") 
      {
        return false;
      }

      const navRoute = this.$router.resolve(this.to).fullPath;
      return navRoute === this.$route.path;
    }
  }
};
</script>

<style scoped>
.nav-group-item {
    position: relative;
    display: block;
    color: #212529;
    text-decoration: none;
    background-color: #fff;
    border: 1px solid rgba(0,0,0,.125);
}

.content {
  padding: 0.5rem 1rem;
}

.nav-group-item.active {
  border: 1px solid purple;
  border-left: 10px solid purple;
  background-color: rgb(253, 239, 255);

}

.nav-group-item.active:hover {
  background-color: rgb(253, 239, 255);
}

.nav-group-item:hover {
  background-color: gainsboro;
}
</style>
