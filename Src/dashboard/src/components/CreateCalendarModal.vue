<template>
  <BaseDialog :show="show" title="Let's create new calendar!">
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
          <CountryPicker
            v-if="type === 'State'"
            id="key"
            class="form-control"
            @select="setCountryKey"
          ></CountryPicker>
          <input
            v-else
            type="text"
            class="form-control"
            id="key"
            v-model="key"
            placeholder="Enter unique calendar key"
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
          &#10004; Add
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
import CountryPicker from "./UI/CountryPicker.vue";

export default {
  name: "create-calendar-modal",
  components: { BaseDialog, CountryPicker },
  props: {
    show: {
      type: Boolean,
      required: true,
    },
  },
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
    },
  },
  methods: {
    onSubmit() {
      this.keyError = false;
      if (this.key === "") {
        this.keyError = true;
        return;
      }
      this.$emit("submitted", { type: this.type, key: this.key });
      this.key = "";
    },
    onCancel() {
      this.$emit("cancelled");
    },
    setCountryKey(country) {
      this.key = country;
    },
    onKeyFieldKeydown(event) {
      if (!/^[A-z_]*$/.test(event.key)) {
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
