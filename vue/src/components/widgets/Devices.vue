<template>
    <v-row class="">
        <v-col md="6" cols="12" v-for="(device, index) in devices" :key="device.label">
            <component
            :is="deviceCards[type]" 
            :commandPrefix="`${commandPrefix}[${index}]`"
            />
        </v-col>
    </v-row>
</template>

<script setup>
import { useSystemFeedbackStore } from '@/store/useSystemFeedbackStore'
import { computed } from 'vue'
import deviceCards from '../device/device-cards'

const props = defineProps(['type', 'commandPrefix', 'maxDeviceCount'])

const systemFeedbackStore = useSystemFeedbackStore()

const devices = computed(() => {
    let arr = []
    for (let index = 0; index < (props.maxDeviceCount || 30); index++) {
        arr.push({
            label: systemFeedbackStore.getPropertyValue(`${props.commandPrefix}[${index}].labelFb`, 'serial') || "unnamed",
            visible: systemFeedbackStore.getPropertyValue(`${props.commandPrefix}[${index}].visibleFb`, 'digital')
        })
    }
    return arr.filter(device => device.visible)
})

</script>

<style scoped>

</style>