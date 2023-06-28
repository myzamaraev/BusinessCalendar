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
}

dialog {
  position: fixed;
  top: 20vh;
  left: 25%;
  width: 50%;
  z-index: 100;
  border-radius: 12px;
  border: none;
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.26);
  padding: 0;
  margin: 0;
  overflow: hidden;
}

/* dialog {
    position: fixed;
    top: 20vh;
    margin: auto;
    width: 40%;
    background-color: white;
    box-shadow: 0 2px 8px rgba(0,0,0,0.26);
    border: 1px solid;
} */

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
  justify-content: flex-end;
  margin: 0;
}

@media (min-width: 768px) {
  dialog {
    left: calc(50% - 20rem);
    width: 40rem;
  }
}
</style>
