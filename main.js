import Vue from 'vue'
import App from './App'
import store from './store'
import axios from 'axios'
import uView from "uview-ui";
import constConfig from './const.js'

Vue.use(uView);

// 引入uView对小程序分享的mixin封装
let mpShare = require('uview-ui/libs/mixin/mpShare.js');
Vue.mixin(mpShare);
// 判断是否登录
let hasLogin = require('@/mixins/hasLogin.js');
Vue.mixin(hasLogin);

Vue.config.productionTip = false
Vue.prototype.$store = store
App.mpType = 'app'

Vue.prototype.$const = constConfig

const app = new Vue({
	...App
})


app.$mount()


