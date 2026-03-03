<template>
    <v-card class="border-thin" rounded="xl" variant="flat">
        <template #title>
            <div class="text-overline text-truncate" style="font-size: 1rem !important; letter-spacing: 2px !important;">
                <span class="text-primary">{{levelFb}}%&nbsp;</span>
                <span>{{labelFb}}</span>
            </div>
        </template>
        <template #append>
            <v-switch
            :hide-details="true"
            :model-value="openFb"
            @update:model-value="toggle"
            color="primary"
            :inset="false"
            density="comfortable"
            class="my-auto"
            ></v-switch>
        </template>
        <v-card-text class="d-flex ga-2">
            <v-btn variant="text" density="comfortable" icon="mdi-roller-shade-closed" class="my-auto" @click="close"></v-btn>
            <CSlider 
            :hide-details="true"
            :join-level="commandPrefix + '.level'"
            :join-level-fb="commandPrefix + '.levelFb'"
            />
            <v-btn variant="text" density="comfortable" icon="mdi-window-closed-variant" class="my-auto" @click="open"></v-btn>
        </v-card-text>
    </v-card>
</template>

<script setup>
import CSlider from '@/components/interactive-elements/CSlider.vue';
import { useSystemFeedbackStore } from '@/store/useSystemFeedbackStore';
import { computed } from 'vue';
import { pulse } from '@/composables/useInteractions';

const props = defineProps(['commandPrefix'])

const systemFeedbackStore = useSystemFeedbackStore()

const labelFb = computed(() => systemFeedbackStore.getPropertyValue(props.commandPrefix + '.labelFb', 'serial'))
const levelFb = computed(() => systemFeedbackStore.getPropertyValue(props.commandPrefix + '.levelFb', 'analog'))
const openFb = computed(() => systemFeedbackStore.getPropertyValue(props.commandPrefix + '.openFb', 'digital'))
const stopFb = computed(() => systemFeedbackStore.getPropertyValue(props.commandPrefix + '.stopFb', 'digital'))
const closeFb = computed(() => systemFeedbackStore.getPropertyValue(props.commandPrefix + '.closeFb', 'digital'))

function toggle(){
    pulse(props.commandPrefix + '.toggle')
}
function open(){
    pulse(props.commandPrefix + '.open')
}
function stop(){
    pulse(props.commandPrefix + '.stop')
}
function close(){
    pulse(props.commandPrefix + '.close')
}

</script>

<style scoped>

</style>