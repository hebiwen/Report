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
                        <Button type="success" class="fr" @click="showAddModal">添加菜单</Button>
                    </Row>
                    <Row class="margin-top-10 searchable-table-con1">
                        <Table :columns="columns" :data="permission" border></Table>
                    </Row>
                </Card>
            </Col>
        </Row>
        
        <Modal v-model="divModal" title="编辑菜单" width="400" @on-ok="" ok-text="保存" >
         <Form ref="formValidate" :model="formValidate" :label-width="80" >
          <FormItem label="菜单名称" prop="title">
            <Input v-model="formValidate.title" class="input" width="400" placeholder="请输入菜单名称"></Input>
          </FormItem>
         </Form>
        </Modal>

        <Modal v-model="addModal" title="添加菜单" width="800" >
          <Form ref="formValidate" :model="formValidate" :rules="ruleValidate" :label-width="80" >
          <FormItem label="菜单名称" prop="title">
            <Input v-model="formValidate.title" class="input" placeholder="请输入菜单名称"></Input>
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
   name: 'permissions',
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
               { key:'title',title:'菜单名称'},
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
            ruleValidate:{
                title:[{ required :true, message: '菜单名称不能为空', trigger: 'blur' }]
            }
       }
   },
   mounted() {
      this.getPermission();
     
   },
   methods: {
      getPermission(){
          // httpGet 参数只能用params传参  httpPost 可以使用qs.stringfiy(),URLSearchParams等传参
          this.$http.get('/api/admin/permissionChilds').then(result => {
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
            this.$http.post('/api/admin/addPermission',query).then(result => {
                console.log("add"+result);
                if(result)
                {
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
}
</script>
<style scoped>
</style>
