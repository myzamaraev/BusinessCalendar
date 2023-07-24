<template>
  <NavGroup name="Calendars">
    <template #header-right> 
       
      <button
        class="add-calendar-button btn btn-light"
        @click="openCreatePage"
      >
        <b>+</b>
      </button>
    </template>
    <template #default>
      <NavItem
        v-for="identifier in identifiers"
        :key="identifier.id"
        :to="calendarRoute(identifier)"
      >
        {{ identifier.id }}
      </NavItem>
      <NavItem v-if="hasMore" @click="loadMore">... Lad more</NavItem>
    </template>
  </NavGroup>
  <CreateCalendarModal
    v-if="isCreateModalVisible"
    @cancelled="isCreateModalVisible = false"
    @submitted="onCreateCalendarSubmitted"
  ></CreateCalendarModal>
</template>

<script>
import { mapGetters } from "vuex";
import NavGroup from "./UI/NavGroup.vue";
import NavItem from "./UI/NavItem.vue";
import CreateCalendarModal from "./CreateCalendarModal.vue";

export default {
  name: "nav-identifiers-section",
  components: {
    NavGroup,
    NavItem,
    CreateCalendarModal,
  },
  data() {
    return {
      isCreateModalVisible: false,
    };
  },
  computed: {
    ...mapGetters("navIdentifiers", ["identifiers", "hasMore"]),
  },
  methods: {
    calendarRoute(calendar) {
      return {
        name: "calendar",
        params: {
          calendarType: calendar.type,
          calendarKey: calendar.key,
        },
      };
    },
    setActive(event) {
      event.target.classList.add("active");
    },
    loadMore() {
      this.$store.dispatch("navIdentifiers/loadMore");
    },
    loadIdentifiers() {
      this.$store.dispatch("navIdentifiers/init");
    },
    openCreatePage() {
      this.isCreateModalVisible = true;
      //this.$router.push({ name: "createCalendar" });
    },

    async onCreateCalendarSubmitted(payload) {
      const isSuccess = await this.$store.dispatch(
        "navIdentifiers/create",
        payload
      );
      if (isSuccess) {
        this.isCreateModalVisible = false;
      }
    },
  },
  created() {
    this.loadIdentifiers();
  },
};
</script>

<style scoped>
.add-calendar-button {
  width: 25px;
  height: 25px;
  padding: 0px;
  margin-left: 5px;
}
</style>
