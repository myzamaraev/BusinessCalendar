import { createRouter, createWebHistory } from "vue-router";
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

  export default router;