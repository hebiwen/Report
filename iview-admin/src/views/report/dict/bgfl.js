// 报告分类
export default [
    
    // { parent : "市场分析报告", 
    //   children:[
    //     { value:"4E954640-960E-4335-9E20-7639CE5D21D2",label:"境内分析报告" },
    //     { value:"4E954640-960E-4335-9E20-7639CE5D21D2",label:"境外分析报告" }
    //   ]
    // },
    // { parent : "技术研究报告",
    //   children:[
    //       { value:"09C0FC4F-74A3-4C63-A7EA-1717B18A1ED3",label:"中文技术报告" },
    //       { value:"D4CE0372-9E37-4F59-9465-CF344B6E63C7",label:"外文技术报告" }
    //     ] 
    // },
    // { parent:'其它',
    //   children:[
    //     { value:"B6FF03FC-D90A-41D2-B6F5-467EA1EC9C69",label:"行业资讯" },
    //     { value:"928F8375-96BA-4B8A-94CF-325E77A2AF46",label:"投资分析报告" },
    //     { value:"80EFBF4F-D625-4EFD-9B2D-BE5374E57DA6",label:"政策环境报告" },
    //     { value:"FF691863-1344-4810-85B7-C6C937ED6F1E",label:"综合分析报告" }
    //   ]
    // }
    { parent : "市场分析报告", 
      children:[
        { value:12,label:"境内分析报告" },
        { value:13,label:"境外分析报告" }
      ]
    },
    { parent : "技术研究报告",
      children:[
          { value:15,label:"中文技术报告" },
          { value:16,label:"外文技术报告" }
        ] 
    },
    { parent:'其它',
      children:[
        { value:10,label:"行业资讯" },
        { value:17,label:"投资分析报告" },
        { value:18,label:"政策环境报告" },
        { value:19,label:"综合分析报告" }
      ]
    }
]