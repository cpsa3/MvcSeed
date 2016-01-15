// require('./main.css');

var Vue = require('vue');
Vue.use(require('vue-resource'));

var app = new Vue({
    el: '#app',
    data: {
        view: 'page-login'
    },
    components: {
        'page-login': function(resolve) {
            require(['./modules/login/app.js'], resolve);
        },
        'page-home': function(resolve) {
            require(['./modules/home'], resolve);
        }
    }
});


function route() {
    app.view = window.location.hash.slice(1) || 'page-login';
}

window.addEventListener('hashchange', route);
window.addEventListener('load', route);
