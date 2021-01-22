import Vue from 'vue'
import iView from 'iview'
import VueRouter from 'vue-router'
// import HelloWorld from '@/components/HelloWorld'
import {routers,mainRouter,appRouter } from './router'
import util from '../libs/util'
import Cookies from 'js-cookie'

// import main from '@/views/main.vue';

Vue.use(VueRouter)

const routerConfig = {
  mode:'history',
  routes:routers
}

export const router = new VueRouter(routerConfig);

router.beforeEach((to,from,next)=>{
  iView.LoadingBar.start();
  util.title(to.meta.title,router.app);

  // if(!Cookies.get('user') && to.name !='login'){
  //   // 判断是否已经登录且前往的页面不是登录页
  //   next({name:'login'});
  // } 
  if(!Cookies.get('user') && to.name =='login'){
    // 判断是否已经登录且前往的页面不是登录页
    next({name:'home'});
  } 
  else if(Cookies.get('user') && to.name =='login' ){
    // 判断是否已经登录且前往的页面是登录页
    util.title();
    next({name:'home'});
  }
  else{
    const curRouterObj = util.getRouterObjByName([mainRouter,...appRouter],to.name);
    if(curRouterObj && curRouterObj.title){
      util.title(curRouterObj.title,router.app);
    }
    // 需要判断权限的路由
    if(curRouterObj && curRouterObj.access != undefined){
      if(curRouterObj.access == parseInt(Cookies.get('access'))){
        // 如果在地址栏输入的是一级菜单则默认打开其第一个二级菜单的页面
        util.toDefaultPage([mainRouter,...appRouter],to.name,router,next);
      }else{
        next({replace:true,name:'home'});
      }
      
    }else{
      // 没有配置权限的路由, 直接通过
      util.toDefaultPage([...routers],to.name,router,next);
    }
  }
})

router.afterEach((to)=>{
  util.openNewPage(router.app,to.name,to.params,to.query);
  iView.LoadingBar.finish();
  window.scrollTo(0,0);
})


