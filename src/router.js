import { createRouter, createWebHistory } from "vue-router";
import TheCalendar from "./components/calendar/TheCalendar.vue";
import AboutPage from "./pages/About.vue";
import CalendarSettings from "./components/calendar/CalendarSettings.vue";
import YearLayout from "./components/calendar/YearLayout.vue";

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
        path: "/:calendarType(state|custom)/:calendarKey",
        component: TheCalendar,
        props: true,
        children: [
          {
            name: "calendar",
            path: ":year(\\d{1,4})?",
            props: true,
            component: YearLayout,
          },
          {
            name: "calendarSettings",
            path: "settings",
            component: CalendarSettings,
            props: true,
          }
        ]
      },
    ],
  });

  export default router;