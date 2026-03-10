<template>
    <v-row :dense="dense" :no-gutters="noGutters">
        <v-col
            v-for="scene in scenes"
            :key="scene.label"
            v-bind="colBreakpoints"
        >
            <v-btn
                block
                :height="buttonHeight"
                :variant="scene.model ? activeVariant : inactiveVariant"
                :color="scene.model ? (activeColor || 'primary') : undefined"
                @click="scene.click"
            >
                {{ scene.label }}
            </v-btn>
        </v-col>
    </v-row>
</template>

<script setup>
import { pulse } from '@/composables/useInteractions'
import { useSystemFeedbackStore } from '@/store/useSystemFeedbackStore'
import { computed } from 'vue'

const props = defineProps({
    commandPrefix: {
        type: String,
        required: true,
    },
    maxSceneCount: {
        type: Number,
        default: 0,
    },
    activeColor: {
        type: String,
        default: 'primary',
    },
    /** Vuetify v-col breakpoint props — cols at each breakpoint (number of columns per row). */
    cols: { type: [Number, String], default: 12 },
    sm:   { type: [Number, String], default: undefined },
    md:   { type: [Number, String], default: undefined },
    lg:   { type: [Number, String], default: undefined },
    xl:   { type: [Number, String], default: undefined },
    /** Height of each scene button in pixels. */
    buttonHeight: {
        type: [Number, String],
        default: 60,
    },
    /** Vuetify button variant when the scene is active. */
    activeVariant: {
        type: String,
        default: 'flat',
    },
    /** Vuetify button variant when the scene is inactive. */
    inactiveVariant: {
        type: String,
        default: 'tonal',
    },
    /** Reduces spacing between buttons. */
    dense: {
        type: Boolean,
        default: true,
    },
    /** Removes gutters between buttons entirely. */
    noGutters: {
        type: Boolean,
        default: false,
    },
})

const systemFeedbackStore = useSystemFeedbackStore()

const colBreakpoints = computed(() => ({
    cols: props.cols,
    ...(props.sm !== undefined && { sm: props.sm }),
    ...(props.md !== undefined && { md: props.md }),
    ...(props.lg !== undefined && { lg: props.lg }),
    ...(props.xl !== undefined && { xl: props.xl }),
}))

const scenes = computed(() => {
    const arr = []
    for (let index = 0; index < props.maxSceneCount; index++) {
        arr.push({
            label:   systemFeedbackStore.getPropertyValue(props.commandPrefix + `[${index}].labelFb`,   'serial'),
            visible: systemFeedbackStore.getPropertyValue(props.commandPrefix + `[${index}].visibleFb`, 'digital'),
            click:   () => pulse(props.commandPrefix + `[${index}].press`),
            model:   systemFeedbackStore.getPropertyValue(props.commandPrefix + `[${index}].pressFb`,   'digital'),
        })
    }
    return arr.filter(scene => scene.visible)
})
</script>
