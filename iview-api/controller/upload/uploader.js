const logger = require("../../common/log");

module.exports = {
    // 单文件上传
    async uploadSingle(req,res,next){
        try {
            await res.send(req.file.path);
        } catch (error) {
            logger.log("");
        }
    },
    // 多文件上传
    async uploadMulter(req,res,next){
        try {
            await res.send(req.files[0].path);
        } catch (error) {
            logger.log("");
        }
    },
    // base64压缩上传
    async uploadBase64(req,res,next){
        try {
            let file = req.body.file;
            let base64Data = file.replace(/^data:image\/\w+;base64,/, '');
            let buffer = new Buffer(base64Data,'base64');
            //var path = uploadFolder.replace('./','') + util.formatDate(new Date(),"yyyyMMdd") +".jpg";
            var path = 'dd.jpg';
            await fs.writeFile(path,buffer,function(err){
                if(err) throw err;
                result.fileName = path; 
                res.send(result);
            })
        } catch (error) {
            logger.log("");
        }
    }
}


