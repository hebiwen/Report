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
            <FormItem label="报告分类:">
                <Select>
                    <Option value=""></Option>
                </Select>
            </FormItem>
            </Col>
        </Row>
        
        <Row>
            <Col span="12">
              <FormItem label="行业分类:" prop="hyfl">
              <Tag v-for="item in iReport.hyfl" :key="item" :name="item" closable size="medium" @on-close="delCategoryTag"></Tag>
              <Button icon="ios-plus-empty" type="success" size="small"  @click="addCategoryTag">添加行业</Button>
              </FormItem>
            </Col>
            <Col span="12">
              <FormItem label="报告来源:" prop="keyword">
                <Input placeholder="请输入报告来源"></Input>
              </FormItem>
            </Col>
        </Row>
        <Row>
          <Col span="12">
            <FormItem label="文件:">
            <Upload ref="upload" action="" name="files" :on-success="handleSuccess" >
              <Button icon="ios-cloud-upload-outline" type="success" size="small">上传文件</Button>
            </Upload> 
            </FormItem>
          </Col>
          <Col span="5">
            <FormItem label="报告页数:">
              <Input placeholder="文件上传完成后自动获取页数" readonly></Input>
            </FormItem>
          </Col>
        </Row>
        <Row>
          <Col span="12">
            <FormItem label="缩略图:">
            <Upload ref="upload" action="" name="files" :on-success="handleSuccess" >
              <Button icon="ios-cloud-upload-outline" type="success" size="small">上传文件</Button>
            </Upload> 
            </FormItem>
          </Col>
        </Row>
        <Row>
          <Col span="8">
            <FormItem label="单价:">
              <Input placeholder="请输入单价">
              <span slot="append">元/页</span>
              </Input>
            </FormItem>
          </Col>
          <Col span="8">
            <FormItem label="可预览页数:">
              <InputNumber :min="0" placeholder="可预览页数"></InputNumber>
            </FormItem>
          </Col>
          <Col span="8">
            <FormItem label="序号:">
              <InputNumber :min="1" placeholder="序号"></InputNumber>
            </FormItem>
          </Col>
        </Row>
        
        <Row>
          <Col span="16">
            <FormItem label="摘要:">
              <Input type="textarea" :rows="4"  placeholder="请输入摘要信息"></Input>
            </FormItem>
          </Col>
        </Row>

        <FormItem>
          <Button type="primary" @click="addReport()">保存</Button>
          <Button class="margin-left-10">重置</Button>
        </FormItem>
      </Form>
      </Card>

    <Modal v-model="divIndustryModal" title="行业分类" width="600" ok-text="确认" >
      <Form ref="formInline"  :label-width="80" inline>
        <Row>
            <Col span="6">
                <Tree :data="industryTree" @on-select-change="OnSelectChangeTags" ref="treeTags" show-checkbox multiple ></Tree>
            </Col>
            <Col span="2"></Col>
            <Col span="16">
                <FormItem label="已选行业" prop="">
                    <div style="border:1px solid #ddd;width:320px;">
                        <Tag closable @on-close="delIndustry()" type="dot" color="success" ></Tag>
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

export default {
  name:'reportAdd',
  data () {
    return {
        divIndustryModal:false,                         // 行业分类弹出框
        industryTree:hyfl,                              // 行业分类JSON
        inputVisible:false,                             // 关键词的输入框默认隐藏
        inputValue:'',                                  // 关键词的输入值
        simpleMDE_Directory:'',                         // 目录的富文本
        simpleMDE_Content:'',                           // 内容的富文本
        iReport:{
          title:'',
          subTitle:'',
          fileName:'',
          bgfl:bgfl,
          hyfl:[],
          keywordTags:[],
          level:''
        },
        ruleValidate:{
          title:[{ required:true,message:'请填写标题',trigger:'blur' }],
          hyfl:[{ required:true,message:'请添加行业分类' }],
          keyword:[{ required:true,message:'请添加关键词' }],
          bgfl:[{ required:true,message:'请选择报告分类' }],
          publishDate:[{ required:true,message:'请选择发布日期' }]
        }
    }
  },
  components:{ bgfl,hyfl },
  methods:{
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
    OnSelectChangeTags(){},
    handleSuccess(){},
    delIndustry(){},
    delCategoryTag(){}

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

