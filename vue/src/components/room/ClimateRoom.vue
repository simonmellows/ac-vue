<template>
    <v-container fluid class="py-0 h-100">
        <v-row class="flex-nowrap scroll-snap-x no-scrollbar h-100">
            <v-spacer></v-spacer>
            <v-col :cols="cols" class="h-100" v-for="(zone, index) in zoneCount" :key="zone">
                <ClimateZoneCard 
                :index="index"
                />
            </v-col>
            <v-spacer></v-spacer>
        </v-row>
    </v-container>
</template>

<script setup>
import ClimateZoneCard from '../zone/zone-cards/ClimateZoneCard.vue';
import vuetify from '../../plugins/vuetify'
import { computed } from 'vue';
import { useSystemFeedbackStore } from '@/store/useSystemFeedbackStore';

const { xs, sm } = vuetify.display
const systemFeedbackStore = useSystemFeedbackStore()

const cols = computed(() => xs.value ? 12 : sm.value ? 6 :  4)

const zoneCount = computed(() => systemFeedbackStore.getPropertyValue('climateRoom.zoneCount', 'analog') || 1)

</script>

<style scoped>

</style>