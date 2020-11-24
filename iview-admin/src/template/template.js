
const editButton = (_this,h,params) => {
    return h('Button', {
        props: { type:'primary',size:'small'},
        style: { marginRight:'5px' },
        on:{ 'click':()=>{ _this.showEditModal(params.row._id) } }
    }, '编辑');
};

const deleteButton = (_this,h,params) => {
    return h('Poptip', {
        props:{ confirm:true,title:'您确认要删除这条数据吗?',transfer:true },
        on:{ 'on-ok':()=>{ _this.remove(params.row._id) } }
    }, [
        h('Button',{
            props:{ type:'error',size:'small',placement:'top'}
        },'删除')
    ]);
};

const addChildButton = (_this,h,params) => {
    return h('Button', {
        props: { type:'success',size:'small'},
        style: { marginRight:'5px' },
        on:{ 'click':()=>{ _this.$router.push({ path:'permission_child',query:{ pid : params.row._id } })  } }
    }, '添加子菜单');
};

// export 可以导出多个函数
// export default 只可以导出一个默认函数
export { editButton ,deleteButton,addChildButton }

