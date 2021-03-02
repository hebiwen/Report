export default [
    { id:1, title:'农、林、牧、渔业', sort:1, select:false },
    { id:2, title:'采矿业', sort:2, select:false },
    
    { id:3, title:'制造业', sort:3, expand:false, select:false, children:[ 
        { id:301, title:'农副食品加工业', sort:1, select:false },
        { id:302, title:'食品制造业', sort:2, select:false },
        { id:303, title:'酒、饮料和精制茶制造业', sort:3, select:false },
        { id:304, title:'烟草制品业', sort:4, select:false },
        { id:305, title:'纺织业', sort:5, select:false },
        { id:306, title:'纺织服装、服饰业', sort:6, select:false },
        { id:307, title:'皮革、毛皮、羽毛及其制品和制鞋业', sort:7, select:false },
        { id:308, title:'木材加工和木、竹、藤、棕、草制品业', sort:8, select:false },
        ]
    },
    { id:4, title:'建筑业', sort:4, select:false },
    { id:5, title:'批发和零售业', sort:5, select:false },
    { id:6, title:'交通运输、仓储和邮政业', sort:6, select:false },
    { id:7, title:'住宿和餐饮业', sort:7, select:false },
    { id:8, title:'信息传输、软件和信息技术服务业', sort:8, select:false },
    { id:9, title:'金融业', sort:9, select:false }
]