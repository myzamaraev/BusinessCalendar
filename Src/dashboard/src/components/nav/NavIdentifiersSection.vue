<template>
  <NavGroup name="Calendars">
    <template #header-right>
      <CubeButton @click="openCreatePage">
        <svg
          xmlns="http://www.w3.org/2000/svg"
          x="0px"
          y="0px"
          width="16px"
          height="16px"
          viewBox="0 0 24 24"
        >
          <path
            fill-rule="evenodd"
            d="M 11 2 L 11 11 L 2 11 L 2 13 L 11 13 L 11 22 L 13 22 L 13 13 L 22 13 L 22 11 L 13 11 L 13 2 Z"
          ></path>
        </svg>
      </CubeButton>
    </template>
    <template #default>
      <transition-group name="list" mode="out-in" tag="div">
        <NavItem
          v-for="identifier in identifiers"
          :key="identifier.id"
          :to="calendarRoute(identifier)"
        >
          <span v-if="identifier.type === 'State'">{{
            countryName(identifier.key)
          }}</span>
          <span v-else>{{ identifier.key }}</span>
        </NavItem>
      </transition-group>
      <Transition name="load-more-item">
        <NavItem v-if="hasMore" @click="loadMore">... Lad more</NavItem>
      </Transition>
    </template>
  </NavGroup>
  <CreateCalendarModal
    :show="isCreateModalVisible"
    @cancelled="isCreateModalVisible = false"
    @submitted="onCreateCalendarSubmitted"
  ></CreateCalendarModal>
</template>

<script>
import { mapGetters } from "vuex";
import NavGroup from "../UI/NavGroup.vue";
import NavItem from "../UI/NavItem.vue";
import CreateCalendarModal from "../CreateCalendarModal.vue";
import CubeButton from "../UI/CubeButton.vue";

export default {
  name: "nav-identifiers-section",
  components: {
    NavGroup,
    NavItem,
    CreateCalendarModal,
    CubeButton,
  },
  data() {
    return {
      isCreateModalVisible: false,
    };
  },
  computed: {
    ...mapGetters("navIdentifiers", ["identifiers", "hasMore"]),
    ...mapGetters("localization", ["countries"]),
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
    countryName(countryCode) {
      const country = this.countries.find((x) => x.code === countryCode);
      return country ? country.name : countryCode;
    },
  },
  created() {
    this.loadIdentifiers();
  },
};
</script>

<style scoped>
.load-more-item-leave-from {
  transform: translateX(0);
  opacity: 100%;
}
.load-more-item-leave-active {
  transition: all 0.3s ease-in;
}

.load-more-item-leave-to {
  transform: translateX(10%);
  opacity: 0;
}

.list-enter-from,
.list-leave-to {
  opacity: 0.5;
  transform: translateX(10%);
}

.list-enter-active,
.list-leave-active {
  transition: all 0.3s ease-in;
}

.list.enter-to,
.list.leave-from {
  opacity: 1;
  transform: translateX(0);
}
</style>
