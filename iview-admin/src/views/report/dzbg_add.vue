<style lang="less">
    @import '../../styles/common.less';
    @import '../../styles/simplemde.min.css';

    .input-tag{ width: 80px; margin-left: 0; }
    .CodeMirror, .CodeMirror-scroll { min-height: 200px; }

</style>

<template desc="添加定制报告">
  <div>
    <Card>
    <Form ref="iReport" :model="iReport" :rules="ruleValidate" :label-width="80" >
        <Row>
            <Col span="12">
            <FormItem label="标题:" prop="title">
                <Input v-model="iReport.title" oninput="getInput(this)" placeholder="请输入标题"></Input>
            </FormItem>
            </Col>
            <Col span="12">
            <FormItem label="报告分类:" prop="category">
                <Select v-model="iReport.category" clearable placeholder="请选择分类" style="width:200px">
                    <Option v-for="item in lstCategory" :key="item.id" :value="item.id">{{item.name}}</Option>
                </Select>
            </FormItem>
            </Col>
        </Row>
        
        <Row>
            <Col span="12">
              <FormItem label="行业分类:">
              <Tag v-for="item in iReport.hyfl" :key="item.id" :name="item.title" closable size="medium" @on-close="delSelectedTag">{{item.title}}</Tag>
              <Button icon="ios-add" type="success" size="small"  @click="addCategoryTag">添加行业</Button>
              </FormItem>
            </Col>
            <Col span="12">
              <FormItem label="报告来源:">
                <Input placeholder="请输入报告来源"></Input>
              </FormItem>
            </Col>
        </Row>
        <Row>
          <Col span="12">
            <FormItem label="文件:">
            <Upload ref="upload" action="#" :format="['pdf']" :on-format-error="handleFormatError" :before-upload="handleBefore" >
              <Button icon="ios-cloud-upload-outline" type="success" size="small">上传文件</Button>
            </Upload> 
            <div v-if="iReport.fileName!=''">已上传:{{ iReport.fileName }}</div> 
            </FormItem>
          </Col>
          <Col span="12">
            <FormItem label="缩略图:">
            <Upload ref="upload" action="#" :format="['jpg','png','jepg']" :on-format-error="handleFormatError" :before-upload="handleBefore" >
              <Button icon="ios-cloud-upload-outline" type="success" size="small">上传文件</Button>
            </Upload> 
            </FormItem>
          </Col>
        </Row>
        <Row>
          <Col span="6">
            <FormItem label="单价:">
              <InputNumber v-model="iReport.price" :min="0" placeholder="请输入单价">
              </InputNumber>
            </FormItem>
          </Col>
          <Col span="6">
            <FormItem label="可预览页数:">
              <InputNumber v-model="iReport.previewPage" :min="0" placeholder="可预览页数"></InputNumber>
            </FormItem>
          </Col>
          <Col span="6">
            <FormItem label="报告页数:">
              <Input v-model="iReport.FilePage" placeholder="报告页数" readonly style="width:78px"></Input>
            </FormItem>
          </Col>
        </Row>
        
        <Row>
          <Col span="16">
            <FormItem label="报告摘要:">
              <Input v-model="iReport.description" type="textarea" :rows="6"  placeholder="请输入摘要信息"></Input>
            </FormItem>
          </Col>
        </Row>

        <FormItem>
          <Button type="primary" @click="addReport()">保存</Button>
          <Button class="margin-left-10">重置</Button>
        </FormItem>
      </Form>
      </Card>

    <Modal v-model="divIndustryModal" title="行业分类" width="800" ok-text="确认" >
      <Form ref="formInline"  :label-width="80" inline>
        <Row>
            <Col span="10">
                <!-- <Tree :data="hyflArray" @on-select-change="onSelectNode" @on-check-change="onSelectNode" ref="treeTags" show-checkbox multiple ></Tree> -->
                <el-tree :data="hyflArray" :props="props" show-checkbox highlight-current node-key="id" ref="treeTags" @check="treeCheck"></el-tree>
            </Col>
            <Col span="2"></Col>
            <Col span="12">
                <FormItem label="已选行业:">
                    <div style="border:1px solid #ddd;width:320px;min-height:30px;">
                        <Tag v-if="iReport.hyfl.length > 0" v-for="item,index in iReport.hyfl" :key="index" :name="item.title" closable @on-close="delSelectedTag" type="dot" color="success" >{{ item.title }}</Tag>
                    </div>
                </FormItem>
            </Col>
        </Row>
      </Form>
    </Modal>
  </div>
</template>

<script>

import util from '@/libs/util'

export default {
  name:'dzbgAdd',
  data () {
    return {
        divIndustryModal:false,                                       // 行业分类弹出框
        hyflArray: JSON.parse(localStorage.hyflArray),             // 行业分类JSON 
        iReport:{
          id:null,
          title:'',
          category:null,
          fileName:'',
          filePath:'',
          filePage:null,
          previewPage:null,
          thumb:'',
          hyfl:[],
          description:'',
          createBy:''
        },
        ruleValidate:{
          title:[{ required:true,message:'请填写标题',trigger:'blur' }],
          category:[{ required:true,message:'请选择分类' }]
        },
        props:{
          label:'title',
          children:'children'
        },
        lstCategory:[{ id:1,name:'样本' },{ id:2,name:'大数据分析报告' },{ id:3,name:'深度研究报告' },{ id:4,name:'专题报告' }]
    }
  },
  created(){
   if(!util.isNullOrEmpty(this.$route.query.id)){
      this.iReport.id = this.$route.query.id;
      this.getReport();
   }
  },
  methods:{
    getReport(){
      var _params = { id:this.iReport.id };
      this.$http.get('/Api/DZReport/GetDZReport',{ params:_params }).then(result => {
          if(result.data.code == util.error) {
            this.$Model.error({ title:'错误',content:result.data.msg }); return
          }
          let dItem = JSON.parse(result.data.data);
          this.iReport = { 
            id:dItem.id,
            title:dItem.Title,
            category:dItem.Category,
            fileName:dItem.FileName,
            filePath:dItem.FilePath,
            filePage:dItem.FilePage,
            previewPage:dItem.PreviewPage,
            thumb:dItem.Thumb,
            hyfl:[],
            description:dItem.Description,
            createBy:dItem.CreateBy
          }
      })
    },
    addReport(){
      this.$refs['iReport'].validate(valid=>{
          if(!valid){
              this.$Modal.error({ title:'提示',content:'<p>保存失败，请检查后重新提交.</p>'});
              return;
          }

        let query = new URLSearchParams();
        query.append('title',this.iReport.title);
        query.append('bgfl',this.iReport.bgfl);
        query.append('thumb',this.iReport.thumb);
        query.append('fileName',this.iReport.fileName);
        query.append('content',this.simpleMDE_Content.value());

        this.$http.post('/api/admin/addReport',query).then(result => {
            if(!result){
                this.$Modal.error({ title:'提示', content:result.message, duration:10 });
                return;
            }
            this.$Modal.success({ title:'提示', content:'保存成功',duration:3 });
            this.$refs['iReport'].resetFields();
        })

      })
    },
    addCategoryTag(){
      this.divIndustryModal = true;
    },
    handleBefore(file){
      var formData = new FormData();
      formData.append('file',file);
      this.$http.post('/Api/FileUpload/UploadFile',formData).then(result=>{
        if(result.data.status == 101) { this.$Modal.error({ title :'上传失败', content : result.data.message }); return }
        if(result.data.status == 0){
          this.iReport.fileName = result.data.fileName;
          this.iReport.filePath = result.data.filePath;
          this.iReport.filePage = result.data.filePage;
        }
      })
    },
    handleFormatError(file){
      this.$Modal.error({ title:'上传失败',content:'文件格式错误' })
    },
    treeCheck(node,cNodes){
      let nodeItem = { id:node.id,title:node.title };
      let idx = this.iReport.hyfl.findIndex(el => el.id == node.id);
      if(idx == -1){
        this.iReport.hyfl.push(nodeItem);
      }else{
        this.iReport.hyfl.splice(idx,1);
      }
    },
    delSelectedTag(event,name){
      var checkNodes = this.$refs.treeTags.getCheckedNodes();
      var node = checkNodes.filter(node => { return node.title == name });
      var nodes = checkNodes.filter(node => { return node.title != name });
      this.$refs.treeTags.setCheckedNodes(nodes);
      this.treeCheck(node[0],nodes)
    },

  }
}

</script>

