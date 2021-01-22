var mssql = require('mssql');
var dbConfig = {
    user:'sa',
    password:'123456',
    server:'192.168.21.188',
    database:'ReportMgr',
    port:1433,
    // options:{
    //     encrypt:true // use this if you're on windows azure
    // },
    pool:{
        min:0,
        max:10,
        idleTimeoutMillis: 3000
    }
}

var db = {
    executeSql : function(sql,callback){
        new mssql.ConnectionPool(dbConfig).connect().then(pool => {
            let ps = new mssql.PreparedStatement(pool);
            ps.prepare(sql,err=>{
                if(err){
                    console.log('connect pool error:'+err);
                    return;
                }
                ps.execute('',(err,result) => {
                    if(err) return;
                    ps.unprepare(err => {
                        if(err) {
                            callback(err,null);
                            return;
                        }
                        callback(err,result)
                    })
                })
            })
        }).catch(err => {
            console.log("connect to mssql error:"+err)
        })
    },
    resultSql:function(sql){
        var pool = new mssql.ConnectionPool(dbConfig);
        pool.connect();
        var request = pool.request();
        var result = request.query(sql);
        return result;
    }
};


module.exports = db;
