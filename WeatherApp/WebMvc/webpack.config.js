var path = require("path");
var webpack = require("webpack");
 
module.exports = {
    entry: "./ClientScripts/ChartUtil.js",
    output: {
        path: __dirname + "/wwwroot/dist",
        filename: "bundle.js",
        library: "ChartUtil",
        libraryTarget: "window"
    }
};