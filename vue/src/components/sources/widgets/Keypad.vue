<template>
    <div class="d-flex flex-column h-100 w-100 ga-2 justify-center">
        <div class="d-flex w-100 justify-center ga-2">
            <v-btn :size="size" rounded="xl">1</v-btn>
            <v-btn :size="size" rounded="xl">2</v-btn>
            <v-btn :size="size" rounded="xl">3</v-btn>
        </div>
        <div class="d-flex w-100 justify-center ga-2">
            <v-btn :size="size" rounded="xl">4</v-btn>
            <v-btn :size="size" rounded="xl">5</v-btn>
            <v-btn :size="size" rounded="xl">6</v-btn>
        </div>
        <div class="d-flex w-100 justify-center ga-2">
            <v-btn :size="size" rounded="xl">7</v-btn>
            <v-btn :size="size" rounded="xl">8</v-btn>
            <v-btn :size="size" rounded="xl">9</v-btn>
        </div>
        <div class="d-flex w-100 justify-center ga-2">
            <v-spacer></v-spacer>
            <v-btn :size="size" rounded="xl">0</v-btn>
            <v-spacer></v-spacer>
        </div>
    </div>
</template>

<script setup>

const props = defineProps(['size'])

</script>

<style scoped>

</style>