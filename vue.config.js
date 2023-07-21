const { defineConfig } = require("@vue/cli-service");
module.exports = defineConfig({
  transpileDependencies: true,
  devServer: {
    proxy: {
      "^/(Management|CalendarIdentifier)": {
        //target: "http://localhost:5029/api/v1",
        target: "https://localhost:7034/api/v1",
        ws: false, //do not proxy websocket trafic
        changeOrigin: true,
      },
      // "^/(Management)": {
      //   target: "https://localhost:7034/api/v1",
      //   ws: false, //do not proxy websocket trafic
      //   changeOrigin: true,
      // },
    },
  },
});
