var express = require('express');
var router = express.Router();
var fs = require('fs');
var multer = require('multer');
var path = require('path');

var common = require('../common/common');

console.log('_dirname:'+__dirname);
console.log('_filename:'+ __filename);
console.log('./:' + path.resolve('./') + path.resolve('./upload'))
console.log('cwd:'+ process.cwd())

var createFolder = function(folder){
   try{
     if(fs.existsSync(folder)){
       fs.accessSync(folder);
     }else{
       fs.mkdirSync(folder); // 同步创建文件夹
     }
   }catch(e){
      console.log('createFolder err:'+e)
   }
 }
 /** 后期考虑做成树形文件夹结构 */
 var uploadFolder = path.resolve('./uploadFolder/' + new Date().format('yyyyMMdd')) ;
 console.log("UploadFolder:" + uploadFolder);
 createFolder(uploadFolder);
 
 var storage = multer.diskStorage({
   destination:function(req,file,cb){
     cb(null,uploadFolder);
   },
   filename:function(req,file,cb){
     cb(null,file.originalname);
   }
 })

const upload = multer({ dest : uploadFolder });
// const upload = multer({ storage : storage }); // 不知道与上面那种有什么区别

var uploader = require('../controller/upload/uploader');
var uploadFTP = require('../controller/upload/uploadFTP');

/* GET home page. */
router.get('/', function(req, res, next) {
   res.render('index', { title: 'upload file api' });
});

//#region 文件上传
router.post('/uploadSingle',upload.single('file'),uploader.uploadSingle);
router.post('/uploadArray',upload.array('files',5),uploader.uploadMulter);
// UploadBase64 前端要么用Blob上传要么用 Base64 后台方法中一定要加Upload.singer()
router.post('/uploadBase64',upload.single('file'), async function(req,res,next){
  let file = req.body.file;
  let fileName = req.body.fileName;
  let base64Data = file.replace(/^data:image\/\w+;base64,/, '');
  let buffer = new Buffer(base64Data,'base64');
  var path = uploadFolder.replace('./','') + '\\' + fileName;
  await fs.writeFile(path,buffer,function(err){
      if(err) throw err;
        res.send(path);
  })
});
//#endregion

//#region FTP大文件上传
router.get('/uploadFTP',uploadFTP.uploadFTP);
//#endregion

//#region sparkMD5分片上传
router.post('/uploadChunk',upload.single('file'),function(req,res,next){
  var hash = req.body.hash;
  var index = req.body.index;

  var chunkPath = path.join(uploadFolder,hash,'/');
  if(!fs.existsSync(chunkPath)) fs.mkdirSync(chunkPath);
  fs.renameSync(req.file.path,chunkPath + hash + '_'+ index);
  res.send('Upload Chunks Success..');
})

router.post('/mergeChunk',function(req,res,next){
  var hash = req.body.hash;
  var name = req.body.name;
  var total = req.body.total;

  var chunkPath = path.join(uploadFolder,hash,'/');
  var filePath = path.join(uploadFolder,name);
  var chunks = fs.readdirSync(chunkPath);

  fs.writeFileSync(filePath,'');
  if(chunks.length != total || chunks.length == 0){
    res.send('Chunks Is Null Or Loss..');
    return;
  }
  for(var i = 0; i < total; i++){
    let fileName = chunkPath + hash + '_' + i;
    fs.appendFileSync(filePath,fs.readFileSync(fileName));
    fs.unlinkSync(fileName);
  }
  fs.rmdirSync(chunkPath);
  res.send(filePath);

})
//#endregion

module.exports = router;