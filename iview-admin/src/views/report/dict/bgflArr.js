// 缓存到内存中
import axios from 'axios';

function getCategory(type){
    axios.get('/Api/Report/GetCategory',{ params: { type:type } }).then(result=>{
        let obj = [];
        if(result.data.code == 0) obj = JSON.parse(result.data.data);
        return obj;
    })
}

export let bgflArr = getCategory(1)

export let hyflArr = getCategory(2)

export let ztflArr = getCategory(3)