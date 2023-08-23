import { createRouter, createWebHistory } from "vue-router";
import TheCalendar from "./components/calendar/TheCalendar.vue";
import AboutPage from "./pages/About.vue";
import CalendarSettings from "./components/calendar/CalendarSettings.vue";
import YearLayout from "./components/calendar/YearLayout.vue";
import NotFound from "./pages/NotFound.vue";

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
          },
        ]
      },
      // pathMatch is the name of the param, e.g., going to /not/found yields
      // { params: { pathMatch: ['not', 'found'] }}
      // this is thanks to the last *, meaning repeated params and it is necessary if you
      // plan on directly navigating to the not-found route using its name
      { path: '/:pathMatch(.*)*', name: 'not-found', component: NotFound },
      // if you omit the last `*`, the `/` character in params will be encoded when resolving or pushing
      { path: '/:pathMatch(.*)', name: 'bad-not-found', component: NotFound }
    ],
  });

  export default router;