<template>
    <v-container fluid class="h-100 pb-0">
        <v-row class="h-100 room-list-row overflow-y-scroll">
            <v-col v-for="room in rooms" :key="room.index" cols="12" sm="6" md="3" xl="3" class="h-50">
                <DashboardCard :label="room.label" @click="emit('roomSelected', room)">
                    <template #text v-if="cardBody">
                        <component :is="cardBody" v-bind="resolveCardBodyProps(room)" />
                    </template>
                    <template #append v-if="showFavourite">
                        <v-icon
                            color="error"
                            @click.stop @mousedown.stop
                            v-on:touchstart.stop v-on:touchmove.stop v-on:touchend.stop
                            :icon="room.favourite ? 'mdi-heart' : 'mdi-heart-outline'"
                            @click="room['click:favourite']"
                        ></v-icon>
                    </template>
                </DashboardCard>
            </v-col>
        </v-row>
    </v-container>
</template>

<script setup>
import DashboardCard from '../general/DashboardCard.vue';

const props = defineProps({
    rooms: { type: Array, default: () => [] },
    showFavourite: { type: Boolean, default: false },
    cardBody: { type: Object, default: null },
    cardBodyProps: { type: [Object, Function], default: () => ({}) }
})

const emit = defineEmits(['roomSelected'])

function resolveCardBodyProps(room) {
    return typeof props.cardBodyProps === 'function' ? props.cardBodyProps(room) : props.cardBodyProps
}
</script>

<style scoped>
.room-list-row {
    max-height: 100%;
}
</style>
