<style lang="less">
    @import '../../styles/common.less';
</style>
<template description='菜单权限'>
   <div>
       <Row :gutter="10">
            <Col>
                <Card>
                    <p slot="title">
                        <Icon type="pinpoint"></Icon>
                        菜单列表
                    </p>
                    <Row>
                        <Input icon="search" placeholder="请输入标题搜索..." style="width: 400px" />
                        <Button class="fr ivu-btn-error margin-left-10" @click="$router.go(-1)">返回</Button>
                        <Button class="fr" type="success" @click="showAddModal">添加菜单</Button>
                        <!-- <router-link to>
                            <Button class="fr ivu-btn-error margin-left-10" @click="$router.go(-1)">返回</Button>
                        </router-link> -->
                        <!-- <router-link tag="Button" :to="{name:'/'}" class="fr ivu-btn-success" >添加菜单</router-link> -->
                    </Row>
                    <Row class="margin-top-10 searchable-table-con1">
                        <Table :columns="columns" :data="permission" border></Table>
                    </Row>
                </Card>
            </Col>
        </Row>
        
        <Modal v-model="divModal" title="权限" width="900" @on-ok="" ok-text="保存" >
         <Form ref="formValidate" :model="formValidate" :label-width="80" >
          <FormItem label="菜单名称" prop="title">
            <Input v-model="formValidate.title" class="input" width="400" placeholder="请输入标题"></Input>
          </FormItem>
         </Form>
        </Modal>

        <Modal v-model="addModal">
         <Form ref="formValidate" :model="formValidate" :label-width="80" >
          <FormItem label="菜单名称" prop="title">
            <Input v-model="formValidate.title" class="input" width="400" placeholder="请输入标题"></Input>
          </FormItem>
         </Form>
         <div slot="footer">
            <Button type="primary" @click="addPermission('formValidate')">保存</Button>
            <Button class="magin-left-10" @click="addModal=false">取消</Button>
         </div>
        </Modal>

   </div>
</template>
<script>
import util from '../../libs/util';
import { editButton,deleteButton,addChildButton } from '../../template/template';
 
export default {
   name: 'permissionChild',
   data() {
       return {
           permission:[],
           divModal:false,
           addModal:false,
           columns:[
               { 
                   render: (h,params) => {
                       return h('div',params.index)
                    }
               },
               { key:'title',title:'权限名称'},
               { title:'操作', align:'center',width:240,
                  render:(h,params) => {
                     let children = [editButton(this,h,params),addChildButton(this,h,params),deleteButton(this,h,params)];
                     return h('div',children);
                  }
               }
           ],
           formValidate:{
                title:''
            },
       }
   },
   mounted() {
      this.getPermission();
   },
   methods: {
      getPermission(){
          console.log("pid"+this.$route.query.pid);
          // httpGet 参数只能用params传参  httpPost 可以使用qs.stringfiy(),URLSearchParams等传参
          this.$http.get('/api/admin/permissionChilds',{ params:{ pid : this.$route.query.pid } }).then(result => {
              console.log("user list:"+result.data);
              this.permission = result.data; 
          })
      },
      showEditModal(id){
        let editPermission = this.permission.filter(item => item._id == id)[0];
        this.formValidate = {
            title:editPermission.title,
        }
        this.divModal = true;
      },
      showAddModal(){
          this.addModal = true;
      },
      addPermission(item){
          this.$refs[item].validate(valid => {
            if(!valid) {
                this.$Message.error('提交失败,请检查..');
                return;
            }
            let query = new URLSearchParams();
            query.append('title',this.formValidate.title);
            query.append('pid',this.$route.query.pid);
            this.$http.post('/api/admin/addPermission',query).then(result => {
                console.log("add"+result);
                if(result){
                    this.$Modal.success({ title:"添加菜单",content:"添加成功" })
                    this.addModal = false;
                }
            })
          })
      },
      remove(id){
         console.log("del:"+id);
      }
      
   },
   watch:{
       $route(to,from){
           console.log("to:"+to+"from:"+from);
           if(to.query.pid != from.query.pid){
               this.getPermission()
           }
       }
   }
}
</script>
<style scoped>
</style>
