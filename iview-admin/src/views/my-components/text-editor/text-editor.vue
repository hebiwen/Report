<style lang="less">
    @import '../../../styles/loading.less';
</style>

<template>
    <div>
        <Spin fix v-if="spinShow">
            <Icon type="load-c" size=18 class="demo-spin-icon-load"></Icon>
            <div>加载组件中...</div>
        </Spin>
    </div>
</template>

<script>

export default {
    name: 'text-editor',
    data () {
        return {
            spinShow: true
        };
    },
    methods: {
        
    },
    mounted () {
        
    },
   
};
</script>

<style>

</style>
