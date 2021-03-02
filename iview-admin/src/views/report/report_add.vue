<style lang="less">
    @import '../../styles/common.less';
    @import '../../styles/simplemde.min.css';

    .input-tag{ width: 80px; margin-left: 0; }
    .CodeMirror, .CodeMirror-scroll { min-height: 200px; }

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
                <OptionGroup v-for="(item,index) in bgflArray" :key="index" :label="item.parent">
                    <Option v-for="(dItem,dIndex) in item.children" :value="dItem.value" :key="dIndex">{{ dItem.label }}</Option>
                </OptionGroup>
                </Select>
                </FormItem>
            </Col>
            <Col span="12">
                <FormItem label="中图分类:">
                    <Select v-model="iReport.ztfl">
                      <Option v-for="item in ztflArray" :key="item.id" :value="item.id">{{ item.title }}</Option>
                    </Select>
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
                <Switch v-model="iReport.zdbg">
                    <span slot="open">是</span>
                    <span slot="close">否</span>
                </Switch>
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
              <Input v-model="iReport.filePage" placeholder="文件上传完成后自动获取页数" readonly></Input>
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

    <Modal v-model="divIndustryModal" title="行业分类" width="600" ok-text="确认" >
      <Form ref="formInline"  :label-width="80" inline>
        <Row>
            <Col span="6">
                <Tree :data="hyflTree" @on-select-change="OnSelectChangeTags" ref="treeTags" show-checkbox multiple ></Tree>
            </Col>
            <Col span="2"></Col>
            <Col span="16">
                <FormItem label="已选行业:">
                    <div style="border:1px solid #ddd;width:320px;min-height:30px;">
                        <Tag v-if="iReport.hyfl.length > 0" v-for="item in iReport.hyfl" :key="item.id" :name="item.title" closable @on-close="delSelectedTag" type="dot" color="success" >{{ item.title }}</Tag>
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
import bgfl from './dict/bgfl'
import hyfl from './dict/hyfl'
import ztfl from './dict/ztfl'

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
        divIndustryModal:false,                         // 行业分类弹出框
        bgflArray:bgfl,                                 // 报告分类JSON
        hyflTree:hyfl,                                  // 行业分类JSON
        ztflArray:ztfl,                                 // 中图分类JSON
        inputVisible:false,                             // 关键词的输入框默认隐藏
        inputValue:'',                                  // 关键词的输入值
        simpleMDE_Directory:'',                         // 目录的富文本
        simpleMDE_Content:'',                           // 内容的富文本
        iReport:{
          title:'',
          subTitle:'',
          fileName:'',
          filePath:'',
          filePage:null,
          bgfl:'',
          hyfl:[],
          ztfl:'',
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
          // title:[
          //   { required:true,message:'请填写标题',trigger:'blur' },
          //   { type:'string',min:1,message:'最小长度为1',trigger:'blur' }
          // ],
          title:[{ validator:checkTitle,trigger:'blur' }],
          hyfl:[{ required:true,message:'请添加行业分类' }],
          keywordTags:[{ required:true,message:'请添加关键词' }],
          bgfl:[{ required:true,message:'请选择报告分类' }],
          publishDate:[{ required:true,message:'请选择发布日期' }]
        }
    }
  },
  components:{ bgfl,hyfl,ztfl },
  methods:{
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
        let _hyfl = [];
        this.iReport.hyfl.forEach(item =>{ _hyfl.push(item.id) });
        query.append('hyfl',_hyfl.toString());
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
            if(result.data.code == 101){
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
    OnSelectChangeTags(node){
      let itemNode = node[node.length-1];
      console.log("node:" + itemNode);
      let nodeItem = { id:itemNode.id,title:itemNode.title }
      // 点击选中时添加分类，再次点击时移除该分类
      if(itemNode.selected){
        this.iReport.hyfl.push(nodeItem);
      }else{
        this.iReport.hyfl.filter(item=>item.id != nodeItem.id);
      }
    },
    delSelectedTag(event,name){
      // 删除分类的同时也要取消Tree中的选中状态
      this.iReport.hyfl = this.iReport.hyfl.filter( item => item.title != name );
    },
    getInput(){
      this.iReport.subTitle = this.iReport.title;
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

