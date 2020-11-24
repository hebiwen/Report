var log4js = require('log4js')
var common = require('./common')
var path = require('path')

let baseLog = path.resolve(__dirname,'./../logs'); 
let logPath = baseLog + '/' + new Date().format('yyyyMMdd') + '/' + new Date().format('yyyyMMdd');

log4js.configure({
    appenders:{
        //设置控制台输出 （默认日志级别是关闭的（即不会输出日志））
        console:{ type : 'console'},
        //设置每天：以日期为单位,数据文件类型，file   注意设置pattern，alwaysIncludePattern属性
        writeLog:{ type :'file',filename:logPath,pattern:'log',alwaysIncludePattern:true }
    },
    // 不同等级的日志追加到不同的输出位置：appenders: ['out', 'allLog']  categories 作为getLogger方法的键名对应
    categories:{
        writeLog: { appenders: ['console', 'writeLog'], level: 'info' },
        default: { appenders: ['console', 'writeLog'], level: 'info' }
    }    
})

// getLogger参数取categories项,为空或者其他值取default默认项
const logger = log4js.getLogger('writeLog');

module.exports = logger;


