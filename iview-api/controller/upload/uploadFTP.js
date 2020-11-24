// FTP上传文件
var path = require('path');
var fs = require('fs');
var ftp = require('ftp');
const client = new ftp();

client.on('ready',()=>{
    console.log("ftp client  is ready")
});
client.on('error',(err)=>{
    console.log("ftp connect error:"+JSON.stringify(err));
})
client.connect({
    host:'134.175.93.211',
    port:21,
    user:'admin',
    password:'123456',
    keepalive:1000
})

async function uploadFile(currentFile,targetFile){
    let dirPath = path.dirname(targetFile);
    let fileName = path.basename(targetFile);
    const rs = fs.createReadStream(currentFile);
    let { err : ea,dir } = await processCWD(dirPath);
    if(ea){
        return Promise.resolve({ err:ea });
    }
    return new Promise((resolve,reject)=>{
        client.put(rs,fileName,(err)=>{
            resolve({err:err});
        })
    })
};
async function processCWD(dirPath){
    return new Promise((resolve,reject)=>{
        client.cwd(dirPath,(err,dir)=>{
          resolve({err:err,dir:dir});
        })
    })
}

// module.exports内的调用函数必须写在外面
module.exports = {
    async uploadFTP(){
        let { err, eb } = await uploadFile('H:/Images/xh.jpg','upload/xh.jpg');
        if(eb){
            console.log("error:"+eb);
            return
        }
        console.log("success")
    }
}

