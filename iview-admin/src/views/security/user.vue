<style lang="less">
    @import '../../styles/common.less';
</style>
<template description='登录账户'>
   <div>
       <Row :gutter="10">
            <Col>
                <Card>
                    <p slot="title">
                        <Icon type="pinpoint"></Icon>
                        登录账户
                    </p>
                    <Row>
                        <Input icon="search" placeholder="请输入标题搜索..." style="width: 400px" />
                        <router-link tag="Button" :to="{name:'/'}" class="fr ivu-btn-success" >添加</router-link>
                    </Row>
                    <Row class="margin-top-10 searchable-table-con1">
                        <Table :columns="columns" :data="users" border></Table>
                    </Row>
                </Card>
            </Col>
        </Row>
        
        <Modal v-model="divModal" title="登录用户详情" width="900" @on-ok="" ok-text="保存" >
         <Form ref="formValidate" :model="formValidate" :label-width="80" >
          <FormItem label="登录账号" prop="title">
            <Input v-model="formValidate.UserCode" class="input" width="400" placeholder="请输入标题"></Input>
          </FormItem>

          <FormItem label="登录名称" prop="title">
            <Input v-model="formValidate.UserName" class="input" width="400" placeholder="请输入标题"></Input>
          </FormItem>
        
          <FormItem label="角色" prop="title">
            <CheckboxGroup v-model="formValidate.RoleName" >
                <Checkbox v-for="role,index in roles" :label="role.Name" :key="index" border>{{ role.Name }}</Checkbox>
            </CheckboxGroup>
          </FormItem>

         </Form>
        </Modal>

   </div>
</template>
<script>
import util from '../../libs/util';
import { editButton,deleteButton } from '../../template/template';
 
export default {
   name: 'admins',
   data() {
       return {
           users:[],
           divModal:false,
           roles:[],
           columns:[
               { 
                   render: (h,params) => {
                       return h('div',params.index)
                    }
               },
               { key:'UserCode',title:'登录账号'},
               { key:'UserName',title:'用户名称'},
               { key:'RoleNames',title:'角色',
                 render:(h,params) => {
                     if(util.isNullOrEmpty(params.row.RoleNames)) return;
                     let tags = [];
                     params.row.RoleNames.split(',').forEach(item => {
                         tags.push(
                             h('Tag',{ props : { type:'border',color:'success'} },item)
                         )
                     })
                     return h('div',tags);
                 }
               },
               { title:'操作', align:'center',width:150,
                  render:(h,params) => {
                     let children = [editButton(this,h,params),deleteButton(this,h,params)];
                     return h('div',children);
                  }
               }
           ],
           formValidate:{
                UserCode:'',
                UserName:'',
                RoleName:[]
            },
       }
   },
   mounted() {
      this.getUser();
      this.getRoleList();
   },
   methods: {
      getUser(){
          // httpGet 参数只能用params传参  httpPost 可以使用qs.stringfiy(),URLSearchParams等传参
          this.$http.get('/api/admin/accountList').then(result => {
              console.log("user list:"+result.data);
              this.users = result.data;
          })
      },
      getRoleList(){
        this.$http.get('/api/admin/roleList').then(result => {
            console.log("role list" + result.data);
            this.roles = result.data;
        })
      },
      showEditModal(uid){
        let editUser = this.users.filter(item => item._id == uid)[0];
        this.formValidate = {
            UserCode:editUser.UserCode,
            UserName:editUser.UserName,
            RoleName: util.isNullOrEmpty(editUser.RoleNames) ? [] : editUser.RoleNames.split(',')
        }
        this.divModal = true;
      },
      remove(id){
         console.log("del:"+id);
      }

   },
}
</script>
<style scoped>
</style>
