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
        { path:'home',title:'主页',name:'home',component:()=>import('@/views/home/home.vue') },
        { path:'report_add',title:'添加素材',name:'addReport',component:()=>import('@/views/report/report_add.vue') },
        { path:'user_add',title:'添加用户',name:'addUser',component:()=>import('@/views/user/user_add.vue') },
        { path:'permission_child',title:'菜单管理',name:'permissionChild',component:()=>import('@/views/security/permission_child.vue') },
    ]
}

export const appRouter = [
    {
        path: '/',
        icon: 'ios-people',
        name: 'admin',
        title: '权限管理',
        component: main,
        children: [
            { path: 'user_list', title: '管理员', name: 'admins', icon: 'ios-person', component: () => import('@/views/security/user.vue') },
            { path: 'role_list', title: '角色管理', name: 'roles', icon: 'arrow-swap', component: () => import('@/views/security/role.vue') },
            { path: 'permission_list', title: '权限管理', name: 'permissions', icon: 'arrow-swap', component: () => import('@/views/security/permission.vue') }
        ]
    },
    {
        path: '/user',
        icon: 'person-stalker',
        name: 'user',
        title: '会员管理',
        component: main,
        children: [
            { path: 'user_list', title: '会员列表', name: 'users', icon: 'android-people', component: () => import('@/views/user/user_list.vue') },
            
        ]
    },
    {
        path:'/report',
        icon:'social-windows',
        name:'report',
        title:'文章管理',
        component:main,
        children:[
            { path: 'report_list', title: '文章列表', name: 'reports',icon:'social-twitch', component: () => import('@/views/report/report_list.vue') },
            { path: '/', title: '文章类型', name: 'reportCate',icon:'social-vimeo', component: () => import('@/views/report/report_list.vue') }
        ]
    },
    {
        path:'/shop',
        icon:'ios-filing',
        name:'shop',
        title:'商城管理',
        component:main,
        children:[
            { path: 'category', title: '商品分类', name: 'goodsCate', icon:'ios-pricetag', component: () => import('@/views/shop/category.vue') },
            { path: 'goods_list', title: '商品列表', name: 'goods',icon:'bag', component: () => import('@/views/shop/goods_list.vue') },
            { path: '/', title: '订单列表', name: 'orders',icon:'ios-cart', component: () => import('@/views/report/report_list.vue') }
        ]
    },
    {
        path:'/',
        icon:'clipboard',
        name:'wechat',
        title:'微信小程序',
        component:main,
        children:[
            { path: '/', title: '首页', name: 'placards',icon:'ios-compose', component: () => import('@/views/report/report_list.vue') }
        ]
    },
    {
        path:'/',
        icon:'clipboard',
        name:'placard',
        title:'公告管理',
        component:main,
        children:[
            { path: '/', title: '公告列表', name: 'placards',icon:'ios-compose', component: () => import('@/views/report/report_list.vue') }
        ]
    },
    {
        path:'/',
        icon:'ios-browsers',
        name:'comment',
        title:'评论管理',
        component:main,
        children:[
            { path: '/', title: '评论列表', name: 'comments',icon:'ios-star', component: () => import('@/views/report/report_list.vue') },
            { path: '/', title: '意见反馈', name: 'suggests',icon:'ios-flag', component: () => import('@/views/report/report_list.vue') }
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
            { path: '', title: '报表列表', name: 'sources',icon:'document-text', component: () => import('@/views/test/test.vue') },
        ]
    },
    {
        path:'/',
        icon:'ios-pricetag',
        name:'tag',
        title:'标签管理',
        component:main,
        children:[
            { path: '', title: '标签列表', name: 'tags',icon:'pricetag', component: () => import('@/views/test/test.vue') },
        ]
    },
    {
        path:'/',
        icon:'settings',
        name:'system',
        title:'系统设置',
        component:main,
        children:[
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

