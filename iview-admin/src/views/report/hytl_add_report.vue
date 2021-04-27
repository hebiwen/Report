<style lang="less">
    @import '../../styles/common.less';
</style>

<template>
    <div style="margin:10px 0;">
    <Row>
    <Card>
        <elt-transfer
        ref="eltTransfer"
        :show-query="true"
        :show-pagination="true"
        :pagination-call-back="paginationCallBack"
        :left-columns="leftColumns"
        :title-texts="['待选','已选']"
        :button-texts="['添加','删除']"
        :query-texts="['筛选','筛选']"
        :table-row-key="row => row.id"
        v-model="tableData"
        >
      <!-- 可以使用插槽获取到列信息和行信息，从而进行数据的处理 -->
      <template v-slot:default="{scope}">
        <div>
          <span >{{scope.row[scope.col.id]}}</span>
        </div>
      </template>
      <template v-slot:leftCondition="{scope}">
        <el-form-item label="分类:">
          <el-select v-model="scope.category" size="mini" clearable placeholder="请选择">
            <el-option-group v-for="group in lstCategory" :key="group.title" :label="group.title">
              <el-option v-for="item in group.children" :key="item.id" :label="item.title" :value="item.id"></el-option>
            </el-option-group>
          </el-select>
        </el-form-item>
        <el-form-item label="级别:">
          <el-select v-model="scope.level" size="mini" clearable placeholder="请选择" style="width:100px">
            <el-option v-for="item in lstLevel" :key="item.value" :label="item.name" :value="item.value"></el-option>
          </el-select>
        </el-form-item>
      </template>
      <template v-slot:rightCondition="{scope}">
        <el-form-item label="名称">
          <el-input v-model="scope.id" size="mini" placeholder="名称"></el-input>
        </el-form-item>
      </template>
    </elt-transfer>
    </Card>
    </Row>
    </div>
</template>

<script>
import util from '../../libs/util'
import eltTransfer from '../my-components/eltTransfer'

export default {
    name:'hytlAddReport',
    data(){
        return {
            lstReport:[],
            total:0,
            pageIndex:1,
            tableData: [],
            leftColumns: [
              {label: 'ID', id: 'id', width: '60px'},
              {label: '报告名称', id: 'title' },
              {label: '发布日期', id: 'publishDate',width: '100px'}
            ],
            lstCategory:[
              { title:'市场分析报告',
                children:[
                  {id:12,title:'境内分析报告'},
                  {id:13,title:'境外分析报告'}
                ]
              },{ title:'技术研究报告',
                  children:[
                    {id:15,title:'中文技术报告'},
                    {id:16,title:'外文技术报告'}
                  ]
              },{ title:'其它',
                  children:[
                    {id:10,title:'行业资讯'},
                    {id:17,title:'投资分析报告'},
                    {id:18,title:'政策环境报告'},
                    {id:19,title:'综合分析报告'}
                  ]}
            ],
            lstLevel:[{value:1,name:'一级'},{value:2,name:'二级'},{value:3,name:'三级'}]
        }
    },  
    components: {
      'elt-transfer': eltTransfer
    },  
    methods:{
    async getReport(obj){
        let level = !obj.level ? '':obj.level;
        let category = !obj.category ? '':obj.category;
        var _params = { level:level,category:category,pageIndex:obj.pageIndex,pageSize:obj.pageSize };
        await this.$http.get("/Api/Report/GetAllReport",{params : _params}).then(result=>{
            if(result.data.code == util.error) return;
            let arrReport = JSON.parse(result.data.data);
            let arr = [];
            arrReport.forEach(el => {
                arr.push({ id:el.id,title:el.title,publishDate:el.publishDate })
            })  
            this.lstReport = arr;
            this.total = result.data.total;
        }).catch(err => {
            console.log("error:"+err);
        })
    },

    async paginationCallBack(obj) {
        // console.log(obj)
        await this.getReport(obj);
        
        return new Promise(((resolve, reject) => {
          try {
            resolve({total: this.total, data: this.lstReport})
          } catch {
            reject()
          }
        }))
      },
      clearTransfer() {
        this.$refs.eltTransfer.clear()
      }
    
    }
}
</script>

