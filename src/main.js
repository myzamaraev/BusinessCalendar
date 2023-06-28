import { createApp } from "vue";
import { createRouter, createWebHistory } from "vue-router";
import App from "./App.vue";
import CalendarIdentifiers from "./components/CalendarIdentifiers.vue";
import TheCalendar from "./components/TheCalendar.vue";

const router = createRouter({
  history: createWebHistory(),
  routes: [
    { path: "/", redirect: { name: "list" } },
    {
      name: "list",
      path: "/list",
      component: CalendarIdentifiers,
    },
    {
      name: "calendar",
      path: "/:calendarType(//state|custom)/:calendarKey",
      component: TheCalendar,
      props: true,
    }
  ],
});

const app = createApp(App);
app.use(router);
app.mount("#app");
