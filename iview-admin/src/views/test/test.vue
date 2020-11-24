<template description='测试页面'>
   <div>
      <div>FTP上传</div>
      <input type="file" @change="uploadFTP" />
      <div>MD5上传</div>
      <input type="file" @change="uploadMD5" />
   </div>
</template>
<script>
import sparkMD5 from 'spark-md5';

const chunkSize = 2 * 1024 * 1024;
const blobSlice = File.prototype.slice || File.prototype.mozSlice || File.prototype.webkitSlice;

export default {
   name: 'test',
   data() {
       return {}
   },
  mounted() {},
  methods: {
     uploadFTP(){
        console.log('null')
     },
     async uploadMD5(e){
      const file = e.target.files[0];
      const blockCount = Math.ceil(file.size / chunkSize); // 分片总数
      const axiosPromiseArray =[]; // 分组上传
      const hash = await this.hashFile(file);

      console.log(hash);

      for(let i=0;i<blockCount;i++){
        const start = i * chunkSize;
        const end = Math.min(file.size,start + chunkSize);

        const form = new FormData();
        form.append('file',blobSlice.call(file,start,end));
        form.append('name',file.name);
        form.append('total',blockCount);
        form.append('index',i);
        form.append('hash',hash);
        form.append('size',file.size);

        const axiosOptions = {
          headers:{ "Content-Type":"multipart/form-data" },
          opUploadProgress : function(event){
            // 处理上传进度
            console.log(blockCount,i,event,file);
          }
        }
        // 加入到Promise数组中
        axiosPromiseArray.push(this.$http.post('/api/index/uploadChunk',form,axiosOptions));
      }

      // 所有分片上传后，请求合并分片文件
      await this.$http.all(axiosPromiseArray).then(() => {
        // 合并chunks
        let query = new URLSearchParams();
        query.append('name',file.name);
        query.append('total',blockCount);
        query.append('hash',hash);
        this.$http.post('/api/index/mergeChunk',query).then(result => {
          console.log(result.data,file);
        })
      })

    },
    hashFile(file){
      return new Promise((resolve,reject) => {
        var spark = new sparkMD5.ArrayBuffer();
        var currentChunk = 0;
        var chunks = Math.ceil(file.size / chunkSize);

        const fileReader = new FileReader();
        // fileReader.readAsArrayBuffer(file);
        fileReader.onload = function(event){
          console.log(event.target.result);

          const result = event.target.result;
          spark.append(result);
          currentChunk++;
          if(currentChunk < chunks){
            loadNext();
            console.log(`第${currentChunk}分片解析完成，开始解析${currentChunk + 1}分片`);
          }else{
            const md5= spark.end();
            const spkmd5 = new sparkMD5();
            spkmd5.append(md5);
            spkmd5.append(file.name);
            const hexHash = spkmd5.end();
            resolve(hexHash);
            console.log('解析完成');
          }
        }
        fileReader.onerror = function(){
          console.log('读取文件失败')
        }
        function loadNext(){
          var start = currentChunk * chunkSize;
          var end = start + chunkSize > file.size ? file.size : (start + chunkSize);
          fileReader.readAsArrayBuffer(blobSlice.call(file,start,end));
        }
        loadNext();
      })
    }
  }

}
</script>
<style scoped>
</style>


