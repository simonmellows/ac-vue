<template>
    <v-card ref="cardRef" class="h-100 border-thin" rounded="xl" >
        <template #prepend v-if="icon">
            <v-icon :icon="icon" :color="iconColor" :class="{'dashboard-card-icon' : !xs}"></v-icon>
        </template>
        <template #title>
            <div class="text-button text-truncate" :class="{'dashboard-card-title' : !xs}">{{ label }}</div>
        </template>
        <template #subtitle v-if="$slots.subtitle">
            <slot name="subtitle"></slot>
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
import { ref, onMounted, onBeforeUnmount, computed } from 'vue';

const props = defineProps(['label', 'icon', 'iconColor'])

const { xs } = vuetify.display

const cardRef = ref(null)
const headerHeight = ref(0)
const cardTextPaddingBottom = ref(0)

const bodyHeight = computed(() => `calc(100% - ${headerHeight.value + cardTextPaddingBottom.value}px)`)

let observer = null

onMounted(() => {
    const el = cardRef.value?.$el
    if (!el) return
    observer = new ResizeObserver(() => {
        const header = el.querySelector('.v-card-item')
        headerHeight.value = header?.offsetHeight ?? 0
        const cardText = el.querySelector('.v-card-text')
        if (cardText) {
            const style = getComputedStyle(cardText)
            cardTextPaddingBottom.value = parseFloat(style.paddingBottom) || 0
        }
    })
    observer.observe(el)
})

onBeforeUnmount(() => {
    observer?.disconnect()
})
</script>

<style scoped>
:deep(.v-card-text) {
    height: v-bind(bodyHeight);
    overflow-y: auto;
    box-sizing: border-box;
}
</style>
