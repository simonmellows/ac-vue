<template>
    <RoomCardList 
    :rooms="rooms.filter(room => room.hasShading)"
    :show-favourite="true"
    :card-body="Scenes"
    :card-body-props="room => ({ 
        commandPrefix: room.commandPrefix + '.shading.scenes', 
        maxSceneCount: 5, 
        activeColor: 'primary' 
    })"
    />
</template>

<script setup>
import RoomCardList from '../lists/RoomCardList.vue';
import useRooms from '@/composables/useRooms';
import Scenes from '../widgets/Scenes.vue';

const { rooms } = useRooms()

</script>

<style scoped>

</style>