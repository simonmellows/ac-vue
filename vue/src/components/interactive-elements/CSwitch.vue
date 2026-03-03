<template>
    <v-switch 
    v-show="!joinShow || show"
    @touchstart="switchPress()"
    @touchend="switchRelease()"
    v-model="active"
    :disabled="(joinEnable && !enabled) || false"
    ></v-switch>
</template>

<script setup>
import { computed, ref, watch } from 'vue'
import { press, release, pulse, useButton } from '../../composables/useInteractions'
import { useSystemFeedbackStore } from '../../store/useSystemFeedbackStore'

// Props
const props = defineProps({
    // Joins
    joinPress: { type: [Number, String] },
    joinEnable: { type: [Number, String] },
    joinShow: { type: [Number, String] },
    joinHold: { type: [Number, String]},

    // Other
    holdTime: { type: Number, default: 1000 },
})

// Emits
const emit = defineEmits([
    'press',
    'release',
    'hold',
    'tap',
    'switchValue'
])

// Store
const systemFeedbackStore = useSystemFeedbackStore()

// Computed properties
const active = computed(() => systemFeedbackStore.getPropertyValue(props.joinPress, 'digital'))
const enabled = computed(() => systemFeedbackStore.getPropertyValue(props.joinEnable, 'digital'))
const show = computed(() => systemFeedbackStore.getPropertyValue(props.joinShow, 'digital'))

// Composables
const { btnPress: switchPress, btnRelease: switchRelease } = useButton(
    () => {
        emit('press')
        press(props.joinPress)
    },
    () => {
        emit('release')
        release(props.joinPress)
    },
    () => {
        emit('tap')
        pulse(props.joinPress)
    },
    () => {
        emit('hold')
        pulse(props.joinHold)
    },
    !!(props.joinHold)
)

watch(active, (newValue) => {
    emit('switchValue', newValue)
})

</script>

<style scoped>
::v-deep(.opacity-100 .v-switch__track){
    opacity: 1 !important;
}
</style>