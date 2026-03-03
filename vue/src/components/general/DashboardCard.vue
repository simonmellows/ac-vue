<template>
    <v-card class="h-100 border-thin" rounded="xl" >
        <template #prepend v-if="icon">
            <v-icon :icon="icon" :color="iconColor" :class="{'dashboard-card-icon' : !xs}"></v-icon>
        </template>
        <template #title>
            <div class="text-button text-truncate" :class="{'dashboard-card-title' : !xs}">{{ label }}</div>
        </template>
        <template #append v-if="$slots.append">
            <slot name="append"></slot>
        </template>
        <template #text v-if="$slots.text">
            <slot name="text"></slot>
        </template>
    </v-card>
</template>

<script setup>
import vuetify from '@/plugins/vuetify';

const props = defineProps(['label', 'icon', 'iconColor'])

const { xs } = vuetify.display

</script>

<style scoped>

</style>