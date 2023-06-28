<template>
  <base-dialog title="New calendar">
    <!-- <base-dialog title="New calendar" @click="onCancel"> -->
    <template v-slot:default>
      <div class="row">
        <label for="type">Type</label>
        <select name="type" id="type" v-model="type">
          <option value="State">State</option>
          <option value="Custom">Custom</option>
        </select>
      </div>
      <div class="row">
        <label for="key">Key</label>
        <input type="text" id="key" v-model="key" />
      </div>
    </template>
    <template v-slot:actions>
      <button type="submit" @click="onSubmit">Create</button>
      <button type="button" @click="onCancel">Cancel</button>
    </template>
  </base-dialog>
</template>

<script>
import BaseDialog from "./UI/BaseDialog.vue";

export default {
  name: "create-calendar-dialog",
  components: { BaseDialog },
  data() {
    return {
      type: "State",
      key: "",
    };
  },
  emits: ["submitted", "cancelled"],
  methods: {
    onSubmit() {
      this.$emit("submitted", this.type, this.key);
    },
    onCancel() {
      this.$emit("cancelled");
    },
  },
};
</script>

<style scoped>
.row {
  margin: 10px;
}

label {
  margin: 10px;
}

/* form {
    width: 80%;
    margin: auto;
    padding: 10px;
    background-color: rgb(240, 240, 240);
} */
</style>
