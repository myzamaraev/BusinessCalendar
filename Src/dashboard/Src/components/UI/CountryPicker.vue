<template>
  <select class="form-select" placeholder="Enter country name" v-model="selectedCountry">
    <option
      v-for="country in countries"
      :key="country.code"
      :value="country.code"
    >
      {{ country.name }}
    </option>
  </select>
</template>

<script>
import { mapGetters } from "vuex";

export default {
  name: "country-picker",
  data() {
    return {
        selectedCountry: "US"
    }
  },
  emits: ["select"],
  computed: {
    ...mapGetters("localization", ["countries", "userCountryCode"]),
  },
  watch: {
    selectedCountry(newValue) {
        this.$emit("select",newValue);
    }
  },
  created() {
    if (this.userCountryCode) {
        this.selectedCountry = this.userCountryCode;
    }
  }
};
</script>
