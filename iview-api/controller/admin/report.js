var db = require('../../mssqldb');

module.exports = {
    async getPageReport(req,res,next){
        try {
            var sql = "SELECT TOP 100 rec_id AS id ,x_title AS Title,x_reportId AS Category,x_verifystate AS [Status],x_manager AS Author,x_publishdate AS PublishDate FROM dbo.resource_info";
            await db.executeSql(sql,function(err,doc){
               if(err){
                   console.log("get report error:"+err);
               }
               let result = { stats:0,result:doc.recordset};
               res.send(result);
           })
        }
        catch (error) {
            console.log(error)
        }
    }
}

