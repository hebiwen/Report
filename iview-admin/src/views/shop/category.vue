<template>
    <div> 
        <Tree :data="categoryTree" :render="renderContent"></Tree>
    </div>
</template>
<script>

export default {
    data () {
        return {
            editState:false,
            buttonProps: {
                type: 'primary',
                size: 'small',
            },
            inputContent:"",    // 输入框要修改的内容
            oldName:"",         // 修改前的TreeNode名称
            categoryTree:[],
            level:1
        }
    },
    mounted(){
        this.getNewCategory();
    },
    methods: {
        //获取分类树数据,此方法与下面获取树结构的方法在数据库中表结构不同，前者不利于做增删改操作
        getCategory(){
            this.$http.get('/api/admin/categoryList').then(result=>{
                if(result.data.length>0){
                     this.categoryTree = this.categoryTreeData(result.data);
                }
            })
        },
        getNewCategory(){
            this.$http.get('/api/admin/categoryListNew').then(result=>{
                if(result.data.length>0){
                    this.categoryTree = this.getTreeData(result.data);
                }
            })
        },
        getTreeData(arr){
            let result = [];
            arr.forEach(item => {
                if(item.ParentId==0){
                    result.push({ 
                        title:item.CategoryName,
                        id:item._id,
                        selfCode:item.SelfCode,
                        children:this.getChildren(item._id,arr)});
                }
            })
            return result;
        },
        getChildren(id,arr){
            let dd = [];
            arr.forEach(item=>{
                if(item.ParentId == id){
                    dd.push({ 
                        title : item.CategoryName,
                        id:item._id,
                        selfCode:item.SelfCode,
                    })
                }
            })
            dd.forEach(child=>{
                let childArr = this.getChildren(child.id,arr);
                if(childArr.length>0) child.children = childArr;
            })
            return dd;
        },
        categoryTreeData(arr){
            var result = [];
            arr.forEach((item)=>{
                let dataItem = {}; 
                if(item.Childrens && item.Childrens.length >0 ){
                    dataItem = {title:item.CategoryName,children:this.CategoryData(item.Childrens),level:item.level};
                }else{
                    dataItem = { title:item.CategoryName,expand:true,level:item.level }
                }
                result.push(dataItem);
            })
            return result;
        },
        

        // 树渲染逻辑
        renderContent (h, { root, node, data }) { 
            return h("span", {
                    class:"hhhaha",
                    style: { 
                        display: 'inline-block',
                        lineHeight:'1.6rem',   
                        width: '100%', 
                        cursor: 'pointer' 
                    },
                    on:{
                        click:()=>{
                            // 点击Tree节点触发
                            data.editState ? '' :  this.handleClickTreeNode(data)
                        }
                    }
                }, [  
                        h('span', [
                            h('Icon', {
                                props: {
                                    type: `${data.children==undefined || data.children.length==0 ? 'md-document' :'ios-folder'}`
                                },
                                style: {
                                    marginRight: '8px', 
                                }
                            }),
                            h(`${ data.editState ? '' : 'span'}`, data.title),
                            h(`${ data.editState ? 'input' : ''}`, 
                                {
                                    attrs:{
                                        value:`${ data.editState ? data.title : ''}`, 
                                        autofocus :"true"
                                    },  
                                    style: {     
                                        width: '12rem', 
                                        cursor: 'auto' ,
                                    },
                                    on:{
                                        change:(event)=>{ 
                                            this.inputContent=event.target.value 
                                        }
                                    }
                                }
                            ),
                        ]), 
                        // 增删改按钮部分
                        h(`${ data.editState ? '' : 'span'}`,
                            {
                                class:"btnNone",
                                style: { marginLeft:'1rem' }
                            },
                            [
                                //操作按钮部分 
                                    // 编辑按钮
                                h('Button', {
                                    props: Object.assign({}, this.buttonProps, {
                                        // icon: 'ios-brush-outline',
                                        type:'primary'
                                    }),
                                    style: {
                                        marginRight: '8px',
                                        width: '40px',
                                        lineHeight: '0',
                                        padding:'0',  
                                        height: '1.4rem',
                                    },
                                    on: {
                                        click: () => { this.editTree(data) }
                                    }
                                },'编辑'),
                                    // 添加按钮
                                h('Button', {
                                    props: Object.assign({}, this.buttonProps, {
                                        // icon: 'ios-close',
                                        type:'primary',
                                    }),
                                    style: {
                                        marginRight: '8px',
                                        width: '40px',
                                        lineHeight: '0',
                                        padding:'0',  
                                        height: '1.4rem',
                                    },
                                    on: {
                                        click: () => { this.append(data) }
                                    }
                                },'添加'),
                                    // 删除按钮
                                h('Button', {
                                    props: Object.assign({}, this.buttonProps, {
                                        // icon: 'ios-remove',
                                        type:'primary'
                                    }),
                                    style: {
                                        marginRight: '8px',
                                        width: '40px',
                                        padding:'0', 
                                        lineHeight: '0', 
                                        height: '1.4rem',
                                    },
                                    on: {
                                        click: () => { this.remove(root, node, data) }
                                    }
                                },'删除')
                            ]
                        ),    
                        // 确认/取消修改部分
                        h(`${ data.editState ? 'span' : ''}`,
                            { 
                                style: { 
                                    marginLeft:'.5rem'
                                }
                            },
                            [  
                                // 确认按钮
                                h('Button', {
                                    props: Object.assign({}, this.buttonProps, {
                                        icon: 'md-checkmark',
                                        type:'primary'
                                    }),
                                    style: {
                                        marginRight: '8px',
                                        border:0,
                                        fontSize:'1rem',
                                        outline:"none"
                                    },
                                    on: {
                                        click: (event) => {  
                                            this.confirmTheChange(data) 
                                        }
                                    }
                                },'保存'),
                                // 取消按钮
                                h('Button', {
                                    props: Object.assign({}, this.buttonProps, {
                                        icon: 'md-close',
                                        type:'primary'
                                    }),
                                    style: {
                                        border:'0',
                                        fontSize:'1rem',
                                        outline:"none"
                                    },
                                    on: {
                                        click: () => { this.CancelChange(data) }
                                    }
                                },'取消') 
                            ]
                        ) 
                ]);
        },
        // 控制Tree当前状态函数
        setStates(data){
            var editState=data.editState
            if(editState){this.$set(data, 'editState', false);}
            else{this.$set(data, 'editState', true);}  
        },
        // Tree修改按钮
        editTree(data){
            event.stopPropagation()  
            this.inputContent=data.title 
            this.oldName=data.title
            this.setStates(data)  
        },
        // 添加按钮
        append (data) {
            event.stopPropagation()
            const children = data.children || [];
            let newTitle = '新建节点';
            children.push({
                title: newTitle,
                expand: true
            }); 
            var query = new URLSearchParams();
            query.append('categoryName',newTitle);
            query.append('parentId',data.id);
            query.append('selfCode',data.selfCode);
            this.$http.post('/api/admin/addCategory',query).then(result=>{
                if(result.data.ok == 1){
                    this.$set(data, 'children', children);
                }
            })
        },
        // 删除按钮
        remove (root, node, data) {
            event.stopPropagation()
                
            this.$Modal.confirm({
                title:"提示",
                content:`您确定删除 “${data.title}” 吗？`,
                onOk: () => {
                    var query = new URLSearchParams();
                    query.append('id',data.id);
                    this.$http.post('/api/admin/removeCategory',query).then(result=>{
                        if(result.data.ok == 1){
                            const parentKey = root.find(el => el === node).parent;
                            const parent = root.find(el => el.nodeKey === parentKey).node;
                            const index = parent.children.indexOf(data);
                            parent.children.splice(index, 1);
                            this.$Message.info('删除成功');
                        }
                    })
                },
                onCancel: () => {
                    this.$Message.info('取消');
                }
            });
        }, 
        // 确认修改树节点
        confirmTheChange(data){   
            if(!this.inputContent){
                this.$Notice.warning({
                    title: '当前输入有误', 
                });
            }else{ 
                if(this.oldName!==this.inputContent){  
                    this.$Modal.confirm({
                        title:"提示",
                        content:`您确定将  “${this.oldName}”  重命名为 “ ${this.inputContent} ” 吗？`,
                        onOk: () => {
                            this.editNode(data,this.inputContent)
                            
                            // data.title=this.inputContent 
                            // this.$Message.info('修改成功');
                        },
                        onCancel: () => {
                            this.$Message.info('取消');
                        }
                    });
                this.setStates(data);
                } else{
                    this.setStates(data);
                }
            }
        },
        // 取消修改树节点
        CancelChange(data){ 
            this.$Notice.info({
                title: '取消修改',
            });
            this.setStates(data)
        },
        // 点击Tree节点触发
        handleClickTreeNode(data){  
            console.log("当前点击:"+data.title)
        },
        editNode(data,inputText){
            var params = new URLSearchParams();
            params.append('id',data.id);
            params.append('newText',inputText);
            this.$http.post('/api/admin/editCategory', params).then(result => {
                if(result.data.ok == 1){
                    data.title = inputText;
                    this.$Message.info('修改成功');
                }
            })
        }

        }
    }
</script>

<style  >
.btnNone{
    display:none
}
/* .hhhaha:hover{color:aqua} */
.hhhaha:hover .btnNone{
   display: inline-block
}

.hhhaha:hover {
   font-weight: 600;
   color:#275cd4
}
.ivu-tree ul li {
    list-style: none;
    /* margin: 8px 0; */
    padding: 0;
    white-space: nowrap;
    outline: none;
}
</style>
