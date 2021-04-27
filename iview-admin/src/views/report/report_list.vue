<style lang="less">
    @import '../../styles/common.less';
    @import '../../styles/simplemde.min.css';

    .input-tag{ width: 80px; margin-left: 0; }
    .CodeMirror, .CodeMirror-scroll { min-height: 200px; }
</style>
<!-- 文章列表 -->
<template>
  <div>
    <Card>
      <p slot="title">
        <Icon type="pinpoint"></Icon> 行业报告         
      </p>
      <Row>
        <Input v-model="search" icon="search" @on-click="getReport" placeholder="请输入标题搜索..." style="width: 400px" />
        <router-link tag="Button" :to="{name:'reportAdd'}" class="fr ivu-btn-success" >添加</router-link>
      </Row>
      <Row class="margin-top-10 searchable-table-con1">
        <Table :columns="columns" :data="reports" border></Table>
        <Page :total="total" :page-size="10" :current="pageIndex" @on-change="changePage" show-total class="margin-top-8" />
      </Row>
    </Card>     
  </div>
</template>

<script>
import util from '@/libs/util'

const editButton = (_this,h,params) => {
    return h('Button', {
        props: { type:'primary',size:'small'},
        style: { marginRight:'5px' },
        //on:{ 'click':()=>{_this.showModal(params.row.id)} }
        on:{ 'click':()=>{ _this.$router.push({name:'reportEdit',params:{ id:params.row.id }}) } }
    }, '编辑');
};

const deleteButton = (_this,h,params) => {
    return h('Poptip', {
        props:{ confirm:true,title:'您确认要删除这条数据吗?',transfer:true },
        on:{ 'on-ok':()=>{_this.removeReport(params.row._id)} }
    }, [
        h('Button',{
            props:{ type:'error',size:'small',placement:'top'}
        },'删除')
    ]);
};

export default {
  name:'reportList',
  data () {
    const checkTitle = (rule,value,callback) => {
      let regex = /^[\u4e00-\u9fa5_a-zA-Z0-9]+$/  //let regex ="[<|>|\}|\{|$|#|\*|\'|\t|\r|\n| |-|%|@|]";
      if(!value){
        callback(new Error("请填写标题"));
      } else if(!regex.test(value)){
        callback(new Error('标题只能是数字、字母和中文组成，不能包含特殊符号和空格'));
      } else {
        callback();
      }
    }
    return {
        search:'',
        reports:[],
        total:0,
        pageIndex:1,
        columns:[
            {  
                align:'center',
                width:70,
                render:(h,params)=>{
                    return h('div', (this.pageIndex-1 ) * 10 + params.index +1)
                }
            },
            { key:'title',title:'标题',align:'center'},
            { key:'bgflName',title:'分类',width:120,align:'center'},
            {   key:'status',
                title:'状态',
                width:120,
                align:'center',
                render:(h,params) => {
                    let iColor,iText = '';
                    switch(params.row.status){
                        case 1: iColor = 'blue';      break
                        case 2: iColor = 'orange';    break
                        case 3: iColor = 'green';     break
                        case 4: iColor = 'cyan';      break
                        default : iColor = 'yellow';  break
                    }
                    return h('Tag',{ props:{ color:iColor }},params.row.statusName)
                }
            },
            { key:'publishDate',title:'发布日期',width:120,align:'center'},
            {
                // key:'_id',
                title:'操作',
                align:'center',
                width:150,
                render:(h,params) => {
                    let children = [editButton(this,h,params),deleteButton(this,h,params)];
                    return h('div',children);
                }
            }
        ],
    };
  },
  // components:{ bgfl,hyfl,ztfl },
  mounted(){
    this.getReport(this.pageIndex);
  },
  methods: {
    getReport(pageIndex){
        var _params = {
            title:this.search,
            category:'',
            pageIndex:pageIndex
        };
        var webapi = "/Api/Report/GetPageReport";       // WEBAPI:/Api/Report/GetPageReport
        //var express = "/api/admin/reportList";        // EXPRESSAPI:/api/admin/reportList
        this.$http.get(webapi,{params : _params}).then(result=>{
            this.reports = JSON.parse(result.data.data);
            this.total = result.data.total;
            this.pageIndex = result.data.pageIndex;
        }).catch(err=>{
            console.log("get report data error:"+err);
        })
    },

    changePage(index){
        this.getReport(index);
    },

    removeReport(id){
        let query  = new URLSearchParams();
        query.append('ids',id);
        this.$http.post('/Api/Report/RemoveReport',query).then(result=>{
            if(result.data.code == util.error){
              this.$Modal.error({ title:'错误', content:result.data.msg });
              return;
            }
            
            this.getReport(this.pageIndex);
        })
    },

  }
}

</script>
<style lang='less'>
    .ivu-form-item{ margin-bottom:15px; }
    .ivu-modal{ top:20px; }
    .ivu-table td, .ivu-table th { height:35px;}
</style>