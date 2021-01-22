var express = require('express');
var router = express.Router();

var report = require('../controller/admin/report');


/* get请求使用req.query获取参数，post请求使用req.body获取参数 */
/* GET users listing. */
router.get('/', function(req, res, next) {
  res.send('admin api responsed success');
});

//#region 报告相关
router.get('/reportList',report.getPageReport);
router.get('/report',report.getReport);
//#endregion

module.exports = router;
