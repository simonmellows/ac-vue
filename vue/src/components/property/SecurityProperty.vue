<template>
    <v-container fluid class="h-100 w-100">
        <v-row class="h-100 overflow-y-scroll my-auto">
            <v-col cols="12" md="8" :class="{'h-100' : !sm}">
                <DashboardCard label="Cameras" icon="mdi-cctv" class="camera-card">
                    <template #text>
                        <div class="d-flex flex-column ga-3 h-100 justify-space-between">
                            <div class="camera-image-container">
                                <img class="camera-image" src="https://dyvdnmp0itmzz.cloudfront.net/wp-content/uploads/2023/03/22155456/Well-Cottage-Jonathan-Buckley_20_0624_001.jpg"></img>
                            </div>
                            <div class="d-flex ga-2 overflow-x-auto no-scrollbar camera-buttons">
                                <v-btn v-for="camera in 20" class="flex-shrink-0 my-auto" rounded="xl" variant="tonal">
                                    Camera {{ camera }}
                                </v-btn>
                            </div>
                        </div>
                    </template>
                </DashboardCard>
            </v-col>
            <v-col cols="12" md="4" :class="{'h-100' : !sm}">
                <v-card class="h-100 border-thin" rounded="xl">
                    <v-card-text>

                    </v-card-text>
                </v-card>
            </v-col>
        </v-row>
    </v-container>
</template>

<script setup>
import DashboardCard from '../general/DashboardCard.vue';
import vuetify from '@/plugins/vuetify';

const { xs, sm, md } = vuetify.display

</script>

<style>
.camera-card .v-card-text{
    height: calc(100% - 83px) !important;
    padding-bottom: 0px !important;
}
.camera-buttons{
    height: 50px !important;
}
.camera-image-container{
    height: calc(100% - 50px) !important; 
    width: 100% !important; 
    display: flex;
    justify-content: center;
}
.camera-image-container .camera-image{
    max-width: 100% !important;
    max-height: 100% !important;
    object-fit: contain;
}
</style>