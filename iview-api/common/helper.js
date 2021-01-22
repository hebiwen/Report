var util = {};

util.defaultPwd = '123456';

util.pageSize = 20;

util.currDate = new Date();

util.isNullOrEmpty = function(obj){
    if(typeof obj == 'undefined' || obj == null || obj == '' ){
        return true;
    }else{
        return false;
    }
}

util.TextValueCheck = function(value){
    var result = { status : true, message : [] };
    
    return result;
}

// NBA final, laker, 总冠军
// var obj={
//     name: "laker",
//     msg: "总冠军"
//   }
//   var message1=`NBA final, ${obj.name}, ${obj.msg}` 

module.exports = util;
