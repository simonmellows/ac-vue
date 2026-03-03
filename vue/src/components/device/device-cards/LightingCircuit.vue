<template>
    <v-card class="border-thin" rounded="xl" variant="flat">
        <template #title>
            <div class="text-overline text-truncate" style="font-size: 1rem !important; letter-spacing: 2px !important;">
                <span class="text-amber">{{levelFb}}%&nbsp;</span>
                <span>{{labelFb}}</span>
            </div>
        </template>
        <template #append>
            <div class="my-auto">{{onFb ? 'ON' : 'OFF'}} &nbsp;&nbsp;</div>
            <v-switch
            :hide-details="true"
            color="amber"
            :inset="false"
            density="comfortable"
            class="my-auto"
            :model-value="onFb"
            @update:model-value="toggle"
            ></v-switch>
        </template>
        <v-card-text class="d-flex ga-3" v-if="typeFb === 1">
            <v-btn @click="turnOff" density="comfortable" variant="text" icon="mdi-weather-sunny-off opacity-20 my-auto"></v-btn>
            <CSlider 
            :joinPress="commandPrefix + '.press'"
            :join-level="commandPrefix + '.level'"
            :join-level-fb="commandPrefix + '.levelFb'"
            @sliderValue="(v) => levelFb = parseInt(v)"
            color="amber"
            elevation="4"
            trackSize="10"
            thumbSize="28"
            density="compact"
            :hide-details="true"
            />
            <v-btn @click="turnOn" density="comfortable" variant="text" icon="mdi-weather-sunny my-auto"></v-btn>
        </v-card-text>
        <v-card-text v-else-if="typeFb === 2">
            <v-color-picker mode="rgb" class="w-100" :hide-inputs="true" elevation="0" :hide-sliders="true" :canvas-height="100"></v-color-picker>
        </v-card-text>
    </v-card>
</template>

<script setup> 
import { computed, ref } from 'vue';
import CSlider from '../../interactive-elements/CSlider.vue';
import CSwitch from '../../interactive-elements/CSwitch.vue';
import { useSystemFeedbackStore } from '@/store/useSystemFeedbackStore';
import { pulse } from '@/composables/useInteractions';

const props = defineProps(['commandPrefix'])

const systemFeedbackStore = useSystemFeedbackStore()

const levelFb = computed(() => systemFeedbackStore.getPropertyValue(props.commandPrefix + '.levelFb', 'analog'))
const onFb = computed(() => systemFeedbackStore.getPropertyValue(props.commandPrefix + '.onFb', 'digital'))
const offFb = computed(() => systemFeedbackStore.getPropertyValue(props.commandPrefix + '.offFb', 'digital'))
const labelFb = computed(() => systemFeedbackStore.getPropertyValue(props.commandPrefix + '.labelFb', 'serial'))
const typeFb = computed(() => systemFeedbackStore.getPropertyValue(props.commandPrefix + '.typeFb', 'analog') || 1)

// RGB Feedback
const redFb = computed(() => systemFeedbackStore.getPropertyValue(props.commandPrefix + '.redFb', 'analog'))
const greenFb = computed(() => systemFeedbackStore.getPropertyValue(props.commandPrefix + '.greenFb', 'analog'))
const blueFb = computed(() => systemFeedbackStore.getPropertyValue(props.commandPrefix + '.blueFb', 'analog'))
const whiteFb = computed(() => systemFeedbackStore.getPropertyValue(props.commandPrefix + '.whiteFb', 'analog'))
const warmWhiteFb = computed(() => systemFeedbackStore.getPropertyValue(props.commandPrefix + '.warmWhiteFb', 'analog'))

function toggle(){
    pulse(props.commandPrefix + '.toggle')
}
function turnOn(){
    pulse(props.commandPrefix + '.turnOn')
}
function turnOff(){
    pulse(props.commandPrefix + '.turnOff')
}

</script>

<style scoped>

</style>