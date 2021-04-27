<template>
  <div id="app" class="app">
    <router-view></router-view>
  </div>
</template>

<script>
export default {
  name: 'App',
  data(){
    return {
      theme:this.$store.state.app.themeColor
    }
  },
  mounted () {

  },
  beforeDestroy () {

  },
  methods: {

  }
}
</script>

