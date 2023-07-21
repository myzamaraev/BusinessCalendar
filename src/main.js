import { createApp } from "vue";
import "bootstrap/dist/css/bootstrap.min.css"

import App from "./App.vue";
import Store from "./store/index.js";
import Router from "./router.js";
import BootstrapAlert from "./components/UI/BootstrapAlert.vue";

const app = createApp(App);
app.use(Router);
app.use(Store);

app.component("b-alert", BootstrapAlert);

app.mount("#app");
