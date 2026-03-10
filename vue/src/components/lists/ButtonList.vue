<template>
    <div class="h-100 d-flex">
        <v-row :dense="dense" :class="{'my-auto' : align === 'center', 'mt-auto' : align === 'end'}">
            <v-col
                v-for="button in buttons"
                :key="button.label"
                v-bind="colBreakpoints"
            >
                <v-btn
                    block
                    :height="buttonHeight"
                    :variant="button.model ? activeVariant : inactiveVariant"
                    :color="button.model ? (activeColor || 'primary') : undefined"
                    @click="button.click"
                >
                    {{ button.label }}
                </v-btn>
            </v-col>
        </v-row>
    </div>
</template>

<script setup>
import { computed } from 'vue'

const props = defineProps({
    /**
     * Array of button definitions.
     * Each button: { label: string, model: boolean, click: Function }
     */
    buttons: {
        type: Array,
        required: true,
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
    /** Height of each button in pixels. */
    buttonHeight: {
        type: [Number, String],
        default: 60,
    },
    /** Vuetify button variant when the button is active. */
    activeVariant: {
        type: String,
        default: 'flat',
    },
    /** Vuetify button variant when the button is inactive. */
    inactiveVariant: {
        type: String,
        default: 'tonal',
    },
    /** Reduces spacing between buttons. */
    dense: {
        type: Boolean,
        default: true,
    },
    /** Vertical alignment of buttons. Vuetify v-row align: 'start' | 'center' | 'end' | 'baseline' | 'stretch' */
    align: {
        type: String,
        default: 'start',
    },
})

const colBreakpoints = computed(() => ({
    cols: props.cols,
    ...(props.sm !== undefined && { sm: props.sm }),
    ...(props.md !== undefined && { md: props.md }),
    ...(props.lg !== undefined && { lg: props.lg }),
    ...(props.xl !== undefined && { xl: props.xl }),
}))
</script>
