<style lang="less">
    @import '../../styles/common.less';
</style>

<!--用户列表-->
<template>
  <div>
    <Row :gutter="10">
            <Col span="24">
                <Card>
                    <p slot="title">
                        <Icon type="pinpoint"></Icon>
                        会员列表
                    </p>
                    <Row>
                        <Col span="3">
                            <Tree :data="deptTree" @on-select-change="OnSelectChange" ref="treeNodes" ></Tree>
                        </Col>
                        <Col span="21">
                            <Row>
                                <Input icon="search" v-model="search" @on-change="getUserlist()" placeholder="请输入姓名搜索..." style="width:400px" />
                                <router-link tag="Button" :to="{name:'addUser'}" class="fr ivu-btn-success margin-left-10" >添加</router-link>
                                <Button type="primary" class="fr margin-left-10">导入</Button>
                                <Button type="warning" class="fr" @click="exportToExcel()" >导出</Button>
                            </Row>
                            <Row class="margin-top-10 searchable-table-con1">
                                <Table :columns="columns" :data="userList"></Table> 
                                <Page :total="total" :page-size="10" :current="pageIndex" @on-change="changePage" show-total class="margin-top-8" />
                            </Row>
                        </Col>
                    </Row>
                </Card>
            </Col>
    </Row>

    <Modal v-model="divModal" title="编辑" width="750" @on-ok="saveUser()" ok-text="保存" >
      <Form ref="formInline" :model="formValidate" :rules="ruleValidate" :label-width="80" inline>
        <Input v-model="formValidate.id" style="display:none" ></Input>
        <Row>
            <Col span="12">
                <FormItem label="姓名" prop="userName">
                    <Input v-model="formValidate.userName" placeholder="Enter your name"></Input>
                </FormItem>
            </Col>
            <Col span="12">
                <FormItem label="部门" prop="dept">
                    <Tag v-for="item in formValidate.dept" :key="item.deptName" :name="item.deptName" closable @on-close="handleClose">{{ item.deptName }}</Tag>

                    <Button icon="ios-add" type="dashed" size="small" @click="handleAdd">添加标签</Button>
                    <!-- <Input v-model="formValidate.dept" placeholder="Enter your name"></Input> -->
                </FormItem>
            </Col>
        </Row>
        
        <Row>
            <Col span="12">
                <FormItem label="登录名" prop="userId">
                    <Input v-model="formValidate.userId" placeholder="Enter your name"></Input>
                </FormItem>
            </Col>
            <Col span="12">
                <FormItem label="密码" prop="password">
                    <Input v-model="formValidate.password" placeholder="Enter your name"></Input>
                </FormItem>
            </Col>
        </Row>

        <Row>
            <Col span="12">
                <FormItem label="手机号" prop="mobile">
                    <Input v-model="formValidate.mobile" placeholder="Enter your name"></Input>
                </FormItem>
            </Col>
            <Col span="12">
                <FormItem label="邮箱" prop="email">
                    <Input v-model="formValidate.email" placeholder="Enter your name"></Input>
                </FormItem>
            </Col>
        </Row>

      </Form>
    </Modal>

    <Modal v-model="divDeptModal" title="部门标签" width="600" ok-text="确认" >
      <Form ref="formInline" :model="formValidate"  :label-width="80" inline>
        <Row>
            <Col span="6">
                <Tree :data="deptTree" @on-select-change="OnSelectChangeTags" ref="treeTags" ></Tree>
            </Col>
            <Col span="2"></Col>
            <Col span="16">
                <FormItem label="部门" prop="dept">
                    <div style="border:1px solid #ddd;width:320px;">
                        <Tag v-for="item in formValidate.dept" :key="item.deptName" :name="item.deptName" closable @on-close="handleClose" type="dot" color="success" >{{ item.deptName }}</Tag>
                    </div>
                </FormItem>
            </Col>
        </Row>
      </Form>
    </Modal>
  </div>
</template>


<script>
import util from '@/libs/util.js';
import blob from '../../libs/excel/Blob.js'
import export2Excel from '../../libs/excel/Export2Excel.js'

export default {
  name:'admin',
  data () {
    return {
      pageIndex:1,
      total:0,
      userList:[], // 用户列表
      exportList:[], 
      deptTree:[],
      search:'', // 搜索框,
      deptCode:'', // 点击部门树时查询的部门Code
      columns:[
        { 
            align:'center',
            width:50,
            render:(h,params)=>{
                return h('div', params.index+1);
            }
        },
        { 
            key:'Avtar',
            title:'头像',
            width:80,
            align:'center',
            render:(h,params)=>{
                return h('div',[
                    h('Avatar',{
                        props:{ shape:'circle',src:params.row.Avtar}
                    })
                ])
            }
        },
        { key:'UserId',title:'账号',align:'center' },
        { key:'UserName',title:'姓名',align:'center',width:100 },
        { key:'DeptName',title:'部门',align:'center' },
        { key:'Mobile',title:'手机',align:'center' },
        { 
            key:'CreateDate',
            title:'创建日期',
            width:140 ,
            align:'center',
            render:(h,params)=>{
                let date = params.row.CreateDate;
                if(!date) return;
                return h('div',util.formatDate(new Date(date),'yyyy-MM-dd hh:mm'))
            }
        },
        { 
            key:'_id',
            title:'操作',
            align:'center',
            render:(h,params)=>{
                return h('div',[
                    h('Button',{ 
                        props:{ type:'primary',size:'small'},
                        style:{ marginRight:'5px' },
                        on:{ click:()=>{this.showModal(params.row._id)} }
                    },'编辑'),
                    h('Button',{ 
                        props:{ type:'error',size:'small'},
                        on:{ click:()=>{this.removeUser(params.row._id)} } 
                    },'删除')
                    ])
            }
        }
        ],

        /* 编辑相关 */
        divModal:false,
        divDeptModal:false,
        formValidate: {
            id:'',
            userName: '',
            // dept: [],
            // deptName:[],
            dept:[], // JsonData [{deptCode:'12.1',deptName:'测试'}]
            password: '',
            mobile: '',
            email: '',
            userId: '',
        },
        ruleValidate: {
            userName: [
                        { required: true, message: '姓名不能为空', trigger: 'blur' }
                    ],
            dept: [
                        { required: true, message: '部门不能为空', trigger: 'change' }
                    ] ,
            email: [
                        { required: true, message: '邮箱不能为空', trigger: 'blur' },
                        { type: 'email', message: '邮箱格式不正确', trigger: 'blur' }
                    ],
            userId: [
                        { required: true, message: '登录名不能为空', trigger: 'change' }
                    ],
            password: [
                        { required: true, message: '密码不能为空', trigger: 'change' }
                    ],
            mobile: [
                        { required: true, message: '手机号不能为空', trigger: 'change' }
                    ]       
        }
            
    };
  },
  mounted(){
     this.getUserlist(this.pageIndex);

     this.getDeptTree();
  },

  methods: {
    /* 获取部门树 */
    getDeptTree(){
        this.$http.get('/api/admin/dept').then(result=>{
            if(result.data == null) return;
            this.deptTree = result.data;
        }).catch(err=>{
            console.log("get dept data error:",err);
        })
    },

    /* 点击部门树时查询该部门的员工 */
    OnSelectChange(itemNode){
        console.log("click tree:"+ itemNode);
        let node = itemNode[itemNode.length-1];
        this.deptCode = node == null ? "1" : node.DeptCode;
        this.getUserlist(this.pageIndex)
    },

    /* 获取员工分页数据 */
    getUserlist(pageIndex){
            var _params = { 
                pageIndex:pageIndex,
                search:this.search,
                deptCode:this.deptCode
            };
            this.$http.get('/api/admin/userList',{params:_params}).then(result=>{
                this.userList = result.data.data;
                this.exportList = result.data.data;
                this.total = result.data.total;
                this.pageIndex = pageIndex;
            }).catch(err=>{
                console.log("get accountInfo data error:",err);
            })
    },

    /* 分页按钮 */
    changePage(index){
        this.getUserlist(index);
    },

    /* 显示员工模态框 */
    showModal(id){
        this.divModal = true;
        this.$http.get('/api/admin/userDetail',{params:{id:id}}).then(result=>{
            let dptJson = this.ArrayParseJson(result.data.DeptCode.split(','),result.data.DeptName.split(','));
            
            this.formValidate = {
                id:result.data._id,
                userName: result.data.UserName,
                dept:dptJson,
                // dept: result.data.DeptCode.split(','),
                // deptName:result.data.DeptName.split(','),
                password: result.data.Password,
                mobile: result.data.Mobile,
                email: result.data.Email,
                userId: result.data.UserId,
            }
        })
    },

    saveUser(){
        var qs = require('qs');
        let deptJson = this.formValidate.dept;
        let dptCodes = [],dptNames =[];
        for(let i = 0; i<deptJson.length; i++)
        {
            dptCodes[i] = deptJson[i].deptCode;
            dptNames[i] = deptJson[i].deptName;
        }

        var query = {
            id:this.formValidate.id,
            userName:this.formValidate.userName,
            // dept:this.formValidate.dept.toString(),
            // deptName:this.formValidate.deptName.toString(),
            dept:dptCodes.toString(),
            deptName:dptNames.toString(),
            userId:this.formValidate.userId,
            password:this.formValidate.password,
            mobile:this.formValidate.mobile,
            email:this.formValidate.email
        }

        this.$http.post('/api/admin/editUser',qs.stringify(query)).then(result=>{
            if(!result) {
                this.$Message.error({content:'保存失败,请重新提交',duration:3});
                return;
            }
            this.$Message.success({content:'保存成功',duration:3});
            this.divModal = false;
            this.getUserlist(this.pageIndex);
        })
    },

    /* 删除用户 */
    removeUser(id){
        let query = new URLSearchParams();
        query.append('id',id);
        this.$http.post('/api/admin/removeUser',query).then(result=>{
            if(!result){
                this.$Message.error({content:'删除失败,请重新提交',duration:3});
                return;
            }
            this.$Message.success({content:'删除成功',duration:3});
            this.getUserlist(this.pageIndex);
        })
    },

    /* 导出Excel */
    exportToExcel(){
        // this.getUserlist(1,999); 要抓取全部的数据
    
        require.ensure([], () => {
        const { export_json_to_excel } = require('../../libs/excel/Export2Excel'); //这里必须使用绝对路径
        const tHeader = ['账号', '姓名', '部门', '手机', '创建日期']//表头信息
        const filterVal = ['UserId', 'UserName', 'Department', 'Mobile','CreateDate']//对应表头的字段名
        const list = this.exportList;
        const data = this.formatJson(filterVal, list);
        export_json_to_excel(tHeader, data, '会员信息表');// 导出的表格名称，根据需要自己命名
      })
    },
    formatJson(filterVal, jsonData){
     	return jsonData.map(v =>{
    	  return filterVal.map(j => v[j])
        })
     },

     /* 弹出框中部门标签 */
    handleAdd () {
        this.divDeptModal = true;
        // if (this.formValidate.dept.length) {
        //     this.formValidate.dept.push(this.formValidate.dept[this.formValidate.dept.length - 1] + 1);
        // }
    },
    handleClose (event, name) {
        var deptJson = this.formValidate.dept;
        if(deptJson.length<=0) return;
        for(let i = 0 ; i < deptJson.length; i++)
        {
            if(deptJson[i].deptName == name)
            {
                deptJson.splice(i,1);
            }
        }
        // const index = this.formValidate.dept.indexOf(name);
        // this.formValidate.dept.splice(index, 1);
    },

    OnSelectChangeTags(itemNode){
        let node = itemNode[itemNode.length-1];
        console.log(node);
        // this.formValidate.dept.push(node.DeptCode);
        // this.formValidate.deptName.push(node.title);
        var nodeItem = {deptCode:node.DeptCode,deptName:node.title};
        this.formValidate.dept.push(nodeItem);
    },
    handleClose2 (event, name) {
        //移除Name的同时也要移除Code
        console.log("event:"+event+"name:"+name);
        // const idxDeptName = this.formValidate.deptName.indexOf(name);
        // this.formValidate.deptName.splice(idxDeptName, 1);
        var deptJson = this.formValidate.dept;
        if(deptJson.length<=0) return;


    },
    ArrayParseJson(...arrList){
        const result = [];
        for(let i = 0,l = arrList.length,arr,key; i<l;i++){
            arr = arrList[i];
            key = ['deptCode','deptName'];
            for(let j = 0, l = arr.length; j<l;j++){
                if(!result[j]) result[j] = {};
                result[j][key[i]] = arr[j];
            }
        }
        return result;
    }

  }
}

</script>
