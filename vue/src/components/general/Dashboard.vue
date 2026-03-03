<template>
    <v-card variant="text" class="h-100 text-start d-flex flex-column mh-100 flex-grow-1">
        <template #prepend v-if="showBack">
            <v-btn 
            icon="mdi-chevron-left" variant="text" 
            @click="emit('back')" 
            density="compact">
            </v-btn>
        </template>
        <template #title>
            <span class="card-title-lg">{{ title }}</span>
        </template>
        <template #subtitle>
            <span style="font-size: 1rem; letter-spacing: 2px; text-transform: uppercase;">
                {{ subtitle }}
            </span>
            <span v-if="sublabel" style="font-size: 1rem; letter-spacing: 1.6px;" 
            :class="sublabelClass">
                &nbsp;{{sublabel}}
            </span>
        </template>
        <v-card-text class="pa-0 dashboard-body">
            <slot name="body"></slot>
        </v-card-text>
    </v-card>
</template>

<script setup>

const props = defineProps(['title', 'subtitle', 'sublabel', 'sublabelClass', 'showBack'])
const emit = defineEmits(['back'])

</script>

<style scoped>

</style>