<template>
    <div class="d-flex flex-column ga-2 justify-center">
        <v-btn class="w-100" height="60" v-for="scene in scenes"
        @click="scene.click"
        :variant="scene.model ? 'flat' : 'tonal'"
        :color="scene.model ? (activeColor || 'primary') : undefined"
        >
            {{ scene.label }}
        </v-btn>
    </div>
</template>

<script setup>
import { pulse } from '@/composables/useInteractions'
import { useSystemFeedbackStore } from '@/store/useSystemFeedbackStore'
import { computed } from 'vue'

const props = defineProps(['commandPrefix', 'maxSceneCount', 'activeColor'])

const systemFeedbackStore = useSystemFeedbackStore()

const scenes = computed(() => {
    let arr = []
    for (let index = 0; index < (props.maxSceneCount || 0); index++) {
        arr.push({
            label: systemFeedbackStore.getPropertyValue(props.commandPrefix + `[${index}].labelFb`, 'serial'),
            visible: systemFeedbackStore.getPropertyValue(props.commandPrefix + `[${index}].visibleFb`, 'digital'),
            click: () => pulse(props.commandPrefix + `[${index}].press`),
            model: systemFeedbackStore.getPropertyValue(props.commandPrefix + `[${index}].pressFb`, 'digital'),
        })
    }
    return arr.filter(scene => scene.visible)
})

</script>

<style scoped>
</style>