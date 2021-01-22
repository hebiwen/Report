
<template>
  <div width="600">
    <Form ref="formValidate" :model="formValidate" :rules="ruleValidate" :label-width="80" >
        <FormItem label="登录名" prop="userId">
            <Input v-model="formValidate.userId" placeholder="Enter your name"></Input>
        </FormItem>
        <FormItem label="姓名" prop="userName">
            <Input v-model="formValidate.userName" placeholder="Enter your e-mail"></Input>
        </FormItem>
        <FormItem label="部门" prop="dept">
            <Input v-model="formValidate.dept" placeholder="Enter your e-mail"></Input>
        </FormItem>
        <FormItem label="手机号" prop="mobile">
            <Input v-model="formValidate.mobile" placeholder="Enter your e-mail"></Input>
        </FormItem>
        <FormItem label="邮箱" prop="email">
            <Input v-model="formValidate.email" placeholder="Enter your e-mail"></Input>
        </FormItem>
        <FormItem label="用户类型" prop="userType">
            <Select v-model="formValidate.userType" placeholder="Select your city">
                <Option value="1">次卡</Option>
                <Option value="2">年卡</Option>
                <Option value="3">永久</Option>
            </Select>
        </FormItem>
        <FormItem label="审核日期">
            <Row>
                <Col span="11">
                    <FormItem prop="date">
                        <DatePicker type="date" placeholder="Select date" v-model="formValidate.auditDate"></DatePicker>
                    </FormItem>
                </Col>
            </Row>
        </FormItem>
        <FormItem label="职位" prop="position">
            <CheckboxGroup v-model="formValidate.position">
                <Checkbox label="总工程师"></Checkbox>
                <Checkbox label="党委委员"></Checkbox>
                <Checkbox label="经营部经理"></Checkbox>
                <Checkbox label="主任"></Checkbox>
            </CheckboxGroup>
        </FormItem>
        <FormItem>
            <Button type="primary" @click="addUser('formValidate')">保存</Button>
            <Button style="margin-left: 8px">取消</Button>
        </FormItem>
    </Form>
  </div>
</template>

<script>
export default {
  name:'adminAdd',
  data () {
    return {
      formValidate:{
        userId:'',
        userName:'',
        dept:'',
        mobile:'',
        email:'',
        position:[],
        userType:null,
        auditDate:Date
      },
      ruleValidate:{
        userId:[{ required : true, message : '账号不能为空',trigger:'blur' }],
        userName:[{ required:true,message:'姓名不能为空',trigger:'blur' }],
        dept:[{ required:true,message:'部门不能为空',trigger:'blur' }],
        mobile:[{ required:true,message:'',trigger:'blur' }],
        email:[
          { required:true,message:'邮箱不能为空',trigger:'blur' },
          { type:'email',message:'邮箱格式不正确', trigger:'blur' }
          ],
      }
    };
  },
  methods:{
    addUser(item){
      // this.$refs[name].resetFields(); 重置
      this.$refs[item].validate(valid=>{
        if(!valid){
          this.$Message.error('保存失败，检查后再重新提交..');
          return;
        }

      let query = new URLSearchParams();
      query.append('userName',this.formValidate.userName);
      query.append('dept',this.formValidate.dept);
      query.append('userId',this.formValidate.userId);
      query.append('mobile',this.formValidate.mobile);
      query.append('email',this.formValidate.email);
      query.append('userType',this.formValidate.userType);
      query.append('position',this.formValidate.position);
      query.append('auditDate',this.formValidate.auditDate);

      this.$http.post('/api/admin/addUser',query).then(result=>{
        if(!result){
          this.$Message.error({ content:'保存失败',duration:3});
          return;
        }
        if(result.errCode==1){
          this.$Message.error({ content:result.message,duration:10 });
          return;
        }
        this.$Message.success({ content: '保存成功',duration:3 })
        this.$refs[item].resetFields(); 
      })
      })
    }
  }
}

</script>
<style lang='scss' scoped>
</style>