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

mssql.on('error',err =>{
    console.log("connect to mssql error:" + err);
})

var sql = {};

let pool = null;
async function initPool(){
    if(pool == null){
        pool = await new mssql.ConnectionPool(dbConfig).connect();
    }
    return pool;
}

sql.queryParams = async (sqlStr,params) => {
    try {
        await initPool(); //连接数据库
        let request = pool.request();
        request.multiple = true;
        if(params != null){
            for(let index in params){
                request.input(index,params[index].sqlType,params[index].inputValue);
            }
        }
        let result = await request.query(sqlStr);
        return { status : true, data : result };
    } catch (error) {
        console.log(sqlStr,error);
        return { status : false, data : error};
    }
}

sql.query = async (sqlStr) => {
    try {
        await initPool(); //连接数据库
        let request = pool.request();
        let result = await request.query(sqlStr);
        return { status : true, data : result };
    } catch (error) {
        console.log(sqlStr,error);
        return { status : false, data : error};
    }
}

module.exports = sql;
