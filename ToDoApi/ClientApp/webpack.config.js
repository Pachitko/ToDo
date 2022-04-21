const path = require("path");
const { merge } = require("webpack-merge");
const modeConfiguration = env => require(`./build-utils/webpack.${env}`)(env);
const HtmlWebpackPlugin = require("html-webpack-plugin")

module.exports = (args) => {
  console.log(`Mode is: ${args.mode}`);
  return merge({
    entry: path.resolve(__dirname, "src/index.tsx"),
    output: {
      path: path.resolve(__dirname, 'dist'),
      filename: "bundle.js",
      publicPath: '/'
    },
    devtool: 'source-map',
    // devtool: "nosources-source-map",
    devServer: {
      // static: {
      //   directory: path.join(__dirname, 'dist'),
      // },
      historyApiFallback: true,
      compress: true,
      port: 9000,
      open: true
    },
    resolve: {
      alias: {
        "src": path.resolve('./src')
      },
      extensions: [".ts", ".tsx", ".js", ".jsx"]
    },
    module: {
      rules: [{
        test: /\.(j|t)sx?$/,
        exclude: /(node_modules|bower_components)/,
        loader: "babel-loader",
        options: {
          presets: [
            // "@babel/env", // forbids using 'async'
            "@babel/react",
            "@babel/typescript"
          ],
          plugins: [
            "babel-plugin-styled-components"
          ]
        }
      },
      // {
      //   test: /\.tsx?$/,
      //   exclude: /(node_modules|bower_components)/,
      //   loader: "ts-loader"
      // },
      {
        test: /\.css$/,
        use: [
          "style-loader",
          {
            loader: "css-loader",
            options: {
              modules: false
            }
          }
        ]
      },
      {
        test: /\.(png|svg|jpg|gif|jpeg)$/,
        exclude: /node_modules/,
        use: ["url-loader", "file-loader"]
      }
      ]
    },
    plugins: [new HtmlWebpackPlugin({
      title: "To Do",
      template: "./src/index.html",
      filename: "./index.html",
      minify: true
    })]
  }, modeConfiguration(args.mode))
}