<template>
    <v-app class="h-100 d-flex">
        <!--<Header :showQuickActions="isRemote ? false : true" :showMediaPower="isRemote ? true : false" />-->
        <Body class="flex-grow-1 overflow-y-auto h-100" />
        <Footer />
    </v-app>
</template>

<script setup>
import Body from '../components/layout/Body'
import Header from '../components/layout/Header'
import Footer from '../components/layout/Footer'

</script>

<style></style>