<style lang="less">
    @import '../../styles/common.less';
    @import './error-page.less';
</style>

<template>
    <div class="error-page">
        
        <Row class="margin-top-10">
            <Card>
                <p slot="title">
                    <Icon :size="14" type="android-lock"></Icon>
                    403-权限不足
                </p>
                <Row>
                    <Col span="10">
                        <Card dis-hover>
                            <Row>
                                <div class="error-page-show">
                                    <error403></error403>
                                </div>
                                <div class="error-page-cover"></div>
                            </Row>
                        </Card>
                    </Col>
                    <Col span="14" class="padding-left-10">
                        <Row type="flex" align="middle" class="error-page-intro-con">
                            <p>在当前登录用户不具有执行当前操作的权限时跳转到该页面，您可以在ajax请求方法中判断返回的状态码为403时跳转到该页面</p>
                        </Row>
                    </Col>
                </Row>
            </Card>
        </Row>
        
    </div>
</template>

<script>
import Error403 from './403.vue';
export default {
    components: {
        Error403
    }
};
</script>
