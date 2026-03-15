import { defineConfig } from "vite";
import vue from "@vitejs/plugin-vue";
import { fileURLToPath, URL } from "node:url";

export default defineConfig({
  plugins: [vue()],
  resolve: {
    alias: {
      "@": fileURLToPath(new URL("./src", import.meta.url)),
    },
  },
  build: {
    target: "esnext",
  },
  server: {
    proxy: {
      "^/bff/v1": {
        // target: "http://localhost:5029",
        target: "https://localhost:7034",
        ws: false,
        changeOrigin: true,
      },
    },
  },
});
