<template>
  <div class="list-group">
    <div class="group-header">
      <div>
        <slot name="header">
          <h5 v-if="name">{{ name }}</h5>
        </slot>
      </div>
      <div class="header-right">
        <slot name="header-right"></slot>
      </div>
    </div>
    <slot></slot>
  </div>
</template>

<script>
export default {
  name: "nav-group",
  props: {
    name: {
      type: String,
      required: true,
    },
  },
};
</script>

<style scoped>
h5 {
  margin: 2px;
}
.group-header {
  display: flex;
  padding: 5px 10px;
  justify-content: space-between;
}

.header-right {
  display: flex;
  align-items: center;
  justify-content: cen;
}
</style>
