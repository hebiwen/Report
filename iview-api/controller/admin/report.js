var sql = require('../../mssqldb');
var helper = require('../../common/helper');

module.exports = {
    // 获取报告详情
    async getReport(req,res,next){
        try {
            var id = req.query.id || 'F9A4BCCB-872A-42B1-AE19-000007ED299E';
            if(helper.isNullOrEmpty(id)) throw "参数不能为空.";
            var sqlStr = `select * from resource_info where rec_id = '${ id }'`;
            let result = await sql.query(sqlStr);
            res.send({ status : 0, data : result.data.recordset });
        } catch (error) {
            console.log(error);
            res.send({ status : 1 ,msg : "操作失败." });
        }
    },
    // 获取报告分页
    async getPageReport(req,res,next){
        try {
            var pageIndex = req.query.pageIndex || 1;
            var title = req.query.title;
            var category = req.query.category;

            var filter = `WHERE 1 = 1 `;
            if(!helper.isNullOrEmpty(title)) filter += `AND Title LIKE '%${ title }%' `;
            if(!helper.isNullOrEmpty(category)) filter += `AND CategoryName = '${ category }' `;
            var sqlCount = `SELECT COUNT(1) AS Total FROM V_Report ${ filter }`;
            var sqlStr =  `SELECT * FROM V_Report ${ filter }`
                    + `ORDER BY PublishDate DESC `
                    + `OFFSET ${ (pageIndex-1) * helper.pageSize } ROWS `
                    + `FETCH NEXT ${ helper.pageSize } ROWS ONLY`;

            let count = await sql.query(sqlCount);
            let list = await sql.query(sqlStr);
            res.send({ status : 0,total : count.data.recordset[0].Total,data : list.data.recordset });
        }
        catch (error) {
            console.log(error);
            res.send({ status:1,msg:"操作失败." });
        }
    },
    async addReport(req,res,next){
        try {
            
        } catch (error) {
            
        }
    },
    async editReport(req,res,next){
        try {
            var id = req.params.id;
            var title = req.params.title;
            var sqlCheckTitle = `SELECT COUNT(1) AS Total FROM dbo.resource_info `
                              + `WHERE rec_id != '${ id }' AND x_title = '${ title }'`;
            var checkTitle = await sql.query(sqlCheckTitle);
            if(checkTitle.data.recordset[0].Total > 0) throw "标题已存在";
            var sqlStr = `UPDATE dbo.resource_info SET x_title = '${ title }' WHERE rec_id = '${ id }'`;
            var result = await sql.query(sqlStr);
            if(!result.data.status) throw result.data.msg;
            res.send({ status : 0 , msg : '操作成功.' });
        } catch (error) {
            console.log(error);
            res.send({ status : 1 , msg : error });
        }
    }
}

