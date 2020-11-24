<template>
    <div style="width:calc(100% - 10px);height:305px;" id="home_page_map"></div>
</template>

<script>
import echarts from 'echarts';

export default {
    name: 'homeMap',
    props: {
        cityData: Array
    },
    mounted () {
        var map = echarts.init(document.getElementById('home_page_map'));
        const chinaJson = require('../map-data/china.json');
        echarts.registerMap('china', chinaJson);
        map.setOption({
            width:750,
            height:305,
            tooltip: {
                trigger: 'item',
                formatter: '{b}<br/>{c}'
            },
            visualMap: {
                min: 0,
                max: 1000,
                itemWidth:20,
                itemHeight:100,
                text: ['High', 'Low'],
                realtime: false,  //是否实时更新
                calculable: true, //是否显示拖拽用的手柄（手柄能拖拽调整选中范围）
                inRange: {        //定义 在选中范围中 的视觉元素
                    color: ['lightskyblue', 'yellow', 'orangered'],
                    // symbolSize: [30, 100]
                }
            },
            grid:{
                top:0,
                bottom:0,
                left:'2%',
                right:'2%',
                containLabel: true
            },
            series: [{
                    type: 'map',
                    mapType: 'china', // 自定义扩展图表类型
                    label: {
                        show: true
                    },
                    data: this.cityData
                }]
            }
        );
        window.addEventListener('resize', function () {
             map.resize();
        });
    }
};
</script>


