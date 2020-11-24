<style lang="less">
    @import '../../styles/common.less';
    @import '../../styles/image-editor.less';
    @import '../../styles/cropper.min.css';
    @import '../../styles/simplemde.min.css';
</style>

<template>
  <div>
    <Row :gutter="10">
            <Col>
                <Card>
                    <p slot="title">
                        <Icon type="pinpoint"></Icon>
                        商品列表
                    </p>
                    <Row>
                        <Input v-model="search" icon="search" class="input" @on-change="getGoods" placeholder="请输入商品名称..." width="240" />
                        <router-link tag="Button" :to="{name:'addGoods'}" class="fr ivu-btn-success" >添加</router-link>
                    </Row>
                    <Row class="margin-top-10 searchable-table-con1">
                        <Table :columns="columns" :data="goods" border></Table>
                         <Page :total="total" :page-size="10" :current="pageIndex" @on-change="changePage" show-total class="margin-top-8" />
                    </Row>
                </Card>
            </Col>
    </Row>

    <Modal v-model="divModal" title="商品详情" width="900" @on-ok="saveGoods()" ok-text="保存" >
      <Form ref="formValidate" :model="formValidate" :rules="ruleValidate" :label-width="80" >
        <Row>
        <Col span="12">
        <FormItem label="商品名称:" prop="goodsName">
            <Input v-model="formValidate.goodsName" class="input" width="400" placeholder="请输入商品名称"></Input>
        </FormItem>
        </Col>
        <Col span="12">
        <FormItem label="规格:" prop="spec">
            <Input v-model="formValidate.spec" class="input" width="400" placeholder="请输入商品名称"></Input>
        </FormItem>
        
        </Col>
        </Row>
        <Row>
            <Col span="12">
            <FormItem label="单价:" prop="goodsPrice">
            <Input v-model="formValidate.goodsPrice" class="input" width="400" placeholder="请输入商品名称"></Input>
            </FormItem>
            </Col>
            <Col span="12">
            <FormItem label="折扣价:" prop="goodsRealPrice">
            <Input v-model="formValidate.goodsRealPrice" class="input" width="400" placeholder="请输入商品名称"></Input>
            </FormItem>
            </Col>
        </Row>

        <Row>
            <Col span="12">
            <FormItem label="商店名称:" prop="goodsPrice">
            <Input v-model="formValidate.shopName" class="input" width="400" placeholder="请输入商品名称"></Input>
            </FormItem>
            </Col>
            <Col span="12">
            <FormItem label="备注:" prop="remark">
            <Input v-model="formValidate.remark" class="input" width="400" placeholder="请输入商品名称"></Input>
            </FormItem>
            </Col>
        </Row>
    
        <Row>
            <Col span="12">
            <FormItem label="缩略图:" prop="thumb" class="image-editor" >
            <Card>
            <Row :gutter="10">
                
                <Col span="10" class="image-editor-con1">
                    <div class="cropper">
                        <img id="cropimg" :src="cpOption.imgURL" alt="源图片文件路径..">
                    </div>
                </Col>
                <Col span="14" class="image-editor-con1">
                    <Row type="flex" justify="center" align="middle" class="image-editor-con1-preview-con">
                        <div id="preview1"></div>
                    </Row>
                    <div class="image-editor-con1-btn-con margin-top-10">
                        <input type="file" accept="image/*" @change="handleChange" id="fileinput" class="fileinput" />
                        <label class="filelabel" for="fileinput"><Icon type="image"></Icon>&nbsp;选择图片</label>
                        <span><Button type="primary" icon="crop" @click="handleCrop" >裁剪</Button></span>
                    </div>
                </Col>
            </Row>
            </Card>
            </FormItem>
            </Col>
            <Col span="12">
                <FormItem label="商品分类:" prop="workCategory">
                    <Tree :data="category" show-checkbox multiple></Tree>
                </FormItem>
            </Col>
        </Row>
        
        <FormItem label="商品介绍:" prop="description">
            <Input v-model="formValidate.description" type="textarea" :autosize="{minRows: 2,maxRows: 5}" placeholder="商品描述..."></Input>
            <!-- <textarea v-model="formValidate.description" width="800" placeholder="请输入商品名称"></textarea> -->
        </FormItem>

        <FormItem label="商品详情:">
            <Row>
                <Col span="24" offset="0">
                <div class="markdown-con">
                    <Card>
                        <textarea  id="txt_markdown_editor" style="display:none;"></textarea>                
                    </Card>
                </div>
                </Col>
            </Row>
        </FormItem>
      </Form>
    </Modal>

  </div>
</template>

<script>
import util from '@/libs/util.js'
import SimpleMDE from 'simplemde'
import Cropper from 'cropperjs';

const editButton = (_this,h,params) => {
    return h('Button', {
        props: { type:'primary',size:'small'},
        style: { marginRight:'5px' },
        on:{ 'click':()=>{_this.showModal(params.row._id)} }
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
  name:'goodsList',
  data () {
    return {
        goods:[],
        total:0,
        pageIndex:1,
        divModal:false,
        editId:null, // 当前编辑的商品Id
        category:[], // 初始化分类
        cropper:'',
        cpOption:{
          showCropedImage:false,
          cropedImg:'',
          imgURL:'', // 选择图片的文件路径
          imgName:'', // 缩虐图的文件名aa.jpg
        },
        fileTags:[], // 初始化上传文件列表
        simpleMDE:'', // 富文本内容
        columns:[
            {  
                align:'center',
                width:70,
                render:(h,params)=>{
                    // return h('div',(this.pageIndex-1) * 10 + params.index +1)
                    return h('div', params.index +1)
                }
            },
            { key:'GoodsName',title:'商品名称',align:'center'},
            { key:'GoodsPrice',title:'单价',width:100,align:'center'},
            { key:'GoodsRealPrice',title:'折扣价',width:100,align:'center'},
            { key:'Spec',title:'规格',width:100,align:'center'},
            { key:'WorkCategory',title:'商品类别',align:'center'},
            {
                // key:'_id',
                title:'操作',
                align:'center',
                width:120,
                render:(h,params) => {
                    let children = [editButton(this,h,params),deleteButton(this,h,params)];
                    return h('div',children);
                }
            }
        ],
        formValidate:{
          goodsName:'',
          goodsPrice:'',
          goodsRealPrice:'',
          spec:'',
          workCategory:[],
          thumb:'',
          shopName:'',
          description:'',
          detail:'',
          remark:''
        },
        ruleValidate:{
          goodsName:[{ required:true,message:'商品名称不能为空',trigger:'blur' }],
          goodsPrice:[{ required:true,message:'商品单价不能为空',trigger:'blur' }]
        },
        search:''
    };
  },
  mounted(){
    this.getGoods();
    
    let img = document.getElementById('cropimg');
    this.cropper = new Cropper(img, {
      dragMode: 'move',
      preview: '#preview1',
      restore: false,
      center: false,
      highlight: false,
      cropBoxMovable: false,
      toggleDragModeOnDblclick: false,
      aspectRatio:6/8, // 设置裁剪框尺寸（宽：6 高：8）
    });
    this.simpleMDE = new SimpleMDE({
        element: document.getElementById('txt_markdown_editor'),
        placeholder:'请填写商品详情',
        toolbar: ['bold', 'italic', 'heading', '|', 'quote', 'unordered-list', 'clean-block', '|', 'link', 'image', 'table', 'horizontal-rule', '|', 'preview']
    })
  },
  methods: {
    getCategory(workCategory){
        this.$http.get('/api/admin/categoryList',{ params:{ group : ['hyfl'] } }).then(result=>{
            if(result.data.length>0){
                result.data.forEach(item=>{
                    let chds = [];
                    if(item.Childrens.length>0){
                        item.Childrens.forEach(chd => {
                            chds.push({ title:chd.CategoryName,checked:workCategory.indexOf(chd)>=0 })
                        })
                    }
                    this.category.push({
                        title:item.CategoryName,
                        expand:workCategory.indexOf(item.CategoryName)>=0,
                        children:chds,
                        checked: workCategory.indexOf(item.CategoryName)>=0
                    }) 
                })
            }
        })
    },
    getGoods(){
        var _params = {
            goodsName:this.search,
            pageIndex:this.pageIndex
        };
        this.$http.get('/api/admin/goodsList',{params:_params}).then(result=>{
            this.goods = result.data.data;
            this.total = result.total;
            this.pageIndex = result.pageIndex;
        }).catch(err=>{
            console.log("get goods data error:"+err);
        })
    },
    changePage(){
        this.getGoods(this.pageIndex);
    },
    removeGoods(id){
        let query  = new URLSearchParams();
        query.append('id',id);
        this.$http.post('/api/admin/removeGoods',query).then(result=>{
            if(!result){
                this.$Message.error({content:'删除失败,请重新操作',duration:3});
                return false;
            }
            this.$Message.success({ content:'删除成功',duration:10 })
            this.getGoods(this.pageIndex);
        })
    },
    showModal(id){
        console.log("editId:"+id);
        this.divModal = true;
        this.editId = id; // 当前编辑的报告Id
        this.$http.get('/api/admin/goodsDetail',{ params:{ id :id } }).then(result => {
            this.formValidate = {
                goodsName: result.data.GoodsName,
                goodsPrice:result.data.GoodsPrice,
                goodsRealPrice:result.data.GoodsRealPrice,
                spec:result.data.Spec,
                workCategory : result.data.WorkCategory? result.data.WorkCategory.split(','):[],
                thumb:result.data.Thumb,
                shopName:result.data.ShopName
            }
            if(!util.isNullOrEmpty(result.data.Thumb)) this.cpOption.imgURL = result.data.Thumb;
            this.simpleMDE.value(result.data.GoodsDetail);

            this.getCategory(this.formValidate.workCategory);
        })
    },
    saveGoods(){
        let query = new URLSearchParams();
        query.append('id',this.editId);
        query.append('goodsName',this.formValidate.goodsName);
        query.append('goodsPrice',this.formValidate.goodsPrice);
        query.append('goodsRealPrice',this.formValidate.goodsRealPrice);
        query.append('spec',this.formValidate.spec);
        query.append('workCatgory',this.formValidate.workCategory);
        query.append('thumb',this.formValidate.thumb);
        query.append('shopName',this.formValidate.shopName);
        query.append('goodsDesc',this.formValidate.description);
        query.append('goodsDetail',this.simpleMDE.value());
        query.append('remark',this.formValidate.remark);
        
        this.$http.post('/api/admin/editReport',query).then(result => {
            if(!result) return false;
            this.$Message.success({ content:'保存成功',duration:3 });
            this.divModal = false;
        })

    },
    handleChange(e){
        let file = e.target.files[0];
        this.cpOption.imgName = file.name;
        let reader = new FileReader();
        reader.onload = () => {
          this.cropper.replace(reader.result);
        };
        reader.readAsDataURL(file);
    },
    handleCrop(){
        let canvas = this.cropper.getCroppedCanvas();
        
       //通过base64上传图片
        var formData = new FormData();
        formData.append('file',canvas.toDataURL('image/jpg'));
        formData.append('fileName',this.cpOption.imgName);
        
        this.$http.post('/api/index/uploadBase64',formData).then(result =>{
            if(!result){
                this.$Message.error({ content:'图片上传失败',duration:3});
                return false;
            }
            console.log("dddd:" + JSON.stringify(result));
            this.formValidate.thumb = '/api/' + result.data;
        })
    },
    handleSuccess(res,file){
        this.formValidate.fileName.push('/api/' + res.fileName)
    }
  }
}

</script>
<style type="less">
    .ivu-modal{ top:20px; }
</style>