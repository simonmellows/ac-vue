<template>
    <v-container class="h-100 py-0 overflow-y-scroll no-scrollbar" ref="container" fluid>
        <v-row class="h-100 flex-nowrap scroll-snap-x no-scrollbar">
            <v-col class="h-100" cols="12" sm="5" md="4" xl="3">
                <v-card title="Scenes" rounded="xl" class="border-thin mh-100" :class="{'h-100': sceneCardFullHeight || (xs && rowHeightExceeded)}"
                ref="sceneCard">
                    <v-card-text class="scene-card-text overflow-y-scroll no-scrollbar" ref="sceneCardText">
                        <Scenes 
                        :commandPrefix="`currentRoom.lighting.scenes`"
                        :maxSceneCount="maxSceneCount"
                        activeColor="amber"
                        />
                    </v-card-text>
                </v-card>
            </v-col>
            <v-col class="h-100 overflow-y-scroll no-scrollbar" cols="12" sm="7" md="8" xl="3">
                <Devices 
                :type="type"
                :commandPrefix="`currentRoom.lighting.circuits`"
                />
            </v-col>
        </v-row>   
    </v-container>
</template>

<script setup>
import vuetify from '../../plugins/vuetify'
import { computed, onMounted, ref, watch } from 'vue'
import useElementDimensions from '../../composables/useElementDimensions'
import Scenes from '../widgets/Scenes.vue'
//import LightingCircuit from '../device/device-cards/LightingCircuit.vue'
//import { useSystemFeedbackStore } from '@/store/useSystemFeedbackStore'
import { useInterfaceDataStore } from '@/store/useInterfaceDataStore'
import Devices from '../widgets/Devices.vue'

const { xs } = vuetify.display

const props = defineProps(['type'])
//const systemFeedbackStore = useSystemFeedbackStore()

const container = ref(null)
const sceneCard = ref(null)
const sceneCardText = ref(null)
const sceneCardFullHeight = ref(false)
const rowHeightExceeded = ref(false)
const interfaceDataStore = useInterfaceDataStore()

const room = computed(() => interfaceDataStore.data.room)

const maxSceneCount = 30
//const maxDeviceCount = 30

function useStabilizedWatcher(source, callback, delay = 10) {
    let timeout
    watch(source, () => {
        clearTimeout(timeout)
        timeout = setTimeout(() => {
            callback()
        }, delay)
    })
    callback()
}

// Container height computations
onMounted(() => {
    const { width: containerWidth  } = useElementDimensions(container)
    const { height: sceneCardHeight  } = useElementDimensions(sceneCard)
    const { height: sceneCardTextHeight } = useElementDimensions(sceneCardText)

    // Watch for container size change and change cols for card view
    useStabilizedWatcher(containerWidth, () => {
        if((sceneCardHeight.value) < sceneCardTextHeight.value) sceneCardFullHeight.value = true
        else sceneCardFullHeight.value = false
    })
})


/*
const devices = computed(() => {
    let arr = []
    for (let index = 0; index < (maxDeviceCount || 0); index++) {
        arr.push({
            label: systemFeedbackStore.getPropertyValue(`lightingCircuit[${props.index}].labelFb`, 'serial'),
        })
    }
    return arr.filter(device => device.label)
})
*/
</script>

<style scoped>
.scene-card-text{
    height: calc(100% - 70px) !important;
}
</style>
