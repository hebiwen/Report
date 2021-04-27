import main from '@/views/main.vue';

export const loginRouter = {
    path:'/login',
    name:'login',
    meta:{
        title:'登录'
    },
    component:()=> import('@/views/login.vue')
}

export const mainRouter = {
    path:'/',
    name:'mainRouter',
    redirect:'/home',
    component:main,
    children:[
        { path:'home',title:'主页',name:'home',component:()=>import('@/views/home/home.vue') }
    ]
}

export const appRouter = [
    {
        path:'/report',
        icon:'ios-paper',
        name:'report',
        title:'行业报告',
        component:main,
        children:[
            { path: 'reportlist', title: '报告列表', name: 'reportList',icon:'ios-copy', component: () => import('@/views/report/report_list.vue') },
            { path: 'reportadd', title: '添加报告', name: 'reportAdd',hidden:true, component: () => import('@/views/report/report_add.vue') },
            { path: 'reportedit', title: '修改报告', name: 'reportEdit',hidden:true, component: () => import('@/views/report/report_add.vue') },
            { path: 'hytllist', title: '题录列表', name: 'hytlList', icon:'ios-list', component: () => import('@/views/report/hytl_list.vue') },
            { path: 'hytledit', title: '修改题录', name: 'hytlEdit',hidden:true, component: () => import('@/views/report/hytl_add.vue') },
            { path: 'hytladd', title: '添加题录', name: 'hytlAdd',hidden:true, component: () => import('@/views/report/hytl_add.vue') },
            { path: 'dzbglist', title: '定制报告', name: 'dzbgList',icon:'ios-browsers', component: () => import('@/views/report/dzbg_list.vue') },
            { path: 'dzbgadd', title: '添加定制报告', name: 'dzbgAdd',hidden:true, component: () => import('@/views/report/dzbg_add.vue') },
            { path: 'dzbgedit', title: '修改定制报告', name: 'dzbgEdit',hidden:true, component: () => import('@/views/report/dzbg_add.vue') }
        ]
    },
    {
        path: '/user',
        icon: 'ios-people',
        name: 'user',
        title: '会员管理',
        component: main,
        children: [
            { path: 'userlist', title: '会员列表', name: 'userList', icon: 'md-people', component: () => import('@/views/user/user_list.vue') },
            { path:'useradd',title:'添加会员',name:'userAdd',hidden:true, component:()=>import('@/views/user/user_add.vue') },
            { path: 'useriplist', title: 'IP管理', name: 'userIpList', icon: 'ios-card', component: () => import('@/views/user/user_ip_list.vue') },
            { path: 'useripadd', title: '添加会员IP', name: 'userIpAdd', hidden:true, component: () => import('@/views/user/user_ip_add.vue') },
        ]
    },
    {
        path: '/security',
        icon: 'ios-cube',
        name: 'security',
        title: '权限管理',
        component: main,
        children: [
            { path: 'adminlist', title: '管理员列表', name: 'adminList', icon: 'md-people', component: () => import('@/views/security/admin_list.vue') },
            { path: 'adminadd', title: '添加管理员', name: 'adminAdd', hidden:true, component: () => import('@/views/security/admin_add.vue') },
            { path: 'rolelist', title: '角色管理', name: 'roleList', icon: 'md-contacts', component: () => import('@/views/security/role.vue') },
            { path: 'permissionlist', title: '权限管理', name: 'permissionList', icon: 'md-list-box', component: () => import('@/views/security/permission.vue') },
            { path:'permissionchild',title:'菜单管理',name:'permissionChild',icon: 'ios-menu',component:()=>import('@/views/security/permission_child.vue') },
        ]
    },
    {
        path:'/web',
        icon:'md-browsers',
        name:'web',
        title:'前端管理',
        component:main,
        children:[
            { path: '/', title: '报告图片', name: 'placards',icon:'ios-images', component: () => import('@/views/report/report_list.vue') },
            { path: '/', title: '重点报告', name: 'placards',icon:'ios-notifications', component: () => import('@/views/report/report_list.vue') }
        ]
    },
    {
        path:'/charges',
        icon:'ios-browsers',
        name:'comment',
        title:'充值消费',
        component:main,
        children:[
            { path: '/', title: '会员充值', name: 'comments',icon:'ios-star', component: () => import('@/views/report/report_list.vue') },
            { path: '/', title: '报告下载', name: 'suggests',icon:'ios-flag', component: () => import('@/views/report/report_list.vue') }
        ]
    },
    {
        path:'/emails',
        icon:'ios-mail',
        name:'email',
        title:'邮件管理',
        component:main,
        children:[
            { path: '', title: '账号管理', name: 'accounts',icon:'ios-person', component: () => import('@/views/test/test.vue') },
            { path: '', title: '发送列表', name: 'sends',icon:'ios-send', component: () => import('@/views/test/test.vue') }
        ]
    },
    {
        path:'/record',
        icon:'ios-stats',
        name:'source',
        title:'报表管理',
        component:main,
        children:[
            { path: '', title: '会员下载', name: 'sources',icon:'document-text', component: () => import('@/views/test/test.vue') },
            { path: '', title: '会员浏览', name: 'sources',icon:'document-text', component: () => import('@/views/test/test.vue') },
            { path: '', title: '报告下载', name: 'sources',icon:'document-text', component: () => import('@/views/test/test.vue') },
            { path: '', title: '报告浏览', name: 'sources',icon:'document-text', component: () => import('@/views/test/test.vue') },
            { path: '', title: '检索词', name: 'sources',icon:'document-text', component: () => import('@/views/test/test.vue') },
            { path: '', title: '热点词', name: 'sources',icon:'document-text', component: () => import('@/views/test/test.vue') },
            { path: '', title: '会员订单', name: 'sources',icon:'document-text', component: () => import('@/views/test/test.vue') },
        ]
    },
    {
        path:'/tags',
        icon:'md-pricetags',
        name:'tag',
        title:'标签管理',
        component:main,
        children:[
            { path: '', title: '关键词列表', name: 'tags',icon:'pricetag', component: () => import('@/views/test/test.vue') },
        ]
    },
    {
        path:'/system',
        icon:'ios-construct',
        name:'system',
        title:'系统设置',
        component:main,
        children:[
            { path: '', title: '报告价格', name: 'sys',icon:'wrench', component: () => import('@/views/test/test.vue') },
            { path: '', title: '报告分类', name: 'sys',icon:'wrench', component: () => import('@/views/test/test.vue') },
            { path: '', title: '行业分类', name: 'sys',icon:'wrench', component: () => import('@/views/test/test.vue') },
            { path: 'test2', title: '系统设置', name: 'sys',icon:'wrench', component: () => import('@/views/test/test2.vue') },
        ]
    }
]

//所有上面定义的路由都要写在下面的routers里
export const routers = [
    loginRouter,
    ...appRouter,
    mainRouter
];

