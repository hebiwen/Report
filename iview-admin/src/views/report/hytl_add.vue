<style lang="less">
    @import '../../styles/common.less';
</style>

<template desc="添加题录">
  <div>
    <Row>
        
        <Col span="18">
        <Card>
        <Form ref="iHytl" :model="iHytl" :rules="ruleValidate" :label-width="80" >
        <Row>
            <Col span="12">
            <FormItem label="行业分类:">
                <Select placeholder="请选择行业"></Select>
            </FormItem>
            </Col>
        </Row>
        <Row>
            <Col span="12">
            <FormItem label="年度:">
                <DatePicker type="year" format="yyyy年" placeholder="请选择年度"></DatePicker>
            </FormItem>
            </Col>
            <Col span="12">
            <FormItem label="期数:">
                <InputNumber placeholder="请输入期数" :max="200" :min="1"></InputNumber>
            </FormItem>
            </Col>
        </Row>
        <Row>
            <Col span="12">
            <FormItem label="题名:">
                <Input readonly placeholder="题名由行业、年度、期数组合而成"></Input>
            </FormItem>
            </Col>
        </Row>
        </Form>
        </Card>
        </Col>
        <Col span="6" rows="4" class="padding-left-10">
            <Card>
                <p slot="title">
                    <Icon type="paper-airplane"></Icon> 发布
                </p>
                <p>
                    发布日期:
                    <DatePicker type="datetime" format="yyyy-MM-dd" placeholder="请选择发布日期"></DatePicker>
                </p>
                <p class="margin-top-10">
                    前端显示:
                    <i-switch>
                    <span slot="open">是</span>
                    <span slot="close">否</span>
                    </i-switch>
                </p>
                <Row class="margin-top-20 publish-button-con">
                    <span class="publish-button"><Button type="primary" @click="addReport()">保存</Button></span>
                    <span class="publish-button"><Button class="margin-left-10">重置</Button></span>
                </Row>
            </Card>
        </Col>
    </Row>
      
    <tplReport></tplReport>

  </div>
</template>

<script>
import tplReport from './hytl_add_report'

export default {
    name:'hytlAdd',
    data(){
        return {
            iHytl : {

            },
            ruleValidate:{}
        }
    },
    components:{tplReport }
}
</script>

<style>
.ivu-form-item { margin-bottom: 17px; }
</style>
