<template>
    <div v-show="!joinShow || show" :style="{ '--icon-color': buttonProps?.iconColor }">
        <v-badge 
        v-bind="badgeProps" 
        v-model="badge"
        >
            <v-btn
            @touchstart="btnPress()"
            @touchend="btnRelease()"
            v-bind="active ? {...buttonProps, ...activeProps} : {...buttonProps}"
            :active="!disableFeedback ? active : false"
            :disabled="(joinEnable && !enabled) || false"
            >
            </v-btn>
        </v-badge>
    </div>
</template>

<script setup>
import { computed, ref } from 'vue'
import { press, release, pulse, useButton } from '../../composables/useInteractions'
import { useSystemFeedbackStore } from '../../store/useSystemFeedbackStore'

// Props
const props = defineProps({
    // Joins
    joinPress: { type: [Number, String] },
    joinEnable: { type: [Number, String] },
    joinShow: { type: [Number, String] },
    joinHold: { type: [Number, String]},
    joinBadge: { type: [Number, String]},
    joinBadgeContent: { type: [Number, String]},
    disableFeedback: { type: Boolean },

    // Props
    activeProps: { type: Object },
    buttonProps: { type: Object },
    badgeProps: { type: Object },

    // Other
    holdTime: { type: Number, default: 1000 },
})

// Emits
const emit = defineEmits([
    'press',
    'release',
    'hold',
    'tap'
])

// Store
const systemFeedbackStore = useSystemFeedbackStore()

// Computed properties
const active = computed(() => systemFeedbackStore.getPropertyValue(props.joinPress, 'digital'))
const enabled = computed(() => systemFeedbackStore.getPropertyValue(props.joinEnable, 'digital'))
const show = computed(() => systemFeedbackStore.getPropertyValue(props.joinShow, 'digital'))
const badge = computed(() => systemFeedbackStore.getPropertyValue(props.joinBadge, 'digital') || false)

// Composables
const { btnPress, btnRelease } = useButton(
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

</script>

<style scoped>

::v-deep(.v-btn .v-icon) {
  color: var(--icon-color) !important;
}

</style>