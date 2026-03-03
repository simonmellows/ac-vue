<template>
    <v-container class="h-100 py-0 overflow-y-scroll no-scrollbar" ref="container" fluid>
        <v-row class="h-100 flex-nowrap scroll-snap-x no-scrollbar">
            <v-col class="h-100" cols="12" sm="5" md="4" xl="3">
                <v-card title="Scenes" rounded="xl" class="border-thin mh-100" :class="{'h-100': sceneCardFullHeight || (xs && rowHeightExceeded)}"
                ref="sceneCard">
                    <v-card-text class="scene-card-text overflow-y-scroll no-scrollbar" ref="sceneCardText">
                        <Scenes 
                        commandPrefix="currentRoom.shading.scenes"
                        :maxSceneCount="20"
                        />
                    </v-card-text>
                </v-card>
            </v-col>
            <v-col class="h-100 overflow-y-scroll no-scrollbar" cols="12" sm="7" md="8" xl="3">
                <Devices 
                :type="type"
                commandPrefix="currentRoom.shading.circuits"
                />
            </v-col>
        </v-row>   
    </v-container>
</template>

<script setup>
import vuetify from '../../plugins/vuetify'
import { onMounted, ref, watch } from 'vue'
import useElementDimensions from '../../composables/useElementDimensions'
import Scenes from '../widgets/Scenes.vue'
import Devices from '../widgets/Devices.vue'

const { xs } = vuetify.display

const props = defineProps(['type'])

const container = ref(null)
const sceneCard = ref(null)
const sceneCardText = ref(null)
const sceneCardFullHeight = ref(false)
const rowHeightExceeded = ref(false)

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

</script>

<style scoped>
.scene-card-text{
    height: calc(100% - 70px) !important;
}
</style>
