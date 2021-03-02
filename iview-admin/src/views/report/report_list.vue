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

    <Modal v-model="divEditModal" title="行业报告详情" width="900" @on-ok="saveReport()" ok-text="保存" >
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
                      <Option v-for="item in ztflArray" :key="item.id" :value="item.id">{{ item.title }}</Option>
                    </Select>
                </FormItem>
            </Col>
        </Row>

        <Row>
          <Col span="12">
          <FormItem label="审核状态:">
            <Tag color="blue">{{ iReport.statusName }}</Tag>
            <Button type="primary" v-if="iReport.status <= 1" size="small" @click="verifyStatus()">审核</Button>
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
          <Button class="margin-left-10">取消</Button>
        </FormItem>
    </Form>
    </Modal>

    <Modal v-model="divIndustryModal" title="行业分类" width="600" ok-text="确认" >
      <Form ref="formInline"  :label-width="80" inline>
        <Row>
            <Col span="6">
                <Tree :data="hyflArray" @on-select-change="OnSelectChangeTags" ref="treeTags" show-checkbox multiple ></Tree>
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
// import bgfl from './dict/bgfl' import hyfl from './dict/hyfl' import ztfl from './dict/ztfl'
// import { bgflArr,hyflArr,ztflArr } from './dict/bgflArr'

const editButton = (_this,h,params) => {
    return h('Button', {
        props: { type:'primary',size:'small'},
        style: { marginRight:'5px' },
        on:{ 'click':()=>{_this.showModal(params.row.id)} }
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
        editId:null,                                    // 当前编辑的报告Id
        divEditModal:false,                             // 编辑弹出框
        isAudit:false,                                  // 是否显示审核通过/不通过
        divIndustryModal:false,                         // 行业分类弹出框
        bgflArray:JSON.parse(localStorage.bgflArray),   // 报告分类JSON
        hyflArray:JSON.parse(localStorage.hyflArray),    // 行业分类JSON
        ztflArray:JSON.parse(localStorage.ztflArray),   // 中图分类JSON
        inputVisible:false,                             // 关键词的输入框默认隐藏
        inputValue:'',                                  // 关键词的输入值
        simpleMDE_Directory:'',                         // 目录的富文本
        simpleMDE_Content:'',                           // 主要内容富文本
        columns:[
            {  
                align:'center',
                width:70,
                render:(h,params)=>{
                    return h('div', (this.pageIndex-1 ) * 10 + params.index +1)
                }
            },
            { key:'title',title:'标题',aling:'center'},
            { key:'bgflName',title:'分类',width:100,align:'center'},
            {   key:'status',
                title:'状态',
                width:100,
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
            { key:'publishDate',title:'发布日期',width:140,align:'center'},
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
          title:[{ validator:checkTitle,trigger:'blur' }],
          hyfl:[{ required:true,message:'请添加行业分类' }],
          keywordTags:[{ required:true,message:'请添加关键词' }],
          bgfl:[{ required:true,message:'请选择报告分类' }],
          publishDate:[{ required:true,message:'请选择发布日期' }]
        },
        
    };
  },
  // components:{ bgfl,hyfl,ztfl },
  mounted(){
    this.getReport(this.pageIndex);

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
    });
    
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
            this.$Message.error({content:'删除失败,请重新操作',duration:3});
            if(result.data.code == 101) return;
            
            this.getReport(this.pageIndex);
        })
    },
    showModal(id){
      
        this.divEditModal = true;
        this.editId = id; // 当前编辑的报告Id
        this.$http.get('/Api/Report/GetReport',{ params:{ id :id } }).then(result => {
            if(result.data.code == 101) {
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
                ztfl:dItem.data.ztfl.toString(),
                fileName:dItem.data.FileName,
                filePath:dItem.data.FilePath,
                filePage:dItem.data.FilePage,
                // hyfl:dItem.data.hyfl.split(','),
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
            var hyflArr = [];
            this.hyflArray.forEach(item =>{
              debugger;
              var hyflItem = dItem.data.hyfl.split(',');
              if(hyflItem.includes(item.id.toString())){
                hyflArr.push({ id : item.id, title : item.title });
              }
            })
            this.iReport.hyfl = hyflArr;
            this.simpleMDE_Directory.value(dItem.data.Directory);
            this.simpleMDE_Content.value(dItem.data.Content);
        })
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
    saveReport(){
        let query = new URLSearchParams();
        query.append('id',this.editId);
        query.append('title',this.iReport.title);

        // let params = JSON.stringify({ id:1,title:"dd" }); 不能使用此方法传参数
        this.$http.post('/Api/Report/SaveReport',query).then(result => {
            this.$Message.success({ content : result.data.msg,duration:3 });
            if(result.data.code == 101 ) return;
            this.divModal = false;
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
    
  }
}

</script>
<style lang='less'>
    .ivu-form-item{ margin-bottom:15px; }
    .ivu-modal{ top:20px; }
    .ivu-table td, .ivu-table th { height:35px;}
</style>