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
        icon:'social-windows',
        name:'report',
        title:'行业报告',
        component:main,
        children:[
            { path: 'report_list', title: '报告列表', name: 'reportList',icon:'social-twitch', component: () => import('@/views/report/report_list.vue') },
            { path: 'report_add', title: '添加报告', name: 'reportAdd',hidden:true, component: () => import('@/views/report/report_add.vue') },
            { path: 'hytl_list', title: '题录列表', name: 'hytl', icon:'ios-pricetag', component: () => import('@/views/report/hytl_list.vue') },
            { path: 'hytl_add', title: '添加题录', name: 'hytlAdd',hidden:true, component: () => import('@/views/report/hytl_add.vue') },
            { path: 'dzbg_list', title: '定制报告', name: 'dzbg',icon:'ios-compose', component: () => import('@/views/report/dzbg_list.vue') },
            { path: 'dzbg_add', title: '添加定制报告', name: 'dzbgAdd',hidden:true, component: () => import('@/views/report/dzbg_add.vue') }
        ]
    },
    {
        path: '/user',
        icon: 'person-stalker',
        name: 'user',
        title: '会员管理',
        component: main,
        children: [
            { path: 'user_list', title: '会员列表', name: 'userList', icon: 'android-people', component: () => import('@/views/user/user_list.vue') },
            { path:'user_add',title:'添加会员',name:'addUser',hidden:true, component:()=>import('@/views/user/user_add.vue') },
            { path: 'user_ip_list', title: 'IP管理', name: 'userIp', icon: 'android-people', component: () => import('@/views/user/user_ip_list.vue') },
            { path: 'user_ip_add', title: '添加会员IP', name: 'userIpAdd', hidden:true, component: () => import('@/views/user/user_ip_add.vue') },
        ]
    },
    {
        path: '/security',
        icon: 'ios-people',
        name: 'security',
        title: '权限管理',
        component: main,
        children: [
            { path: 'admin_list', title: '管理员列表', name: 'admin', icon: 'android-people', component: () => import('@/views/security/admin_list.vue') },
            { path: 'admin_add', title: '添加管理员', name: 'adminAdd', hidden:true, component: () => import('@/views/security/admin_add.vue') },
            { path: 'role_list', title: '角色管理', name: 'roles', icon: 'arrow-swap', component: () => import('@/views/security/role.vue') },
            { path: 'permission_list', title: '权限管理', name: 'permissions', icon: 'arrow-swap', component: () => import('@/views/security/permission.vue') },
            { path:'permission_child',title:'菜单管理',name:'permissionChild',component:()=>import('@/views/security/permission_child.vue') },
        ]
    },
    {
        path:'/',
        icon:'clipboard',
        name:'web',
        title:'前端管理',
        component:main,
        children:[
            { path: '/', title: '报告图片', name: 'placards',icon:'ios-compose', component: () => import('@/views/report/report_list.vue') },
            { path: '/', title: '重点报告', name: 'placards',icon:'ios-compose', component: () => import('@/views/report/report_list.vue') }
        ]
    },
    {
        path:'/',
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
        path:'/',
        icon:'ios-email',
        name:'email',
        title:'邮件管理',
        component:main,
        children:[
            { path: '', title: '账号管理', name: 'accounts',icon:'ios-person', component: () => import('@/views/test/test.vue') },
            { path: '', title: '发送列表', name: 'sends',icon:'ios-paperplane', component: () => import('@/views/test/test.vue') }
        ]
    },
    {
        path:'/',
        icon:'ios-paper',
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
        path:'/',
        icon:'ios-pricetag',
        name:'tag',
        title:'标签管理',
        component:main,
        children:[
            { path: '', title: '关键词列表', name: 'tags',icon:'pricetag', component: () => import('@/views/test/test.vue') },
        ]
    },
    {
        path:'/',
        icon:'settings',
        name:'system',
        title:'系统设置',
        component:main,
        children:[
            { path: '', title: '报告价格', name: 'sys',icon:'wrench', component: () => import('@/views/test/test.vue') },
            { path: '', title: '报告分类', name: 'sys',icon:'wrench', component: () => import('@/views/test/test.vue') },
            { path: '', title: '行业分类', name: 'sys',icon:'wrench', component: () => import('@/views/test/test.vue') },
            { path: '', title: '系统设置', name: 'sys',icon:'wrench', component: () => import('@/views/test/test.vue') },
        ]
    }
]

//所有上面定义的路由都要写在下面的routers里
export const routers = [
    loginRouter,
    ...appRouter,
    mainRouter
];

