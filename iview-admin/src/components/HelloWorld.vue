<template>
  <div class="hello">
    <h1>{{ msg }}</h1>
    <h2>Essential Links</h2>
    <ul v-for="item in Categorys">
      <li>{{ item.CategoryName }}</li>
    </ul>
  </div>
</template>

<script>
export default {
  name: 'HelloWorld',
  data () {
    return {
      msg: 'Welcome to Your Vue.js App',
      Categorys:[]
    }
  },
  mounted:function(){
    this.getCategory()
  },
  methods:{
    getCategory(){
      this.$http.get('/home').then(result=>{
        console.log(result);
        this.Categorys = result.data;
      }).catch(err=>{
        console.log("home get data error:"+err);
      })
    }
  }
}
</script>

