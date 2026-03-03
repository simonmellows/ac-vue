<template>
    <v-container fluid class="h-100 py-0 overflow-y-scroll no-scrollbar">
        <v-row>
            <v-col v-for="source in sources" :key="source.label" :cols="12" sm="6" md="4" class="h-100">
                <!--<v-card  class="h-100 border-thin" rounded="xl"
                :variant="source.model ? 'tonal' : undefined"
                :color="source.model ? 'primary' : undefined"
                @click="[source.click(), emitter.emit('sourceSelected')]">
                    <template #prepend>
                        <v-icon :icon="source.icon" style="font-size: 30px;"></v-icon>
                    </template>
                    <template #title>
                        <div class="dashboard-card-title text-button">{{ source.label }}</div>
                    </template>
                    <template #append v-if="source.inUse">
                        <div @click.stop @mousedown.stop>
                            <v-menu :close-on-content-click="false">
                                <template v-slot:activator="{ props }">
                                    <v-chip v-bind="props" color="primary" variant="flat" class="text-overline" size="small">
                                        In Use
                                    </v-chip>
                                </template>
                                <v-card variant="flat" class="border-thin" rounded="xl">
                                    <v-card-text class="d-flex flex-column ga-3 overflow-scroll-y">
                                        <v-btn 
                                        v-for="(room, index) in rooms.filter(room => {
                                            if(type === 'watch') return room.sourceWatch === source.id
                                            else if(type === 'listen') return room.sourceListen === source.id
                                        })"
                                        @click="room['click:power']"
                                        :key="index"
                                        :value="index" 
                                        prepend-icon="mdi-power" rounded="xl" color="error" variant="tonal" class="w-100" >
                                            {{ room.label }}
                                        </v-btn>
                                    </v-card-text>
                                </v-card>
                            </v-menu>
                        </div>
                    </template>
                </v-card>-->
                <DashboardCard :icon="source.icon" :label="source.label"
                :variant="source.model ? 'tonal' : undefined"
                :color="source.model ? 'primary' : undefined"
                @click="[source.click(), emitter.emit('sourceSelected')]"
                >
                    <template #append v-if="source.inUse">
                        <div @click.stop @mousedown.stop>
                            <v-menu :close-on-content-click="false">
                                <template v-slot:activator="{ props }">
                                    <v-chip v-bind="props" color="primary" variant="flat" class="text-overline" size="small">
                                        In Use
                                    </v-chip>
                                </template>
                                <v-card variant="flat" class="border-thin" rounded="xl">
                                    <v-card-text class="d-flex flex-column ga-3 overflow-scroll-y">
                                        <v-btn 
                                        v-for="(room, index) in rooms.filter(room => {
                                            if(type === 'watch') return room.sourceWatch === source.id
                                            else if(type === 'listen') return room.sourceListen === source.id
                                        })"
                                        @click="room['click:power']"
                                        :key="index"
                                        :value="index" 
                                        prepend-icon="mdi-power" rounded="xl" color="error" variant="tonal" class="w-100" >
                                            {{ room.label }}
                                        </v-btn>
                                    </v-card-text>
                                </v-card>
                            </v-menu>
                        </div>
                    </template>
                </DashboardCard>
            </v-col>
        </v-row>
    </v-container>
</template>

<script setup>
import { pulse } from '@/composables/useInteractions'
import { useSystemFeedbackStore } from '@/store/useSystemFeedbackStore'
import { computed } from 'vue'
import { emitter } from '../../plugins/eventBus'
import useRooms from '@/composables/useRooms'
import DashboardCard from '../general/DashboardCard.vue'

const props = defineProps(['commandPrefix', 'maxSourceCount', 'type'])

const systemFeedbackStore = useSystemFeedbackStore()

const { rooms } = useRooms()

const sources = computed(() => {
    let arr = []
    for (let index = 0; index < (props.maxSourceCount || 50); index++) {
        arr.push({
            label: systemFeedbackStore.getPropertyValue(`${props.commandPrefix}[${index}].labelFb`, 'serial'),
            index: index,
            id: index + 1,
            icon: systemFeedbackStore.getPropertyValue(`${props.commandPrefix}[${index}].iconFb`, 'serial'),
            inUse: systemFeedbackStore.getPropertyValue(`${props.commandPrefix}[${index}].inUseFb`, 'digital'),
            visible: systemFeedbackStore.getPropertyValue(`${props.commandPrefix}[${index}].visibleFb`, 'digital'),
            click: () => pulse(`${props.commandPrefix}[${index}].press`),
            model: systemFeedbackStore.getPropertyValue(`${props.commandPrefix}[${index}].pressFb`, 'digital'),
        })
    }
    return arr.filter(source => source.visible)
})

</script>

<style scoped>

</style>