<template>
  <BaseDialog :title="'Delete calendar identifier '+calendarIdentifier">
    <template v-slot:default>
      <form @submit.prevent="onSubmit">
        <p class="text-danger">
          <b>Are you sure about deleting the calendar? Deletion can't be undone, all data related to this calendar identifier will
          be permanently lost! </b>
        </p>
        <p class="text-danger text-sm">
         Be especially careful in production environment, as it can potentially cause issues with third-party services accessing this identifier through <b>Calendar API</b>. 
        </p>
        <div class="mb-3">
          <label for="identifier" class="form-label">please enter full API Identifier to confirm your intent</label>
          <input
            type="text"
            class="form-control"
            id="identifier"
            v-model="enteredIdentifier"
            placeholder="Enter API Identifier"
            @keydown="onIdentifierFieldKeydown"
            @paste.prevent
          />
          <div v-if="keyError" class="text-danger">
            <small>Entered text doesn't match with API identifier</small>
          </div>
        </div>
      </form>
    </template>
    <template v-slot:actions>
      <div class="d-grid gap-2 d-sm-block">
        <button type="submit" class="btn btn-danger" @click="onSubmit">
          &#128465; Delete
        </button>
        <button type="button" class="btn btn-secondary" @click="onCancel">
          &#x274C; Cancel
        </button>
      </div>
    </template>
  </BaseDialog>
</template>

<script>
import BaseDialog from "./UI/BaseDialog.vue";

export default {
  name: "delete-calendar-modal",
  components: { BaseDialog },
  props: {
    calendarIdentifier: {
      type: String,
      required: true,
    },
  },
  data() {
    return {
      enteredIdentifier: "",
      keyError: false,
    };
  },
  emits: ["submitted", "cancelled"],
  watch: {
    type() {
      this.key = "";
    },
  },
  methods: {
    onSubmit() {
      this.keyError = false;
      if (
        this.enteredIdentifier === "" ||
        this.enteredIdentifier != this.calendarIdentifier
      ) {
        this.keyError = true;
        return;
      }

      this.$emit("submitted", { type: this.type, key: this.key });
    },
    onCancel() {
      this.$emit("cancelled");
    },
    onIdentifierFieldKeydown(event) {
      if (/^\W$/.test(event.key)) {
        event.preventDefault();
      }
    },
  },
};
</script>

<style scoped>
.btn {
  margin: 2px;
}
</style>
