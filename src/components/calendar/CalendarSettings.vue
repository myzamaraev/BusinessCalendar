<template>
  <section>
    <InputCopy name="API Idntifier" :value="identifier" />

    <label for="delete-btn">Remove calendar</label>
    <button
      id="delete-btn"
      class="btn btn-danger btn-sm"
      @click="isDeleteModalVisible = true"
    >
      &#128465; Delete
    </button>

    <DeleteCalendarModal
      :calendarIdentifier="identifier"
      v-if="isDeleteModalVisible"
      @cancelled="isDeleteModalVisible = false"
      @submitted="onDeleteCalendarSubmitted"
    ></DeleteCalendarModal>
  </section>
</template>

<script>
import { mapGetters } from "vuex";
import InputCopy from "../UI/InputCopy.vue";
import DeleteCalendarModal from "../DeleteCalendarModal.vue";

export default {
  name: "calendar-settings",
  components: {
    InputCopy,
    DeleteCalendarModal,
  },
  props: {
    calendarType: {
      type: String,
      required: true,
    },
    calendarKey: {
      type: String,
      required: true,
    },
  },
  data() {
    return {
      isDeleteModalVisible: false,
    };
  },
  computed: {
    ...mapGetters("calendar", ["identifier"]),
  },
  methods: {
    async onDeleteCalendarSubmitted() {
      const isSuccess = await this.$store.dispatch("navIdentifiers/delete", {
        identifier: this.identifier,
      });
      if (isSuccess) {
        this.isDeleteModalVisible = false;
        this.$router.push("/")
      }
    },
  },
};
</script>

<style scoped>
section {
  padding: 20px;
}

section > * {
  margin: 10px;
}
</style>
