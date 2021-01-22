import { mainRouter, appRouter } from '@/router/router';
import util from '@/libs/util'
import Cookies from 'js-cookie'
import Vue from 'vue'

const app = {
    state:{
        cachePage:[], 
        lang:'', // 语言
        isFullScreen:false, // 是否全屏
        openedSubmenuArr:[], // 展开的菜单数组
        menuTheme:'dark', // 菜单主题
        themeColor:'', // 主题颜色
        // 打开的内容选项页面列表
        pageOpenedList:[
            { 
            title:'首页',
            path:'',
            name:'home'
            }
        ],
        currentPageName:'',
        currentPath:[
            {
                title:'首页',
                path:'',
                name:'home'
            }
        ],
        menuList:[], // 面包屑数组
        routers:[mainRouter,...appRouter],
        tagsList:[...mainRouter.children],
        messageCount:0,
        dontCache:[], // 在这里定义不想要缓存的页面
    },
    mutations:{
        //改变状态(state)时，所做的操作
        setTagsList(state,list){
            state.tagsList.push(...list);
        },
        updateMenuList(state){
            let menuList = [];
            appRouter.forEach((item,index)=>{
                if(item.children.length==1){
                    menuList.push(item);
                }else{
                    let len = menuList.push(item);
                    let childrenArr = [];
                    childrenArr = item.children.filter((child)=>{
                        if(child.hidden != true) return child;
                    });
                    // menuList[len-1].children = childrenArr;
                    if (childrenArr === undefined || childrenArr.length === 0) {
                        menuList.splice(len - 1, 1);
                    } else {
                        let handledItem = JSON.parse(JSON.stringify(menuList[len - 1]));
                        handledItem.children = childrenArr;
                        menuList.splice(len - 1, 1, handledItem);
                    }
                }
            });
            state.menuList = menuList;
        },
        changeMenuTheme(state,menuTheme){
            state.menuTheme = menuTheme;
        },
        changeMainTheme(state,mainTheme){
            state.themeColor = mainTheme;
        },
        addOpenSubmenu(state,name){
            let hasThisName = false;
            let isEmpty = false;
            if(name.length <= 0) isEmpty = true;
            if(state.openedSubmenuArr.indexOf(name)>-1) hasThisName = true;
            if(!hasThisName && !isEmpty ) state.openedSubmenuArr.push(name);
        },
        closePage(state,name){
            state.cachePage.forEach((item,index) => {
                if(item==name) state.cachePage.splice(index,1); // 删除数组中index元素
            });
        },
        initCachePage(state){
            if(localStorage.cachePage) state.cachePage = JSON.parse(localStorage.cachePage);
        },
        removeTag(state,name){
            state.pageOpenedList.map((item,index)=>{
                if(item.name==name){
                    state.pageOpenedList.splice(index,1);
                }
            })
        },
        pageOpenedList (state, get) {
            let openedPage = state.pageOpenedList[get.index];
            if (get.argu) {
                openedPage.argu = get.argu;
            }
            if (get.query) {
                openedPage.query = get.query;
            }
            state.pageOpenedList.splice(get.index, 1, openedPage);
            localStorage.pageOpenedList = JSON.stringify(state.pageOpenedList);
        },
        clearAllTags (state) {
            state.pageOpenedList.splice(1);
            state.cachePage.length = 0;
            localStorage.pageOpenedList = JSON.stringify(state.pageOpenedList);
        },
        clearOtherTags (state, vm) {
            let currentName = vm.$route.name;
            let currentIndex = 0;
            state.pageOpenedList.forEach((item, index) => {
                if (item.name === currentName) {
                    currentIndex = index;
                }
            });
            if (currentIndex === 0) {
                state.pageOpenedList.splice(1);
            } else {
                state.pageOpenedList.splice(currentIndex + 1);
                state.pageOpenedList.splice(1, currentIndex - 1);
            }
            let newCachepage = state.cachePage.filter(item => {
                return item === currentName;
            });
            state.cachePage = newCachepage;
            localStorage.pageOpenedList = JSON.stringify(state.pageOpenedList);
        },
        setOpenedList (state) {
            state.pageOpenedList = localStorage.pageOpenedList ? JSON.parse(localStorage.pageOpenedList) : [otherRouter.children[0]];
        },
        setCurrentPath (state, pathArr) {
            state.currentPath = pathArr;
        },
        setCurrentPageName (state, name) {
            state.currentPageName = name;
        },
        setAvator (state, path) {
            localStorage.avatorImgPath = path;
        },
        switchLang (state, lang) {
            state.lang = lang;
            Vue.config.lang = lang;
        },
        clearOpenedSubmenu (state) {
            state.openedSubmenuArr.length = 0;
        },
        setMessageCount (state, count) {
            state.messageCount = count;
        },
        increateTag (state, tagObj) {
            if (!util.oneOf(tagObj.name, state.dontCache)) {
                state.cachePage.push(tagObj.name);
                localStorage.cachePage = JSON.stringify(state.cachePage);
            }
            state.pageOpenedList.push(tagObj);
            localStorage.pageOpenedList = JSON.stringify(state.pageOpenedList);
        },
        
    }
}

export default app;
