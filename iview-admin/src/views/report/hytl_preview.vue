<template desc="题录预览">
    <div>
        <Row class="top">
            <h2>行业研究报告题录</h2>
            <h3>{{title}}</h3>
            <p>（报告加工时间：2021-04-06 -- 2021-04-11）</p>
        </Row>
        <Row>
            <List>
                <ListItem>
                    <ListItemMeta avatar="https://dev-file.iviewui.com/userinfoPDvn9gKWYihR24SpgC319vXY8qniCqj4/avatar" title="This is title" description="This is description, this is description." />
                </ListItem>
                <ListItem>
                    <ListItemMeta avatar="https://dev-file.iviewui.com/userinfoPDvn9gKWYihR24SpgC319vXY8qniCqj4/avatar" title="This is title" description="This is description, this is description." />
                </ListItem>
                <ListItem>
                    <ListItemMeta avatar="https://dev-file.iviewui.com/userinfoPDvn9gKWYihR24SpgC319vXY8qniCqj4/avatar" title="This is title" description="This is description, this is description." />
                </ListItem>
            </List>
        </Row>
        <Row>
            <p>如果没有您需要的报告，您可以到行业研究报告数据库（<a href="/Home/Index" target="_blank">http://hybg.hbstl.org.cn</a> ）查找或定制</p>
            <p>如果您在使用中有任何问题，请及时反馈给我们。</p>
        </Row>
    </div>
</template>

<script>
export default {
    name:'tlPreview',
    props:{
        id:{
            type:Number
        }
    },
    data(){
        return {
            id:this.props.id,
            title:'dddd'
        }
    },
    mounted(){
        this.getHytl();
    },
    methods:{
        getHytl(){
            console.log("id:"+this.id);
        }
    }
}
</script>

<style>
 .top{ border-bottom: 3px solid #FF0000; text-align: center;}
 .top h2{ 
    font-size: 24px;
    font-weight: 900;
    color: red;
    line-height: 30px;}
.top h3,p{ margin: 10px;
    font-size: 20px;
    font-family: "宋体"; }
</style>