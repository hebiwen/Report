<style lang="less">
    @import '../../styles/common.less';
    @import '../../styles/simplemde.min.css';

    .input-tag{ width: 80px; margin-left: 0; }
    .CodeMirror, .CodeMirror-scroll { min-height: 200px; }
    .ivu-form-item-content .ivu-btn {margin:0 5px;}

</style>

<template desc="添加报告">
  <div>
    <Card>
    <Form ref="iReport" :model="iReport" :rules="ruleValidate" :label-width="80" >
        <Row>
            <Col span="12">
            <FormItem label="标题:" prop="title">
                <Input v-model="iReport.title" @on-change="getInput" placeholder="请输入标题"></Input>
            </FormItem>
            </Col>
            <Col span="12">
            <FormItem label="副标题:">
                <Input v-model="iReport.subTitle"  placeholder="请输入副标题"></Input>
            </FormItem>
            </Col>
        </Row>
        
        <Row>
            <Col span="12">
                <FormItem label="报告分类:" prop="bgfl">
                <Select v-model="iReport.bgfl">
                <OptionGroup v-if="item.children != null" v-for="(item,index) in bgflArray" :key="index" :label="item.title">
                    <Option v-for="(dItem,dIndex) in item.children" :value="dItem.id" :key="dIndex">{{ dItem.title }}</Option>
                </OptionGroup>
                <OptionGroup label="其它">
                  <Option v-if="item.children==null" v-for="(item,index) in bgflArray" :key="index" :value="item.id">{{item.title}}</Option>
                </OptionGroup>
                </Select>
                </FormItem>
            </Col>
            <Col span="12">
                <FormItem label="中图分类:">
                    <Select v-model="iReport.ztfl">
                      <Option v-for="(item,index) in ztflArray" :key="index" :value="item.id">{{ item.title }}</Option>
                    </Select>
                </FormItem>
            </Col>
        </Row>

        <Row v-if="isEdit" >
          <Col span="12">
          <FormItem label="审核状态:">
            <Tag color="blue">{{ iReport.statusName }}</Tag>
            <Button type="primary" v-if="iReport.status <= 1" size="small" @click="verifyStatus()">{{ isAudit?'关闭':'审核' }}</Button>
            <Button type="success" size="small" v-if="iReport.status == 0" v-show="isAudit" @click="doAudit(1)" >初审通过</Button>
            <Button type="error" size="small" v-if="iReport.status == 0" v-show="isAudit" @click="doAudit(2)" >初审不通过</Button>
            <Button type="success" size="small" v-if="iReport.status == 1" v-show="isAudit" @click="doAudit(3)" >终审通过</Button>
            <Button type="error" size="small" v-if="iReport.status == 1" v-show="isAudit" @click="doAudit(4)">终审不通过</Button>
          </FormItem>
          </Col>
        </Row>

        <Row>
            <Col span="12">
            <FormItem label="重点关注:">
                <RadioGroup v-model="iReport.zdgz">
                    <Radio label="每周报告"></Radio>
                    <Radio label="月度报告"></Radio>
                    <Radio label="季度报告"></Radio>
                    <Radio label="年度报告"></Radio>
                </RadioGroup>
            </FormItem>
            </Col>
            <Col span="12">
            <FormItem label="重点报告:">
                <i-switch v-model="iReport.zdbg">
                    <span slot="open">是</span>
                    <span slot="close">否</span>
                </i-switch>
            </FormItem>
            </Col>
        </Row>

        <Row>
            <Col span="12">
              <FormItem label="行业分类:" prop="hyfl">
              <Tag v-for="item in iReport.hyfl" :key="item.id" :name="item.title" closable size="medium" @on-close="delSelectedTag">{{item.title}}</Tag>
              <Button icon="ios-add" type="success" size="small"  @click="addCategoryTag">添加行业</Button>
              </FormItem>
            </Col>
            <Col span="12">
              <FormItem label="关键词:" prop="keywordTags">
              <Tag v-for="item in iReport.keywordTags" :key="item" :name="item" size="medium" closable @on-close="delKeywordTag">{{item}}</Tag>
              <Input class="input-tag" v-if="inputVisible" v-model="inputValue" ref="saveInputTag" @on-enter="addKeywordTag" @on-blur="addKeywordTag"></Input>
              <Button icon="ios-add" type="success" size="small" @click="showInputTag">添加关键词</Button>
              </FormItem>
            </Col>
        </Row>
        <Row>
          <Col span="12">
            <FormItem label="文件:">
            <Upload ref="upload" v-model="iReport.fileName" action="#" :format="['pdf']" :on-format-error="handleFormatError" :before-upload="handleBefore">
              <Button icon="ios-cloud-upload-outline" type="success" size="small">上传文件</Button>
            </Upload>
            <div v-if="iReport.fileName!=''">已上传:{{ iReport.fileName }}</div> 
            </FormItem>
          </Col>
          <Col span="5">
            <FormItem label="报告页数:">
              <Input v-model="iReport.filePage" placeholder="上传完成后自动获取页数" readonly></Input>
            </FormItem>
          </Col>
        </Row>
        <Row>
          <Col span="8">
            <FormItem label="作者:">
              <Input v-model="iReport.author" placeholder="请输入作者"></Input>
            </FormItem>
          </Col>
          <Col span="8">
            <FormItem label="作者单位:">
              <Input v-model="iReport.company" placeholder="请输入作者单位"></Input>
            </FormItem>
          </Col>
          <Col span="8">
            <FormItem label="信息来源:">
              <Input v-model="iReport.source" placeholder="请输入信息来源"></Input>
            </FormItem>
          </Col>
        </Row>
        <Row>
          <Col span="8">
            <FormItem label="语言:">
              <RadioGroup v-model="iReport.language">
                <Radio label="中文"></Radio>
                <Radio label="英文"></Radio>
              </RadioGroup>
            </FormItem>
          </Col>
          <Col span="8">
            <FormItem label="国家:">
              <Select v-model="iReport.country" placheolder="请选择国家">
                <Option value="美国">美国</Option>
                <Option value="中国">中国</Option>
              </Select>
            </FormItem>
          </Col>
          <Col span="8">
            <FormItem label="地区:">
              <Input v-model="iReport.area" placeholder="请输入地区"></Input>
            </FormItem>
          </Col>
        </Row>

        <Row>
          <Col span="16">
            <FormItem label="摘要:">
              <Input v-model="iReport.abstract" type="textarea" :rows="4"  placeholder="请输入摘要信息"></Input>
            </FormItem>
          </Col>
          <Col span="8">
            <FormItem label="报告级别:">
              <RadioGroup v-model="iReport.level">
                <Radio label="1">一级</Radio>
                <Radio label="2">二级</Radio>
              </RadioGroup>
            </FormItem>
            <FormItem label="发布日期:" prop="publishDate">
              <DatePicker v-model="iReport.publishDate" format="yyyy-MM-dd" type="date" placeholder="请选择日期"></DatePicker>
            </FormItem>
          </Col>
        </Row>

        <FormItem label="目录:">
            <Row>
                <Col span="24" offset="0">
                <div class="markdown-con">
                  <textarea id="txt_markdown_directory"></textarea>                
                </div>
                </Col>
            </Row>
        </FormItem>

        <FormItem label="主要内容:">
            <Row>
                <Col span="24" offset="0">
                <div class="markdown-con">
                  <textarea id="txt_markdown_content"></textarea>                
                </div>
                </Col>
            </Row>
        </FormItem>

        <FormItem>
          <Button type="primary" @click="addReport()">保存</Button>
          <Button class="margin-left-10" @click="resetReport()">重置</Button>
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
import SimpleMDE from 'simplemde'
export default {
  name:'reportAdd',
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
        isEdit:false,                                    // 是否编辑
        isAudit:false,                                   // 是否显示审核通过/不通过
        divIndustryModal:false,                          // 行业分类弹出框
        bgflArray: JSON.parse(localStorage.bgflArray),   // 报告分类JSON
        hyflArray: JSON.parse(localStorage.hyflArray),   // 行业分类JSON
        ztflArray: JSON.parse(localStorage.ztflArray),   // 中图分类JSON
        inputVisible:false,                              // 关键词的输入框默认隐藏
        inputValue:'',                                   // 关键词的输入值
        simpleMDE_Directory:'',                          // 目录的富文本
        simpleMDE_Content:'',                            // 内容的富文本
        iReport:{
          id:null,
          title:'',
          subTitle:'',
          fileName:'',
          filePath:'',
          filePage:null,
          bgfl:'',
          hyfl:[],
          ztfl:null,
          keywordTags:[],
          level:'',
          publishDate: new Date(),
          zdgz:null,
          zdbg:false,
          author:'',
          company:'',
          language:'',
          source:'',
          country:'',
          area:'',
          abstract:''
        },
        ruleValidate:{
          // title:[{ required:true,message:'请填写标题',trigger:'blur' },{ type:'string',min:1,message:'最小长度为1',trigger:'blur' }],
          title:[{ required:true,validator:checkTitle,trigger:'blur' }],
          hyfl:[{ required:true,message:'请添加行业分类' }],
          keywordTags:[{ required:true,message:'请添加关键词' }],
          bgfl:[{ required:true,message:'请选择报告分类' }],
          publishDate:[{ required:true,message:'请选择发布日期' }]
        },
        props:{
          label:'title',
          children:'children'
        }
    }
  },
  created(){
    if(!util.isNullOrEmpty(this.$route.query.id)){
      this.iReport.id = this.$route.query.id;
      this.isEdit = true;
      this.getReport();
    }
  },
  methods:{
    getReport(){
        this.$http.get('/Api/Report/GetReport',{ params:{ id : this.iReport.id } }).then(result => {
            if(result.data.code == 0) {
                this.$Modal.error({ title:'提示', content:result.data.msg,duration:3});
                return;
            }
            let dItem = JSON.parse(result.data.data);
            this.iReport = {
                title: dItem.data.Title,
                subTitle: dItem.data.SubTitle,
                bgfl: dItem.data.bgfl,
                status:dItem.data.Status,
                statusName: dItem.statusName,
                ztfl:dItem.data.ztfl,
                fileName:dItem.data.FileName,
                filePath:dItem.data.FilePath,
                filePage:dItem.data.FilePage,
                keywordTags:dItem.data.KeyWords.split(','),
                level:dItem.data.Level,
                publishDate: dItem.data.PublishDate,
                zdgz:dItem.data.zdgzfl,
                zdbg:dItem.data.IsMain == 1 ? true : false,
                author:dItem.data.Author,
                company:dItem.data.Company,
                language:dItem.data.Language,
                source:dItem.data.Source,
                country:dItem.data.Country,
                area:dItem.data.Area,
                abstract:dItem.data.Abstract,
            }
            this.iReport.hyfl = util.mapCategory(this.hyflArray,dItem.data.hyfl);
            this.simpleMDE_Directory.value(dItem.data.Directory);
            this.simpleMDE_Content.value(dItem.data.Content);
        })
    },
    resetReport(){
      this.$refs['iReport'].resetFields();
    },
    addReport(){
      this.$refs['iReport'].validate(valid=>{
        if(!valid){
            this.$Modal.error({ title:'提示',content:'<p>保存失败，请检查后重新提交.</p>'});
            return;
        }
        debugger;
        console.log("iReport:" + JSON.stringify(this.iReport));
        let query = new URLSearchParams();
        query.append('title',this.iReport.title);
        query.append('subTitle',this.iReport.subTitle);
        query.append('bgfl',this.iReport.bgfl);
        query.append('ztfl',this.iReport.ztfl);
        query.append('zdgz',this.iReport.zdgz);
        query.append('zdbg',this.iReport.zdbg);
        query.append('hyfl',this.iReport.hyfl.map(item => item.id).toString());
        query.append('keyword',this.iReport.keywordTags.toString());
        query.append('filePage',this.iReport.filePage);
        query.append('filePath',this.iReport.filePath);
        query.append('fileName',this.iReport.fileName);
        query.append('author',this.iReport.author);
        query.append('company',this.iReport.company);
        query.append('source',this.iReport.source);
        query.append('language',this.iReport.language);
        query.append('country',this.iReport.country);
        query.append('area',this.iReport.area);
        query.append('abstract',this.iReport.abstract);
        query.append('level',this.iReport.level);
        query.append('publishDate',this.iReport.publishDate);
        query.append('directory',this.simpleMDE_Directory.value());
        query.append('content',this.simpleMDE_Content.value());
        console.log("query:" + JSON.stringify(query));
        this.$http.post('/Api/Report/SaveReport',query).then(result => {
          let msg = { title:'提示', content:result.data.msg }
            if(result.data.code == util.error){
                this.$Modal.error(msg);
                return;
            }
            this.$Modal.success(msg);
            this.$refs['iReport'].resetFields();
        })
      })
    },
    addCategoryTag(){
      this.divIndustryModal = true;
    },
    showInputTag(){
      this.inputVisible = true;
      this.$nextTick(()=>{
        this.$refs.saveInputTag.$refs.input.focus();
      })
    },
    addKeywordTag(){
      let inputValue = this.inputValue;
      if(inputValue) this.iReport.keywordTags.push(inputValue);
      this.inputVisible = false;
      this.inputValue = '';
    },
    delKeywordTag(event,name){
      const index = this.iReport.keywordTags.indexOf(name);
      this.iReport.keywordTags.splice(index,1);
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
    checkChange(node,checked,indeter){
      let nodeItem = { id:node.id,title:node.title }
      // 点击选中时添加分类，再次点击时移除该分类
      if(checked){
        this.iReport.hyfl.push(nodeItem);
      }else{
        let idx = this.iReport.hyfl.findIndex(el => el.id == node.id);
        this.iReport.hyfl.splice(idx,1);
        if(indeter) this.$refs.treeTags.setChecked(node,false);
      }
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
    getInput(){
      this.iReport.subTitle = this.iReport.title;
    },
    verifyStatus(){
      this.isAudit = !this.isAudit;
    },
    doAudit(status){
      let query = new URLSearchParams();
      query.append('status',status);
      query.append('id',this.editId);
      this.$http.post('/Api/Report/AuditReport',query).then(result => {
        debugger
        if(result.data.code == 101){
            this.$Modal.error({title:util.errorTitle, content:result.data.msg });
            return;
        }
        let dItem = JSON.parse(result.data.data);
          this.iReport.statusName = dItem.statusName;
          this.iReport.status = dItem.data.Status;
          this.$Modal.success({ title:util.infoTitle, content:result.data.msg,duration:3 });
      })
      this.isAudit = false;
    },
  },
  mounted(){
    this.simpleMDE_Directory = new SimpleMDE({
        element: document.getElementById('txt_markdown_directory'),
        // autoDownloadFontAwesome:false,       // 自动下载FontAwesome,国内下载非常慢，所以要手动安装npm install fontawesome
        placeholder:'请输入报告目录',
        toolbar: ['bold', 'italic', 'heading', '|', 'quote', 'unordered-list', 'clean-block', '|', 'link', 'image', 'table', 'horizontal-rule', '|', 'preview']
    })
    this.simpleMDE_Content = new SimpleMDE({
        element: document.getElementById('txt_markdown_content'),
        // autoDownloadFontAwesome:false,       // 自动下载FontAwesome,国内下载非常慢，所以要手动安装npm install fontawesome
        placeholder:'请输入报告内容',
        toolbar: ['bold', 'italic', 'heading', '|', 'quote', 'unordered-list', 'clean-block', '|', 'link', 'image', 'horizontal-rule', '|', 'preview']
    })
  }
}

</script>

