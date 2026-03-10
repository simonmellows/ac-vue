<template>
    <RoomCardList
        :rooms="rooms.filter(room => room.hasLighting)"
        :show-favourite="false"
        :card-body="ButtonList"
        :card-body-props="room => ({
            buttons: buildSceneButtons(room.commandPrefix + '.lighting.scenes', 5),
            activeColor: 'warning',
            cols: 12,
            align: 'center',
        })"
    />
</template>

<script setup>
import RoomCardList from '../lists/RoomCardList.vue';
import ButtonList from '../lists/ButtonList.vue';
import useRooms from '@/composables/useRooms';
import { pulse } from '@/composables/useInteractions';
import { useSystemFeedbackStore } from '@/store/useSystemFeedbackStore';

const { rooms } = useRooms()
const systemFeedbackStore = useSystemFeedbackStore()

function buildSceneButtons(commandPrefix, count) {
    const arr = [
        {
            label: "On",
            model: systemFeedbackStore.getPropertyValue(commandPrefix + `.onFb`, 'digital'),
            click: () => pulse(commandPrefix + `.on`),
        },
        {
            label: "Off",
            model: systemFeedbackStore.getPropertyValue(commandPrefix + `.offFb`, 'digital'),
            click: () => pulse(commandPrefix + `.off`),
        }
    ]
    /*for (let index = 0; index < count; index++) {
        const visible = systemFeedbackStore.getPropertyValue(commandPrefix + `[${index}].visibleFb`, 'digital')
        if (!visible) continue
        arr.push({
            label: systemFeedbackStore.getPropertyValue(commandPrefix + `[${index}].labelFb`, 'serial'),
            model: systemFeedbackStore.getPropertyValue(commandPrefix + `[${index}].pressFb`, 'digital'),
            click: () => pulse(commandPrefix + `[${index}].press`),
        })
    }*/
    return arr
}
</script>

<style scoped>

</style>