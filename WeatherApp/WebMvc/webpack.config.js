var path = require("path");
var webpack = require("webpack");
 
module.exports = {
    entry: "./ClientScripts/Main/Main.js",
    output: {
        path: __dirname + "/wwwroot/dist",
        filename: "bundle.js",
        library: "BundleLib",
        libraryTarget: "window"
    },
    plugins: [
        new webpack.ProvidePlugin({
            $: "jquery",
            jQuery: "jquery"
        })
    ],
    module: {
        rules: [{
                test: require.resolve('jquery'),
                use: [{
                    loader: 'expose-loader',
                    options: {
                        exposes: ["$", "jQuery"],
                    }
                }]
            },
            {
                test: require.resolve('./ClientScripts/Js/ChartUtil'),
                use: [{
                    loader: 'expose-loader',
                    options: {
                        exposes: ["ChartUtil"],
                    }
                }]
            },
            {
                test: /\.css$/i,
                use: ["style-loader", "css-loader"],
            },
        ]
    }
};