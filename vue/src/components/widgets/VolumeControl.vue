<template>
    <div v-if="format === 2" class="d-flex ga-2 justify-center w-100">
        <v-btn icon="mdi-volume-off" variant="text" density="comfortable" class="my-auto" color="grey"></v-btn>
        <v-btn icon="mdi-volume-low" variant="text" density="comfortable" class="my-auto"></v-btn>
        <v-btn icon="mdi-volume-high" variant="text" density="comfortable" class="my-auto"></v-btn>
    </div>
    <div v-else-if="format === 1" class="d-flex ga-2 w-100">
        <v-btn icon="mdi-volume-off" variant="text" density="comfortable" class="my-auto" color="grey"></v-btn>
        <v-btn icon="mdi-volume-low" variant="text" density="comfortable" class="my-auto"></v-btn>
        <v-progress-linear class="flex-grow-1 my-auto mx-2 vol-level" height="7" rounded="xl"></v-progress-linear>
        <v-btn icon="mdi-volume-high" variant="text" density="comfortable" class="my-auto"></v-btn>
    </div>
    <div v-else class="d-flex ga-2 w-100">
        <v-btn icon="mdi-volume-low" variant="text" density="comfortable" class="my-auto"></v-btn>
        <v-slider :hide-details="true" class="pe-3 my-auto vol-level"></v-slider>
    </div>
</template>

<script setup>

const props = defineProps(['format'])

</script>

<style scoped>
.vol-level{
    min-width: 270px !important;
}
</style>