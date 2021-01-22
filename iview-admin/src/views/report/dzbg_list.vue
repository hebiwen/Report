<style lang="less">
    @import '../../styles/common.less';
</style>

<template desc="定制报告">
  <div>
    <Card>
        <p slot="title">
            <Icon type="pinpoint"></Icon> 定制报告              
        </p>
        <Form label-width="80">
            <Row>
            <Col span="8">
                <FormItem label="报告分类:">
                    <Select placeholder="请选择分类"></Select>
                </FormItem>
            </Col>        
            <Col span="8">
                <FormItem label="题名:">
                    <Input v-model="search" icon="search" @on-click="getDirectory()" placeholder="请输入题名搜索..." />
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

    <Modal v-model="divPreviewModal" title="预览" width="800">
      <tlPreview :id="id"></tlPreview>
    </Modal>

  </div>
</template>

<script>
import tlPreview from './hytl_preview';

const previewButton = (_this,h,params) => {
    return h('Button', {
        props: { type:'info',size:'small'},
        style: { marginRight:'5px' },
        on:{ 'click':()=>{_this.showPreviewModal(params.row.id)} }
    }, '预览');
};

const editButton = (_this,h,params) => {
    return h('Button', {
        props: { type:'primary',size:'small'},
        style: { marginRight:'5px' },
        on:{ 'click':()=>{_this.$router.push({ name:'hytlAdd',query:{ id:params.row.id}})} }
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
    name:'hytl',
    data(){
        return {
            id:null,
            dzReport:[],
            pageIndex:1,
            total:0,
            search:'',
            divPreviewModal:false,
            columns:[
            {  align:'center', width:70, render:(h,params)=>{
                    return h('div', (this.pageIndex-1 ) * 10 + params.index +1)
                }
            },
            { key:'title',title:'题名',align:'center'},
            { key:'hyfl',title:'报告分类',align:'center' },
            { key:'publishDate',title:'发布日期',align:'center' },
            { title:'操作', align:'center', width:200, render:(h,params) => {
                    let children = [previewButton(this,h,params),editButton(this,h,params),deleteButton(this,h,params)];
                    return h('div',children);
                }
            }]
        }
    },
    components:{ tlPreview },
    mounted(){
        this.getDirectory();
    },
    methods:{
        getDirectory(){
            var params = {
                pageIndex :1
            }
            this.$http.get('/Api/Directory/GetPageDirectory',{ params : params }).then(result => {
                if(result.data.code != 0) return;
                let dItem = JSON.parse(result.data.data)
                this.tlReport = dItem;
                this.total = result.data.total;
            })
        },
    
    }
}
</script>
