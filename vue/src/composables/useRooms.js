import { useSystemFeedbackStore } from "@/store/useSystemFeedbackStore";
import { computed } from "vue";
import { pulse } from "./useInteractions";
import { Ripple } from "vuetify/lib/directives";

export default function useRooms(){

    const roomCommandPrefix = 'rooms'
    const systemFeedbackStore = useSystemFeedbackStore()

    const allRoomLabels = computed(() => systemFeedbackStore.getSerialArray(roomCommandPrefix, 150, 'labelFb'))
    const rooms = computed(() => allRoomLabels.value?.map((roomName, roomIndex) => {
        let commandPrefix = `${roomCommandPrefix}[${roomIndex}]`
        return {
            label: roomName,
            commandPrefix: commandPrefix,
            index: roomIndex,
            id: roomIndex + 1,
            favourite: systemFeedbackStore.getPropertyValue(`${commandPrefix}.favouriteFb`, 'digital'),
            roomGroup: systemFeedbackStore.getPropertyValue(`${commandPrefix}.roomGroupFb`, 'analog'),
            visible: systemFeedbackStore.getPropertyValue(`${commandPrefix}.visibleFb`, 'digital'),
            sourceWatch: systemFeedbackStore.getPropertyValue(`${commandPrefix}.sourceWatchFb`, 'analog'),
            sourceListen: systemFeedbackStore.getPropertyValue(`${commandPrefix}.sourceListenFb`, 'analog'),

            // Methods
            'click:favourite': () => pulse(`${commandPrefix}.favourite`),
            'click:power': () => pulse(`${commandPrefix}.roomOff`),
            'click:toggleWatch': () => pulse(`${commandPrefix}.watchOff`),
            'click:toggleListen': () => pulse(`${commandPrefix}.listenOff`),
        }
    }).filter(room => room.visible))

    const favourites = computed(() => rooms?.value?.filter(room => room.favourite))

    function getRoomByIndex(index){
        return rooms?.value?.find(room => room.index === index)
    }
    function getRoomById(id){
        return rooms?.value?.find(room => room.id === id)
    }
    function getRoomsByRoomGroup(roomGroup){
        return rooms?.value?.find(room => room.roomGroup === roomGroup)
    }

    return {
        rooms,
        favourites,
        getRoomByIndex,
        getRoomById,
        getRoomsByRoomGroup,
    }
}