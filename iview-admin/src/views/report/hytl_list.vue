<style lang="less">
    @import '../../styles/common.less';
</style>

<template desc="行业题录">
  <div>
    <Card>
        <p slot="title">
            <Icon type="pinpoint"></Icon> 行业报告题录              
        </p>
        <Form :label-width="80">
            <Row>
            <Col span="8">
                <FormItem label="行业分类:">
                    <Select placeholder="请选择行业"></Select>
                </FormItem>
            </Col>        
            <Col span="8">
                <FormItem label="题名:">
                    <Input v-model="search" icon="search" @on-click="getDirectory()" placeholder="请输入题名搜索..." />
                </FormItem>
            </Col>        
            <Col span="8">
                <Button type="warning" class="fr" @click="sendDirectory" >批量发送</Button>
                <Button type="info" class="fr margin-right-10">自动添加</Button>
                <router-link tag="Button" :to="{name:'hytlAdd'}" class="fr ivu-btn-success margin-right-10" >添加</router-link>
            </Col>
            </Row>
        </Form>
        <Row class="margin-top-10 searchable-table-con1">
            <Table :columns="columns" :data="tlReport" border ref="selection" @on-select="doSelect" @on-select-cancel="doCancel" @on-select-all="doSelectAll" @on-select-all-cancel="doSelectAll"></Table>
            <Page :total="total" :page-size="10" :current="pageIndex" @on-change="changePage" show-total class="margin-top-8" />
        </Row>
    </Card>

    <Modal v-model="divPreviewModal" title="预览" width="1000">
      <tlPreview :id="id"></tlPreview>
    </Modal>

  </div>
</template>

<script>
import tlPreview from './hytl_preview';
import util from '../../libs/util';

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
        on:{ 'click':()=>{ _this.$router.push({ name:'hytlEdit',query:{ id:params.row.id}})} }
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
    name:'hytlList',
    data(){
        return {
            id:null,
            selectedIds:new Set(),                      // 已选中的id
            tlReport:[],
            pageIndex:1,
            total:0,
            search:'',
            divPreviewModal:false,
            columns:[
            {  align:'center', type:'selection' , title:'全选', width:50, render:(h,params)=>{
                    return h('Checkbox');
                    // return h('Checkbox',{ on:{ 'on-change':()=>{ alert(params.row.id) } } });
                }
            },
            {  align:'center', width:70, render:(h,params)=>{
                    return h('div', (this.pageIndex-1 ) * 10 + params.index +1)
                }
            },
            { key:'title',title:'题名',align:'center'},
            { key:'hyfl',title:'行业分类',width:120, align:'center' },
            { key:'publishDate',title:'发布日期',width:120, align:'center' },
            { title:'操作', align:'center', width:180, render:(h,params) => {
                    let children = [previewButton(this,h,params),editButton(this,h,params),deleteButton(this,h,params)];
                    return h('div',children);
                }
            }]
        }
    },
    components:{ tlPreview },
    mounted(){
        this.getDirectory(this.pageIndex);
    },
    methods:{
        getDirectory(pageIndex){
            var params = { pageIndex : pageIndex }
            this.$http.get('/Api/Directory/GetPageDirectory',{ params : params }).then(result => {
                if(result.data.code == util.error) return;
                let dItem = JSON.parse(result.data.data)
                this.tlReport = dItem;
                this.total = result.data.total;
            })
        },

        changePage(index){
            this.getDirectory(index);
        },

        doSelect(selection,row){
            this.selectedIds.add(row.id);
        },

        doCancel(selection,row){
            this.selectedIds.delete(row.id);
        },

        doSelectAll(selection){
            if(selection && selection.length == 0 ){
                var dItems = this.$refs.selection.data;
                dItems.forEach((item)=>{
                    if(this.selectedIds.has(item.id)){
                        this.selectedIds.delete(item.id)
                    }
                })
            }else{
                selection.forEach((item)=>{
                    this.selectedIds.add(item.id);
                })
            }
        },

        sendDirectory(){
            console.log(this.selectedIds);
        },
        showPreviewModal(id){
            this.divPreviewModal = true;
            this.id = id;
        }
        
    }
}
</script>
