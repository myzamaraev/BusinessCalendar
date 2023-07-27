<template>
  <BaseDialog title="Let's create new calendar">
    <template v-slot:default>
      <form @submit.prevent>
        <div class="mb-3">
          <label for="type" class="form-label">Type</label>
          <select name="type" class="form-select" id="type" v-model.trim="type">
            <option value="State">State</option>
            <option value="Custom">Custom</option>
          </select>
          <div id="typeHelp" class="form-text"></div>
        </div>
        <div class="mb-3">
          <label for="key" class="form-label">Key</label>
          <CountryPicker v-if="type==='State'"
          id="key"
          class="form-control"
          @select="setCountryKey"
          ></CountryPicker>
          <input v-else
            type="text"
            class="form-control"
            id="key"
            v-model="key"
            @keydown="onKeyFieldKeydown"
          />
          <div v-if="keyError" class="text-danger">
            <small>Please provide unique key</small>
          </div>
        </div>
      </form>
    </template>
    <template v-slot:actions>
      <div class="d-grid gap-2 d-sm-block">
        <button type="submit" class="btn btn-success" @click="onSubmit">
          Add
        </button>
        <button type="button" class="btn btn-secondary" @click="onCancel">
          Cancel
        </button>
      </div>
    </template>
  </BaseDialog>
</template>

<script>
import BaseDialog from "./UI/BaseDialog.vue";
import CountryPicker from "./UI/CountryPicker.vue";

export default {
  name: "create-calendar-dialog",
  components: { BaseDialog, CountryPicker },
  data() {
    return {
      type: "State",
      key: "",
      keyError: false,
    };
  },
  emits: ["submitted", "cancelled"],
  watch: {
    type() {
      this.key = "";
    }
  },
  methods: {
    onSubmit() {
      this.keyError = false;
      if (this.key === "") {
        this.keyError = true;
        return;
      }
      this.$emit("submitted", { type: this.type, key: this.key });
    },
    onCancel() {
      this.$emit("cancelled");
    },
    setCountryKey(country) {
      this.key = country;
      console.log(this.key);
    },
    onKeyFieldKeydown(event) {
      if (/^\W$/.test(event.key)) {
        event.preventDefault();
      }
    }
  },
};
</script>

<style scoped>
.btn {
  margin: 2px;
}
</style>
