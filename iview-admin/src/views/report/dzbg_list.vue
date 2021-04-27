<style lang="less">
    @import '../../styles/common.less';
</style>

<template desc="定制报告">
  <div>
    <Card>
        <p slot="title">
            <Icon type="pinpoint"></Icon> 定制报告              
        </p>
        <Form :label-width="80">
            <Row>
            <Col span="8">
                <FormItem label="报告分类:">
                    <Select placeholder="请选择分类"></Select>
                </FormItem>
            </Col>        
            <Col span="8">
                <FormItem label="题名:">
                    <Input v-model="search" icon="search" @on-click="GetPageDZReport()" placeholder="请输入题名搜索..." />
                </FormItem>
            </Col>        
            <Col span="8">
                <router-link tag="Button" :to="{name:'dzbgAdd'}" class="fr ivu-btn-success margin-right-10" >添加</router-link>
            </Col>
            </Row>
        </Form>
        <Row class="margin-top-10 searchable-table-con1">
            <Table :columns="columns" :data="dzReport" border></Table>
            <Page :total="total" :page-size="10" :current="pageIndex" @on-change="changePage" show-total class="margin-top-8" />
        </Row>
    </Card>
  </div>
</template>

<script>
import util from '../../libs/util';
const previewButton = (_this,h,params) => {
    return h('Button', {
        props: { type:'info',size:'small',target:"_blank",to:params.row.filePath},
        style: { marginRight:'5px' },
        // on:{ 'click':()=>{_this.showPreviewModal(params.row.id)} }
    }, '预览');
};

const editButton = (_this,h,params) => {
    return h('Button', {
        props: { type:'primary',size:'small'},
        style: { marginRight:'5px' },
        on:{ 'click':()=>{_this.$router.push({ name:'dzbgEdit',query:{ id:params.row.id}})} }
    }, '编辑');
};

const deleteButton = (_this,h,params) => {
    return h('Poptip', {
        props:{ confirm:true,title:'您确认要删除这条数据吗?',transfer:true },
        on:{ 'on-ok':()=>{_this.removeReport(params.row.id)} }
    }, [
        h('Button',{
            props:{ type:'error',size:'small',placement:'top'}
        },'删除')
    ]);
};

export default {
    name:'dzbgList',
    data(){
        return {
            dzReport:[],
            pageIndex:1,
            total:0,
            search:'',
            columns:[
            {  align:'center', width:70, render:(h,params)=>{
                    return h('div', (this.pageIndex-1 ) * 10 + params.index +1)
                }
            },
            { key:'title',title:'题名',align:'center'},
            { key:'cName',title:'报告分类',width:120,align:'center' },
            { key:'hyfl',title:'行业分类',width:140,align:'center' },
            { key:'price',title:'单价',width:80,align:'center' },
            { key:'previewPage',title:'可预览页数',width:100,align:'center' },
            { title:'操作', align:'center', width:180, render:(h,params) => {
                    let children = [previewButton(this,h,params),editButton(this,h,params),deleteButton(this,h,params)];
                    return h('div',children);
                }
            }]
        }
    },
    components:{  },
    mounted(){
        this.getDZReport();
    },
    methods:{
        getDZReport(){
            var params = { pageIndex :this.pageIndex,title:'' }
            this.$http.get('/Api/DZReport/GetPageDZReport',{ params : params }).then(result => {
                if(result.data.code == util.error ) return;
                let dItem = JSON.parse(result.data.data)
                this.dzReport = dItem;
                this.total = result.data.total;
            })
        },
        changePage(){

        },
        removeReport(id){
            let query = new URLSearchParams();
            query.append("ids",id);
            this.$http.post("/Api/DZReport/RemoveReport",query).then(result => {
                if(result.data.code == util.error) {
                    this.$Message.error({ content:result.msg, duration:3}); return
                }
                this.getDZReport();
            });
        }
    }
}
</script>
