<template>
    <v-app-bar class="background-theme-color" elevation="0">
        <v-app-bar-nav-icon variant="text" icon="mdi-chevron-left"></v-app-bar-nav-icon>
        
        <v-spacer></v-spacer>

        <v-btn icon="mdi-close" variant="text"></v-btn>
        
    </v-app-bar>
    <v-navigation-drawer
        v-model="drawer"
        temporary
      >
        <v-list
          :items="items"
        ></v-list>
      </v-navigation-drawer>
</template>

<script setup>

import { ref, watch } from 'vue'

  const items = [
    {
      title: 'Foo',
      value: 'foo',
    },
    {
      title: 'Bar',
      value: 'bar',
    },
    {
      title: 'Fizz',
      value: 'fizz',
    },
    {
      title: 'Buzz',
      value: 'buzz',
    },
  ]

  const drawer = ref(false)
  const group = ref(null)

  watch(group, () => {
    drawer.value = false
  })

</script>

<style scoped>

</style>