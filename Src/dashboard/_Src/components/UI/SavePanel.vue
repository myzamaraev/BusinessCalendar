<template>
  <teleport to="body">
    <section>
      <b-alert class="alert" type="secondary">
        <span>Some changes are not saved yet.. </span>
        <div class="actions">
          <button type="button" @click="save" class="btn btn-primary btn-sm">
            Save
          </button>
          <button
            type="button"
            @click="cancel"
            class="btn btn-secondary btn-sm"
          >
            Cancel
          </button>
        </div>
      </b-alert>
    </section>
  </teleport>
</template>

<script>
export default {
  name: "save-panel",
  emits: ["save", "cancel"],
  methods: {
    save() {
      this.$emit("save");
    },
    cancel() {
      this.$emit("cancel");
    },
  },
};
</script>

<style scoped>
section {
  display: flex;
  justify-content: center;
  position: fixed;
  bottom: 0;
  text-align: center;
  width: 100%;
}
button {
  margin-left: 5px;
}

.alert {
  width: 100%;
}

.actions {
  display: inline-block;
}

@media screen and (min-width: 500px) {
  .alert {
    max-width: 500px;
  }
}
</style>
