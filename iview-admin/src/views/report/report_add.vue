<style lang="less">
    @import '../../styles/common.less';
    @import '../../styles/image-editor.less';
    @import '../../styles/cropper.min.css';
    @import '../../styles/simplemde.min.css';
</style>

<!-- 添加/修改文章 -->
<template>
  <div>
    <Form ref="formValidate" :model="formValidate" :rules="ruleValidate" :label-width="80" >
        <FormItem label="标题：" prop="title">
            <Input v-model="formValidate.title" class="input" placeholder="请输入标题"></Input>
        </FormItem>

        <FormItem label="分类：" prop="category">
            <CheckboxGroup v-model="formValidate.category">
                <Checkbox label="文学"></Checkbox>
                <Checkbox label="历史"></Checkbox>
                <Checkbox label="教育"></Checkbox>
            </CheckboxGroup>
        </FormItem>

        <FormItem label="缩略图：" prop="thumb" class="image-editor" >
          <Row :gutter="10">
            <Col span="12">
                <Card>
                    <Row :gutter="10">
                        <Col span="12" class="image-editor-con1">
                            <div class="cropper">
                                <img id="cropimg" :src="cpOption.imgURL" alt="">
                            </div>
                        </Col>
                        <Col span="10" class="image-editor-con1">
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
            </Col>
          </Row>

          <!-- 
            <img :src="formValidate.thumb" style="width:160px;height:180px" class="fl" >
              <Upload action="/api/admin/uploadSingle" name="file" :on-success="handleSuccess" class="fl margin-left-10" >
              <Button icon="ios-cloud-upload-outline">上传图片</Button>
            </Upload>
          -->
        </FormItem>
        <FormItem label="文件">
            <Upload action="/api/admin/uploadArray" name="files"  :on-success="handleSuccess" multiple >
              <Button icon="ios-cloud-upload-outline">上传文件</Button>
            </Upload> 
        </FormItem>

        <FormItem label="摘要">
            <Row>
                <Col span="12" offset="0">
                <div class="markdown-con">
                    <Card>
                        <textarea  id="txt_markdown_editor" style="display:none;"></textarea>                
                    </Card>
                </div>
                </Col>
            </Row>
        </FormItem>
        
        <FormItem>
            <Button type="primary" @click="addReport()">保存</Button>
            <Button style="margin-left: 8px">取消</Button>
        </FormItem>
    </Form>
  </div>
</template>

<script>

import util from '@/libs/util'
import SimpleMDE from 'simplemde'
import Cropper from 'cropperjs';

export default {
  name:'addReport',
  data () {
    return {
        uploadList: [],
        cropper:'',
        cpOption:{
          showCropedImage:false,
          cropedImg:'',
          imgURL:'', // 选择图片的文件路径
          imgName:'', // 缩虐图的文件名aa.jpg
        },
        
        simpleMDE:'', // 富文本内容
        formValidate:{
          title:'',
          fileName:[],
          thumb:'',
          category:[]
        },
        ruleValidate:{
          title:[{ required:true,message:'标题不能为空',trigger:'blur' }]
        }
    }
  },
  methods:{
    addReport(){
      this.$refs['formValidate'].validate(valid=>{
          if(!valid){
              this.$Message.error('保存失败，请检查后重新提交.');
              return;
          }

        let query = new URLSearchParams();
        query.append('title',this.formValidate.title);
        query.append('category',this.formValidate.category);
        query.append('thumb',this.formValidate.thumb);
        query.append('fileName',this.formValidate.fileName);
        query.append('content',this.simpleMDE.value());

        this.$http.post('/api/admin/addReport',query).then(result => {
            if(!result){
                this.$Message.error({ content:result.message,duration:10 });
                return;
            }
            this.$Message.success({ content:'保存成功',duration:3 });
            this.$refs['formValidate'].resetFields();
        })

      })
    },
    // iview-upload上传
    handleSuccess (res, file) {
        if(res && !util.isNullOrEmpty(res.fileName)){
          this.formValidate.fileName.push("/api" + res.fileName);
        }
    },
    // 1、选择图片 2、裁剪 3、压缩 4、上传
    handleChange(e){
      let file = e.target.files[0];
        this.cpOption.imgName = file.name;
        let reader = new FileReader();
        reader.onload = () => {
          this.cropper.replace(reader.result);
        };
        reader.readAsDataURL(file);
    },
    // getObjectURL(file){
    //     var url = null;
    //     if(window.createObjectURL!=undefined){
    //         url = window.createObjectURL(file)
    //     }else if(window.URL!=undefined){
    //         url = window.URL.createObjectURL(file)
    //     }else if(window.webkitURL!=undefined){
    //         url = window.webkitURL.createObjectURL(file)
    //     }
    //     return url;
    // },
    handleCrop(){
        let canvas = this.cropper.getCroppedCanvas();
        let dataFile = util.dataURLtoFile(canvas.toDataURL(),this.cpOption.imgName);
        var formData = new FormData();
        formData.append('file',dataFile);
        this.$http.post('/api/index/uploadSingle',formData).then(result => {
            if(!result){
                this.$Message.error({ content:'图片上传失败',duration:3 });
                return false;
            }
            this.formValidate.thumb = '/api' + result;
        })
    }
  },
  mounted(){
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
      // width:90,
      // height:120
    });
    this.simpleMDE = new SimpleMDE({
        element: document.getElementById('txt_markdown_editor'),
        placeholder:'请填写文章内容',
        toolbar: ['bold', 'italic', 'heading', '|', 'quote', 'unordered-list', 'clean-block', '|', 'link', 'image', 'table', 'horizontal-rule', '|', 'preview']
    })
  }
}

</script>

