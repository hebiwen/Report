<style lang="less">
    @import '../../styles/common.less';
    @import '../../styles/draggable-list.less';

    .demo-drawer-footer{
        width: 100%;
        position: absolute;
        bottom: 0;
        left: 0;
        border-top: 1px solid #e8e8e8;
        padding: 10px 16px;
        text-align: right;
        background: #fff;
    }
    .ivu-card-head p{ height:24px;line-height:24px }
</style>

<template>
    <div>
        <Row>
            <Col span="24" class="margin-top-10">
                <Card>
                    <Row>
                        <Col span="12">
                            <Card dis-hover>
                                
                                <p slot="title">
                                    <Icon type="ios-list-outline"></Icon> 可选择报告(拖拽到右侧添加)
                                    <Button @click="isDrawer = true" class="fr" type="primary" size="small">查询</Button>
                                </p>

                                <div style="height: 360px;">
                                     <ul id="todoList" class="iview-admin-draggable-list">
                                        <li v-for="(item, index) in todoArray" :key="index" class="notwrap todolist-item" :data-index="index">
                                            {{ item.content }}
                                            <span class="fr">2020-01-18</span>
                                        </li>
                                    </ul>
                                </div>
                                <Page :total="total" :page-size="20" :current="pageIndex" @on-change="changePage" show-total class="margin-top-8" />
                            </Card>
                        </Col>
                        <Col span="12" class="padding-left-10">
                            <Card dis-hover>
                                <p slot="title">
                                    <Icon type="ios-list"></Icon>
                                    已选择报告(拖拽到左侧删除)
                                </p>
                                <div style="height: 360px;">
                                    <ul id="doList" class="iview-admin-draggable-list"></ul>
                                </div>
                            </Card>
                        </Col>
                    </Row>
                </Card>
            </Col>
        </Row>
        <Drawer title="查询可选择报告" :width="350" v-model="isDrawer">
            <Form :label-width="80">
                <FormItem label="报告名称:">
                    <Input placeholder="请输入报告名称"/>
                </FormItem>
                <FormItem label="报告分类:">
                    <Select></Select>
                </FormItem>
                <FormItem label="报告级别:">
                    <Select></Select>
                </FormItem>
            </Form>
            <div class="demo-drawer-footer">
                <Button style="margin-right: 8px" @click="isDrawer = false">Cancel</Button>
                <Button type="primary" @click="isDrawer = false">Submit</Button>
            </div>
            </Drawer>
    </div>
</template>

<script>
import Sortable from 'sortablejs';
export default {
    name:'hytlAddReport',
    data(){
        return {
            total:0,
            pageIndex:1,
            isDrawer:false,
            todoArray: [
                {
                    content: '完成iview-admin基本开发'
                },
                {
                    content: '对iview-admin进行性能优化'
                },
                {
                    content: '对iview-admin的细节进行优化'
                },
                {
                    content: '完成iview-admin开发'
                },
                {
                    content: '解决发现的bug'
                },
                {
                    content: '添加更多组件'
                },
                {
                    content: '封装更多图表'
                },
                {
                    content: '增加更多人性化功能'
                }
            ],
            doArray:[]
        }
    },
    mounted () {
        document.body.ondrop = function (event) {
            event.preventDefault();
            event.stopPropagation();
        };
        let vm = this;
        let todoList = document.getElementById('todoList');
        Sortable.create(todoList, {
            group: {
                name: 'list',
                pull: true
            },
            animation: 120,
            ghostClass: 'placeholder-style',
            fallbackClass: 'iview-admin-cloned-item',
            onRemove (event) {
                vm.doArray.splice(event.newIndex, 0, vm.todoArray[event.item.getAttribute('data-index')]);
            }
        });
        let doList = document.getElementById('doList');
        Sortable.create(doList, {
            group: {
                name: 'list',
                pull: true
            },
            sort: false,
            filter: '.iview-admin-draggable-delete',
            animation: 120,
            fallbackClass: 'iview-admin-cloned-item',
            onRemove (event) {
                vm.doArray.splice(event.oldIndex, 1);
            }
        });
    },
    methods:{
        changePage(){},

    }
}
</script>

