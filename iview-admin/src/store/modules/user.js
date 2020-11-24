import Cookies from 'js-cookie'

const user = {
    state:{},
    mutations:{
        logout(state,vm){
            Cookies.remove('user');
            Cookies.remove('password');
            Cookies.remove('access');
            // 恢复默认样式
            localStorage.clear();
        }
    }
}

export default user;
