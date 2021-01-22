<template desc="题录预览">
    <div>
        <p>题录ID：{{ id }} </p> 
    </div>
</template>

<script>
export default {
    name:'tlPreview',
    props:{
        id:{
            type:Number
        }
    },
    data(){
        return {
            
        }
    }
}
</script>