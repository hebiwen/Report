<style lang="less">
    @import "../styles/main.less";
</style>
<template>
    <div class="main" :class="{'main-hide-text': shrink}">
        <!-- 左侧菜单栏 -->
        <div class="sidebar-menu-con" :style="{width: shrink?'60px':'200px', overflow: shrink ? 'visible' : 'auto'}">
            <scroll-bar ref="scrollBar">
                <shrinkable-menu 
                    :shrink="shrink"
                    :open-names="openedSubmenuArr"
                    :menu-list="menuList"
                    :before-push="beforePush"
                    @on-change="handleSubmenuChange"
                    >
                    <div slot="top" class="logo-con">
                        <img v-show="!shrink"  src="../images/logo.jpg" key="max-logo" />
                        <img v-show="shrink" src="../images/logo-min.jpg" key="min-logo" />
                    </div>
                </shrinkable-menu>
            </scroll-bar>
        </div>

        <!-- 顶部 -->
        <div class="main-header-con" :style="{paddingLeft: shrink?'60px':'200px'}">
            <div class="main-header">
                <div class="navicon-con">
                    <Button :style="{transform: 'rotateZ(' + (this.shrink ? '-90' : '0') + 'deg)'}" type="text" @click="toggleClick">
                        <Icon type="navicon" size="32"></Icon>
                    </Button>
                </div>
                <div class="header-middle-con">
                    <div class="main-breadcrumb">
                        <breadcrumb-nav :currentPath="currentPath"></breadcrumb-nav>
                    </div>
                </div>
                <div class="header-avator-con">
                    <message-tip v-model="mesCount"></message-tip>

                    <div class="user-dropdown-menu-con">
                        <Row type="flex" justify="end" align="middle" class="user-dropdown-innercon">
                            <Dropdown transfer trigger="click" @on-click="handleClickUserDropdown">
                                <a href="javascript:void(0)">
                                    <span class="main-user-name">{{ userName }}</span>
                                    <Icon type="arrow-down-b"></Icon>
                                </a>
                                <DropdownMenu slot="list">
                                    <DropdownItem name="ownSpace">个人中心</DropdownItem>
                                    <DropdownItem name="logout" divided>退出登录</DropdownItem>
                                </DropdownMenu>
                            </Dropdown>
                            <Avatar :src="avatorPath" style="background: #619fe7;margin-left: 10px;"></Avatar>
                        </Row>
                    </div>
                </div>
            </div>
            <div class="tags-con">
                <tags-page-opened :pageTagsList="pageTagsList"></tags-page-opened>
            </div>
        </div>

        <!-- 主体部分 -->
        <div class="single-page-con" :style="{left: shrink?'60px':'200px'}">
            <div class="single-page">
                <keep-alive :include="cachePage">
                    <router-view></router-view>
                </keep-alive>
            </div>
        </div>
    </div>
</template>

<script>
    import scrollBar from '@/views/my-components/scroll-bar/vue-scroller-bars';
    import shrinkableMenu from './main-components/shrinkable-menu/shrinkable-menu.vue';
    import breadcrumbNav from './main-components/breadcrumb-nav.vue';
    import messageTip from './main-components/message-tip.vue';
    import tagsPageOpened from './main-components/tags-page-opened.vue';

    import Cookies from 'js-cookie';
    import util from '@/libs/util.js';
    
    export default {
        data(){
            return {
                shrink:false,
                userName:'admin',
                openedSubmenuArr:this.$store.state.app.openedSubmenuArr
            }
        },
        components:{
            shrinkableMenu,
            scrollBar,
            breadcrumbNav,
            messageTip,
            tagsPageOpened
        },
        computed:{
            // computed定义的方法我们是以属性访问的形式调用的，{{menuList}}。具有缓存功能
            menuList(){
                return this.$store.state.app.menuList;
            },
            pageTagsList(){
                return this.$store.state.app.pageOpenedList;
            },
            currentPath(){
                return this.$store.state.app.currentPath;
            },
            avatorPath(){
                return localStorage.avatorImgPath;
            },
            cachePage(){
                return this.$store.state.app.cachePage;
            },
            mesCount(){
                return this.$store.state.app.messageCount;
            }
        },
        methods:{
            init(){
                let pathArr = util.setCurrentPath(this,this.$route.name);
                this.$store.commit('updateMenuList');
                if(pathArr.length>=2){
                    this.$store.commit('addOpenSubmenu',pathArr[1].name);
                }
                this.userName = Cookies.get('user');
                let messageCount = 3;
                this.messageCount = messageCount.toString();
                this.checkTag(this.$route.name);
                this.$store.commit('setMessageCount',3);
            },
            toggleClick(){},
            handleClickUserDropdown(name){
                if(name == "ownSpace"){
                    util.openNewPage(this,'');
                    this.$router.push({ name : 'ownSpane' })
                }else if(name == "logout") {
                    this.$store.commit('logout',this);
                    this.$router.push({ name : 'login' })
                }
            },
            checkTag(name){
                let openpageHasTag = this.pageTagsList.some(item=>{
                    if(item.name == name){
                        return true;
                    }
                });
                if(!openpageHasTag){
                    util.openNewPage(this,name,this.$route.params || {},this.$route.query || {} );
                }
            },
            handleSubmenuChange(val){ console.log(val); },
            beforePush(name){return true;},
            scrollBarResize(){}
        },
        mounted(){
            this.init();
        },
        created(){
            this.$store.commit('setOpenedList');
        },
        watch:{
            '$route'(to){
                this.$store.commit('setCurrentPageName',to.name);
                let pathArr = util.setCurrentPath(this,to.name);
                if(pathArr.length > 2){
                    this.$store.commit('addOpenSubmenu',pathArr[1].name);
                }
                this.checkTag(to.name);
                localStorage.currentPageName = to.name;
            }
        }
    }
</script>

