<style lang="less">
    @import '../styles/login.less';
</style>

<template>
    <div class="login" @keydown.enter="handleSubmit">
        <div class="login-con">
            <Card :bordered="false">
                <p slot="title">
                    <Icon type="log-in"></Icon>
                    欢迎登录
                </p>
                <div class="form-con">
                    <Form ref="loginForm" :model="form" :rules="rules">
                        <FormItem prop="userName">
                            <Input v-model="form.userName" placeholder="请输入用户名">
                                <span slot="prepend">
                                    <Icon :size="16" type="person"></Icon>
                                </span>
                            </Input>
                        </FormItem>
                        <FormItem prop="password">
                            <Input type="password" v-model="form.password" placeholder="请输入密码">
                                <span slot="prepend">
                                    <Icon :size="14" type="locked"></Icon>
                                </span>
                            </Input>
                        </FormItem>
                        <FormItem prop="verifyCode">
                            <Input v-model="form.verifyCode" placeholder="验证码">
                                <span slot="prepend">
                                    <Icon :size="14" type="locked"></Icon>
                                </span>
                                <span slot="append" class="v-code" @click="initVerifyCode">{{checkCode}}</span>
                            </Input>
                        </FormItem>
                        <FormItem>
                            <Button @click="handleSubmit" type="primary" long>登录</Button>
                        </FormItem>
                    </Form>
                    <p class="login-tip">输入手机号或者登录账号</p>
                </div>
            </Card>
        </div>
    </div>
</template>

<script>
import Cookies from 'js-cookie';
export default {
    data () {
        return {
            checkCode:'',
            form: {
                userName: Cookies.get('user'),
                password: '',
                verifyCode:''
            },
            rules: {
                userName: [
                    { required: true, message: '账号不能为空', trigger: 'blur' }
                ],
                password: [
                    { required: true, message: '密码不能为空', trigger: 'blur' }
                ],
                verifyCode :[{ required:true,message:'验证码不能为空',trigger:'blur' }]
            }
        };
    },
    mounted(){
        this.initVerifyCode();
    },
    methods: {
        initVerifyCode(){
            let code = "",codeLength =4;
            let random = new Array(0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R',
            'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z'); //随机数
            for (let i = 0; i < codeLength; i++) {
                let index = Math.floor(Math.random()*36); // 取得随机数的索引
                code += random[index];
            }
            this.checkCode = code;
        },
        checkVerifyCode(){
            if(this.form.verifyCode.toUpperCase() != this.checkCode){
                this.$Message.error({content:'验证码错误，请重新输入',type:'waring'});
                this.initVerifyCode();
                return false;
            }
            return true;
        },
        async handleSubmit () {
            this.$refs.loginForm.validate((valid) => {
                if (valid && this.checkVerifyCode()) {
                    var query = new URLSearchParams();
                    query.append("userName",this.form.userName);
                    query.append("password",this.form.password);
                    this.$http.post('/api/admin/doLogin', query).then(result => {
                        if(result.data.length <= 0){
                            this.$Message.error("账号或密码错误");
                            return;
                        }
                        Cookies.set('user', result.data[0].UserName);
                        Cookies.set('password', this.form.password);
                        this.$store.commit('setAvator',result.data[0].Avtar);
                        Cookies.set('access',0);
                        this.$router.push({name: 'home'});
                    })
                }
            });
        }
    }
};
</script>

<style>
.v-code { width: 80px; font-size: 14px; }
</style>
