<style lang="less">
    @import '../../styles/common.less';
    @import '../../styles/image-editor.less';
    @import '../../styles/cropper.min.css';
    @import '../../styles/simplemde.min.css';
   
    @import 'quill/dist/quill.snow.css';
</style>
<!-- 文章列表 -->
<template>
  <div>
    <Row :gutter="10">
            <Col>
                <Card>
                    <p slot="title">
                        <Icon type="pinpoint"></Icon>
                        行业报告
                    </p>
                    <Row>
                        <Input v-model="search" icon="search" @on-change="getReport" placeholder="请输入标题搜索..." style="width: 400px" />
                        <router-link tag="Button" :to="{name:'addReport'}" class="fr ivu-btn-success" >添加</router-link>
                    </Row>
                    <Row class="margin-top-10 searchable-table-con1">
                        <Table :columns="columns" :data="reports" border></Table>
                         <Page :total="total" :page-size="10" :current="pageIndex" @on-change="changePage" show-total class="margin-top-8" />
                    </Row>
                </Card>
            </Col>
    </Row>

    <Modal v-model="divModal" title="文章详情" width="900" @on-ok="saveReport()" ok-text="保存" >
      <Form ref="formValidate" :model="formValidate" :rules="ruleValidate" :label-width="80" >
        <FormItem label="标题：" prop="title">
            <Input v-model="formValidate.title" class="input" width="400" placeholder="请输入标题"></Input>
        </FormItem>

        <FormItem label="分类：" prop="category">
            <CheckboxGroup v-model="formValidate.category">
                <Checkbox label="文学"></Checkbox>
                <Checkbox label="历史"></Checkbox>
                <Checkbox label="教育"></Checkbox>
            </CheckboxGroup>
        </FormItem>

        <FormItem label="缩略图：" prop="thumb" class="image-editor" >
            <Card>
            <Row :gutter="10">
                
                <Col span="8" class="image-editor-con1">
                    <div class="cropper">
                        <img id="cropimg" :src="cpOption.imgURL" alt="源图片文件路径..">
                    </div>
                </Col>
                <Col span="6" class="image-editor-con1">
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
        <FormItem label="文件">
            <Upload ref="upload" action="/api/admin/uploadArray" name="files" :show-upload-list="true" :default-file-list="fileTags" :on-success="handleSuccess" multiple >
              <Button icon="ios-cloud-upload-outline">上传文件</Button>
            </Upload> 
        </FormItem>

        <FormItem label="摘要">
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

        <FormItem label="Quill">
            <quill-editor ref="quill_editor" :options="quillOption" ></quill-editor>
        </FormItem>
      </Form>
    </Modal>

  </div>
</template>

<script>
import util from '@/libs/util.js'
import SimpleMDE from 'simplemde'
import Cropper from 'cropperjs'
import {quillEditor} from 'vue-quill-editor'  // 局部引用


const editButton = (_this,h,params) => {
    return h('Button', {
        props: { type:'primary',size:'small'},
        style: { marginRight:'5px' },
        on:{ 'click':()=>{_this.showModal(params.row.ID)} }
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
  name:'report',
  data () {
    return {
        reports:[],
        total:0,
        pageIndex:1,
        divModal:false,
        editId:null, // 当前编辑的报告Id
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
                    return h('div', (this.pageIndex-1 ) * 10 + params.index +1)
                }
            },
            { key:'Title',title:'标题'},
            { key:'Category',title:'分类',width:100,align:'center'},
            { key:'PublishDate',title:'创建日期',width:140,align:'center'},
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
        formValidate:{
          title:'',
          fileName:[],
          thumb:'',
          category:''
        },
        ruleValidate:{
          title:[{ required:true,message:'标题不能为空',trigger:'blur' }]
        },
        search:'',
        quillOption:{
            modules:{ toolbar:[['bold', 'italic', 'underline', 'strike'], ['blockquote', 'code-block'],[{ 'header': 1 }, { 'header': 2 }]] },
            theme:'snow',
            placeholder:'请输入正文'
        },
    };
  },
  components:{
      quillEditor
  },
  mounted(){
    this.getReport(this.pageIndex);
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
        placeholder:'请填写文章内容',
        toolbar: ['bold', 'italic', 'heading', '|', 'quote', 'unordered-list', 'clean-block', '|', 'link', 'image', 'table', 'horizontal-rule', '|', 'preview']
    })
    
  },
  methods: {
    getReport(pageIndex){
        var _params = {
            name:this.search,
            pageIndex:pageIndex
        };
        this.$http.get('/Api/Report/GetPageReport',{params : _params}).then(result=>{
            console.log(result);
            this.reports = result.data.data;
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
        query.append('id',id);
        this.$http.post('/api/admin/removeReport',query).then(result=>{
            if(!result){
                this.$Message.error({content:'删除失败,请重新操作',duration:3});
                return false;
            }
            this.$Message.success({ content:'删除成功',duration:10 })
            this.getReport(this.pageIndex);
        })
    },
    showModal(id){
        this.divModal = true;
        this.editId = id; // 当前编辑的报告Id
        this.$http.get('/Api/Report/GetReport',{ params:{ id :id } }).then(result => {
            this.formValidate = {
                title: result.data.x_title,
                thumb:'',
                category: result.data.x_reportId
            }
            this.simpleMDE.value(result.data.x_title);
        })
    },
    saveReport(){
        let query = new URLSearchParams();
        query.append('id',this.editId);
        query.append('title',this.formValidate.title);
        query.append('thumb',this.formValidate.thumb);
        query.append('content',this.simpleMDE.value());
        query.append('fileName',util.isNullOrEmpty(this.formValidate.fileName)?'' : this.formValidate.fileName.toString());
        query.append('category',util.isNullOrEmpty(this.formValidate.category)?'' : this.formValidate.category.toString());

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
        // let dataFile = util.dataURLtoFile(canvas.toDataURL(),this.cpOption.imgName);
        // var formData = new FormData();
        // formData.append('file',dataFile);
        // this.$http.post('/api/admin/uploadSingle',formData).then(result => {
        //     if(!result){
        //         this.$Message.error({ content:'图片上传失败',duration:3 });
        //         return false;
        //     }
        //     this.formValidate.thumb = '/api' + result.data.fileName;
        // })
       
       //通过base64上传图片
        var formData = new FormData();
        formData.append('file',canvas.toDataURL('image/jpg'));
        formData.append('fileName',this.cpOption.imgName);
        this.$http.post('/api/admin/uploadBase64',formData).then(result =>{
            if(!result){
                this.$Message.error({ content:'图片上传失败',duration:3});
                return false;
            }
            this.formValidate.thumb = '/api/' + result.data.fileName;
        })
    },
    handleSuccess(res,file){
        this.formValidate.fileName.push('/api/' + res.fileName)
    }
  }
}

</script>
<style lang='less'>
    .ivu-form-item{ margin-bottom:15px; }
    .ivu-modal{ top:20px; }
    .input{ width:400px }
    .ivu-card-body{padding:5px}
    .ivu-table td, .ivu-table th { height:35px;}
</style>