// The Vue build version to load with the `import` command
// (runtime-only or standalone) has been set in webpack.base.conf with an alias.
import Vue from 'vue'
import iView from 'iview'
import App from './App'
import { router } from './router/index'
import {appRouter} from './router/router'
import store from './store'
import axios from 'axios'
import ElementUI from 'element-ui'

import 'iview/dist/styles/iview.css'
import 'element-ui/lib/theme-chalk/index.css' 

//axios.defaults.baseURL = 'http://localhost:27298'  // 设置默认api地址可以解决不能调用WEBAPI的问题
axios.defaults.baseURL = 'http://localhost:8001'
axios.defaults.headers['Content-Type'] = 'application/x-www-form-urlencoded;charset=UTF-8'
// axios.defaults.headers['Content-Type'] = 'application/json'

Vue.config.productionTip = false
Vue.prototype.$http = axios

Vue.use(iView)
Vue.use(ElementUI)

/* eslint-disable no-new */
new Vue({
  el: '#app',
  router,
  store,
  render: h => h(App),
  data:{
    currentPageName:''
  },
  mounted(){
    this.currentPageName = this.$root.name;
    this.$store.commit('updateMenuList');
  },
  created(){
    let tagsList = [];
    appRouter.map((item)=>{
      if(item.children.length<=1){
        tagsList.push(item.children[0]);
      }else{
        tagsList.push(...item.children);
      }
    });
    console.log("maintagslist:"+tagsList.toString());
    this.$store.commit('setTagsList',tagsList);
  }
})
