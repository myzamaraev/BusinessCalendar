import { createRouter, createWebHistory } from "vue-router";
import TheCalendar from "./components/calendar/TheCalendar.vue";
import AboutPage from "./pages/About.vue";

const router = createRouter({
    history: createWebHistory(),
    routes: [
      { path: "/", redirect: { name: "about" } },
      {
        name: "about",
        path: "/about",
        component: AboutPage,
      },
      {
        name: "calendar",
        path: "/:calendarType(state|custom)/:calendarKey",
        component: TheCalendar,
        props: true,
      }
    ],
  });

  export default router;