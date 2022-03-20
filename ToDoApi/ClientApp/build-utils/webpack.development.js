const BabelStyledComponentsPlugin = require("babel-plugin-styled-components");

module.exports = (env) => ({
    mode: "development",
    module: {
        rules: [
            {
                test: /\.s[ac]ss$/,
                use: [
                    // 3. Creates `style` nodes from JS strings
                    "style-loader",
                    // 2. Translates CSS into CommonJS
                    {
                        loader: "css-loader",
                        options: {
                            modules: false
                            // modules: /\.module\.\w+$/
                        }
                    },
                    // 1. Compiles Sass to CSS
                    "sass-loader",
                ],
            }
        ],
    }
});