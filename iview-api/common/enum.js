// 常用字符串定义成枚举
var ENUM = {}

ENUM.COUNTER = {
    ReportId:'ReportIdSeqGenerator',
    AccountId:'AccountInfoSeqGenerator',
    CategoryId:'CategoryIdSeqGenerator',
    PermissionId:'PermissionIdSeqGenerator'
}

ENUM.Sort = {
    ASC : 1,
    DESC : -1
}

module.exports = ENUM;
