const { defineConfig } = require("@vue/cli-service");
module.exports = defineConfig({
  transpileDependencies: true,
  devServer: {
    proxy: {
      "^/bff/v1": {
        //target: "http://localhost:5029",
        target: "https://localhost:7034",
        ws: false, //do not proxy websocket trafic
        changeOrigin: true,
      },
    },
  },
});
