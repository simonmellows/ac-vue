<template>
    <v-container fluid class="h-100 pb-0">
        <v-row>
            <v-container fluid class="d-flex ga-2 flex-nowrap overflow-x-scroll no-scrollbar">
                <v-btn v-for="roomGroup in allRoomGroupsMapped" :key="roomGroup.id" 
                class="text-overline flex-shrink-0" 
                :variant="currentRoomGroup === roomGroup.id ? undefined : 'outlined'"
                :color="currentRoomGroup === roomGroup.id ? 'primary' : undefined"
                rounded=xl
                :size="xs ? 'small' : lg ? 'large' : undefined"
                @click="roomGroup.click">
                    <v-icon :icon="roomGroup.icon" v-if="roomGroup.icon"></v-icon>
                    <div v-else>{{ roomGroup.label }}</div>
                </v-btn>
            </v-container>
        </v-row>
        <v-row v-if="emptyMessage" class="room-select-message-container">
            <v-col >
                <div class="h-100 w-100 d-flex flex-column">
                    <v-card size="x-large" class="mx-auto my-auto" rounded="xl">
                        <v-card-text class="text-overline text-center">
                            {{ emptyMessage }}
                        </v-card-text>
                    </v-card>
                </div>
            </v-col>
        </v-row>
        <v-row v-else-if="!view" class="h-100 room-list-row overflow-y-scroll">
            <v-col v-if="filteredRooms.length" v-for="room in filteredRooms" cols="12" sm="6" md="4" xl="3" class="h-50">
                <DashboardCard :label="room.label" @click="room.click">
                    <template #append>
                        <v-icon color="error" @click.stop @mousedown.stop v-on:touchstart.stop v-on:touchmove.stop v-on:touchend.stop
                        :icon="room.favourite ? 'mdi-heart' : 'mdi-heart-outline'"
                        @click="room['click:favourite']"
                        ></v-icon>
                    </template>
                </DashboardCard>
            </v-col>
        </v-row>
        <v-row v-else-if="view === 1" class="floorplan-container">
            <Floorplan :floorplan="floorplanData" @roomSelected="roomSelected"/>
        </v-row>
    </v-container>
</template>

<script setup>
import { computed, ref, watchEffect } from 'vue';
import { useInterfaceDataStore } from '@/store/useInterfaceDataStore';
import { useSystemFeedbackStore } from '@/store/useSystemFeedbackStore';
import { pulse } from '@/composables/useInteractions';
import Floorplan from './Floorplan.vue';
import floorplans from '../../data/floorplans'
import vuetify from '@/plugins/vuetify';
import DashboardCard from '../general/DashboardCard.vue';

const props = defineProps(['view'])

const { xs, lg } = vuetify.display

const roomGroupCommandPrefix = 'roomGroups'
const roomCommandPrefix = 'rooms'

const emit = defineEmits('roomSelected')
const interfaceDataStore = useInterfaceDataStore()
const systemFeedbackStore = useSystemFeedbackStore()
const currentRoomGroup = computed(() => interfaceDataStore.data.roomGroup)
const emptyMessage = computed(() => {
    if(props.view === 0 && !filteredRooms.value?.length) return "No rooms available in this room group"
    if(props.view === 1 && !floorplanData.value) return "No floorplan for this room group"
    else return null
})
const defaultRoomGroups = [
    {
        label: "All",
        id: "all",
        visible: true,
        click: () => interfaceDataStore.data.roomGroup = "all"
    },
    {
        label: "Favourites",
        icon: "mdi-heart",
        id: "favourites",
        visible: true,
        click: () => interfaceDataStore.data.roomGroup = "favourites"
    }
]
const allRoomGroups = computed(() => systemFeedbackStore.getSerialArray(roomGroupCommandPrefix, 20, 'labelFb'))
const allRooms = computed(() => systemFeedbackStore.getSerialArray(roomCommandPrefix, 150, 'labelFb'))

// Room Groups
const allRoomGroupsMapped = computed(() => {
    return [...defaultRoomGroups, ...allRoomGroups.value?.map((roomGroupName, roomGroupIndex) => {
        let commandPrefix = `${roomGroupCommandPrefix}[${roomGroupIndex}]`
        return {
            label: roomGroupName,
            id: roomGroupIndex,
            visible: systemFeedbackStore.getPropertyValue(`${commandPrefix}.visibleFb`, 'digital'),
            floorplan: systemFeedbackStore.getPropertyValue(`${commandPrefix}.floorplanFileNameFb`, 'serial'),
            click: () => interfaceDataStore.data.roomGroup = roomGroupIndex
        }
    })].filter(roomGroup => roomGroup.visible)
})

// Rooms
const filteredRooms = computed(() => {
    return allRooms.value?.map((roomName, roomIndex) => {
        let commandPrefix = `${roomCommandPrefix}[${roomIndex}]`
        return {
            label: roomName,
            index: roomIndex,
            favourite: systemFeedbackStore.getPropertyValue(`${commandPrefix}.favouriteFb`, 'digital'),
            roomGroup: systemFeedbackStore.getPropertyValue(`${commandPrefix}.roomGroupFb`, 'analog'),
            visible: systemFeedbackStore.getPropertyValue(`${commandPrefix}.visibleFb`, 'digital'),
            click: () => roomSelected(roomIndex),
            'click:favourite': () => pulse(`${commandPrefix}.favourite`),
        }
    }).filter(room => {
        if(room.visible){
            if(currentRoomGroup.value === 'all') return room
            else if(currentRoomGroup.value === 'favourites' && room.favourite) return room
            else if(Array.isArray(room.roomGroup) && room.roomGroup.includes(currentRoomGroup.value)) return room
            else if(room.roomGroup === currentRoomGroup.value) return room
        }
    })
})

function roomSelected(roomIndex){
    let commandPrefix = `${roomCommandPrefix}[${roomIndex}]`
    pulse(`${commandPrefix}.press`)
    emit('roomSelected')
}

// Floorplans
const floorplanData = ref(null)

watchEffect(() => {
    let roomGroup = allRoomGroupsMapped.value?.find(rg => rg.id === currentRoomGroup.value)
    if(roomGroup?.floorplan){
        floorplanData.value = floorplans[roomGroup?.floorplan] || null
    }
    else floorplanData.value = null
})

</script>

<style scoped>
.room-list-row{
    max-height: calc(100% - 68px) !important;
}
.room-select-message-container{
    height: calc(100% - 68px) !important;
}
.floorplan-container{
    height: calc(100% - 68px) !important;
}

</style>