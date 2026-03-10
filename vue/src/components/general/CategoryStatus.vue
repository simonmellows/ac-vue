<template>
    <div :class="`text-${color}`" :style="size ? `font-size: ${size}px;` : undefined">
        {{ text }}
    </div>
</template>

<script setup>
const props = defineProps(['text', 'color', 'size'])
</script>

<style lang="scss" scoped>

</style>