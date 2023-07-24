<template>
  <teleport to="body">
    <div class="backdrop">
      <dialog open>
        <header>
          <slot name="header">
            <h2>{{ title }}</h2>
          </slot>
        </header>
        <section>
          <slot></slot>
        </section>
        <div class="menu">
          <slot name="actions"></slot>
        </div>
      </dialog>
    </div>
  </teleport>
</template>

<script>
export default {
  name: "base-dialog",
  props: {
    title: {
      type: String,
      required: false,
    },
  },
};
</script>

<style scoped>
.backdrop {
  position: fixed;
  top: 0;
  left: 0;
  height: 100vh;
  width: 100%;
  background-color: rgba(0, 0, 0, 0.75);
  z-index: 10;
  overflow-y: auto;
}

dialog {
  position: absolute;
  top: 5vh;
  left: 5%;
  width: 90%;
  z-index: 100;
  border-radius: 12px;
  border: none;
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.26);
  padding: 0;
  margin: 0;
  overflow: hidden;
}

header {
  padding: 1rem;
  text-align: center;
}

header h2 {
  margin: 0;
}

section {
  padding: 1rem;
}

.menu {
  padding: 1rem;
  display: flex;
  justify-content: center;
}

@media (min-width: 768px) {
  dialog {
    left: calc(50% - 20rem);
    width: 40rem;
  }
}

@media (min-height: 500px) {
  dialog {
    top: 20vh;
  }
}
</style>
