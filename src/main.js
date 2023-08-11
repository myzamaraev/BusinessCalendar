import { createApp } from "vue";
import "bootstrap/dist/css/bootstrap.min.css";
import Toast from "vue-toastification";
import "vue-toastification/dist/index.css";

import App from "./App.vue";
import Store from "./store/index.js";
import Router from "./router.js";
import BootstrapAlert from "./components/UI/BootstrapAlert.vue";
import BootstrapLoader from "./components/UI/BootstrapLoader.vue";

const app = createApp(App);
app.use(Router);
app.use(Store);

const options = {
  toastDefaults: {
    // ToastOptions object for each type of toast
    'error': {
      timeout: 5000,
      hideProgressBar: true,
    },
    'warning': {
      timeout: 5000,
      hideProgressBar: true,
    },
    'success': {
      timeout: 2000,
      hideProgressBar: true,
    },
    'info': {
      timeout: 2000,
      hideProgressBar: true,
    },
  },
};

app.use(Toast, options);

app.component("b-alert", BootstrapAlert);
app.component("b-loader", BootstrapLoader);

app.mount("#app");
