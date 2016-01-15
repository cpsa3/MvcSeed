//require('es6-promise').polyfill();

var path = require('path');

module.exports = {
    //context: path.join(__dirname, '../'),
    entry: "./app/main.js",
    output: {
        path: "./build",
        publicPath: "/Scripts/build/",
        filename: "build.js"
    },
    module: {
        loaders: [{
            test: /\.css$/,
            loader: "style-loader!css-loader"
        }, {
            test: /\.(png|jpg|gif)$/,
            loader: 'url-loader'
        }, {
            test: /\.(html|tpl)$/,
            loader: 'html-loader'
        }, ]
    }
}

console.log(path.join(__dirname, '../'));
