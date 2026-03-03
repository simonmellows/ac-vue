<template>
    <div class="d-flex flex-wrap ga-2 justify-center mx-auto overflow-y-scroll" style="max-width: 200px;">
        <v-btn class="flex-grow-1 w-100" v-for="(button, index) in buttons">{{ button }}</v-btn>
    </div>
</template>

<script setup>

const props = defineProps(['buttons'])

</script>

<style scoped>

</style>