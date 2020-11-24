<style lang="less">
    @import '../../styles/common.less';
</style>

<template description='角色管理'>
   <div>
       <Row :gutter="10">
            <Col>
                <Card>
                    <p slot="title">
                        <Icon type="pinpoint"></Icon>
                        角色列表
                    </p>
                    <Row>
                        <Input icon="search" placeholder="请输入标题搜索..." style="width: 400px" />
                        <router-link tag="Button" :to="{name:'addReport'}" class="fr ivu-btn-success" >添加</router-link>
                    </Row>
                    <Row class="margin-top-10 searchable-table-con1">
                        <Table :columns="columns" :data="roles" border></Table>
                    </Row>
                </Card>
            </Col>
        </Row>

        <Modal v-model="editModal" title="更改角色权限" width="900">
         <Form ref="formValidate" :model="formValidate" :label-width="80" >
          <FormItem label="角色名称" prop="title">
            <Input v-model="formValidate.RoleName" class="input" width="400" placeholder="请输入标题"></Input>
          </FormItem>
          
          <FormItem label="权限分配" prop="title">
            <Tree :data="pData" ref="menuTree" show-checkbox  style="height:200px;width:240px;overflow-y:scroll;"></Tree>
          </FormItem>
         </Form>
         <div slot="footer">
            <Button type="primary" @click="editRole('formValidate')">保存</Button>
            <Button class="magin-left-10" @click="editModal=false">取消</Button>
         </div>
        </Modal>
   </div>
</template>
<script>
import { editButton,deleteButton } from '../../template/template';

export default {
   name: 'roles',
   data() {
       return {
           roles:[],
           editModal:false,
           columns:[
               { 
                   render: (h,params) => {
                       return h('div',params.index)
                    }
               },
               { key:'Name',title:'角色名称'},
               { title:"编辑",align:'center',width:150,
                 render : (h,params) => {
                     let children = [editButton(this,h,params),deleteButton(this,h,params)];
                     return h('div',children);
                 }
               }
           ],
           formValidate:{
               RoleId:null,
               RoleName:''
           },
           pData:[],
           cntRolePermission:[]
       }
   },
  mounted() {
      this.getRole();
  },
  methods: {
    getRole(){
        // httpGet 参数只能用params传参  httpPost 可以使用qs.stringfiy(),URLSearchParams等传参
        this.$http.get('/api/admin/roleList').then(result => {
            console.log("role list:"+result.data);
            this.roles = result.data;
        })
    },
    showEditModal(id){
        let role = this.roles.filter(item => item._id == id)[0];
        if(role == null) return;
        this.formValidate.RoleName = role.Name;
        this.formValidate.RoleId = role._id;
        this.getCurrentRolePermission(id);
        this.getPermission();
        this.editModal = true;
    },
    editRole(){
        console.log("checkNodes:" + this.$refs.menuTree.getCheckedNodes());
        let checkedNodes = this.$refs.menuTree.getCheckedNodes();
        if(checkedNodes == this.cntRolePermission) return;  // 若没有修改
        let checkedArr = [];
        if(checkedNodes.length>0){
            checkedNodes.forEach(item => {
                checkedArr.push(item.title);
            })
        }
        var params = new URLSearchParams();
        params.append("roleId",this.formValidate.RoleId);
        params.append("roleName",this.formValidate.RoleName);
        params.append("permission",checkedArr.toString());
        this.$http.post('/api/admin/addRolePermission',params).then(result => {
            this.$Modal.success({ title : '修改权限', content :'修改成功' })
            this.editModal = false;
        })
    },
    getPermission(){
        this.$http.get('/api/admin/permissionList').then(result => {
            console.log("permission"+result.data);
            let pdata = [];
            if(result.data.length <= 0) return;
            result.data.forEach(item => {
                if(item.pid == 0){
                    pdata.push({
                        title:item.title,
                        id:item._id,
                        checked:this.cntRolePermission.indexOf(item.title) >= 0 ,
                        children:this.getchildItems(item._id,result.data)
                    })
                }
            })
            console.log("pdata:"+ JSON.stringify(pdata));
            this.pData = pdata;
        })
    },
    getchildItems(id,arr){
        let cData = [];
        let cItem = arr.filter(item => item.pid == id );
        cItem.forEach(child => {
            cData.push({
                title:child.title,
                id:child._id,
                checked : this.cntRolePermission.indexOf(child.title) >= 0,
                children:this.getchildItems(child._id,arr)
            })
        })
        return cData;
    },
    getCurrentRolePermission(roleId){
        this.$http.get('/api/admin/currentRolePermission',{ params : { roleId :roleId } }).then(result => {
            console.log("dd"+JSON.stringify(result))
            if(result.data.length>0){
                result.data.forEach(item => {
                    this.cntRolePermission.push(item.Permission)
                })
            }else{
                this.cntRolePermission = [];
            }
        })
    }
    
  }
}
</script>
<style scoped>
</style>
