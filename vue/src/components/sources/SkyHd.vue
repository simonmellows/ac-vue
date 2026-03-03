<template>
    <v-container class="w-100 h-100 d-flex flex-column ga-4" fluid>
        <v-card class="w-100 h-100 d-flex flex-column" variant="text">
            <v-card-text class="flex-grow-1">
                <v-window v-model="sourceWindowItem" class="h-100 w-100">
                    <v-window-item class="h-100">
                        <div class="h-100 d-flex flex-column justify-space-around">
                            <IconButtons />
                            <DPad />
                        </div>
                    </v-window-item>
                    <v-window-item class="h-100">
                        <div class="h-100 d-flex flex-column justify-space-around">
                            <ChannelButtons />
                            <CustomButtons :buttons="['Box Office', 'Interactive', 'Info', 'Guide']" />
                        </div>
                    </v-window-item>
                    <v-window-item class="h-100">
                        <div class="h-100 d-flex flex-column justify-space-around">
                            <ColorButtons />
                            <Keypad />
                        </div>
                    </v-window-item>
                </v-window>
            </v-card-text>
            <v-card-actions>
                <v-spacer></v-spacer>
                <v-btn :class="{'opacity-30' : sourceWindowItem != 0}" density="compact" variant="text" icon="mdi-circle-small" @click="sourceWindowItem = 0"></v-btn>
                <v-btn :class="{'opacity-30' : sourceWindowItem != 1}" density="compact" variant="text" icon="mdi-circle-small" @click="sourceWindowItem = 1"></v-btn>
                <v-btn :class="{'opacity-30' : sourceWindowItem != 2}" density="compact" variant="text" icon="mdi-circle-small" @click="sourceWindowItem = 2"></v-btn>
                <v-spacer></v-spacer>
            </v-card-actions>
        </v-card>
        <!--<v-row class="flex-grow-1">
            <v-col cols="12" md="4">
                <div class="h-100 d-flex flex-column flex-wrap ga-5 justify-space-between">
                    <IconButtons />
                    <ChannelButtons />
                    <ColorButtons />
                </div>
            </v-col>
            <v-col cols="12" md="4">
                <DPad />
            </v-col>
            <v-col cols="12" md="4">
                <div class="h-100 d-flex flex-column justify-space-between">
                    <Favourites />
                    <Keypad />
                    <CustomButtons :buttons="['Box Office', 'Interactive', 'Info', 'Guide']" />
                </div>
            </v-col>
        </v-row>
        <div class="w-100 d-flex justify-center">
            <v-card rounded="xl" variant="flat" class="border-thin mx-auto" max-width="500">
                <v-card-text class="pa-2">
                    <VolumeControl :format="0" />
                </v-card-text>
            </v-card>
        </div>-->
    </v-container>
</template>

<script setup>
import { ref } from 'vue';
import DPad from './widgets/DPad.vue';
import Keypad from './widgets/Keypad.vue';
import Favourites from './widgets/Favourites.vue';
import CustomButtons from './widgets/CustomButtons.vue';
import ColorButtons from './widgets/ColorButtons.vue';
import IconButtons from './widgets/IconButtons.vue';
import ChannelButtons from './widgets/ChannelButtons.vue';
import VolumeControl from '../widgets/VolumeControl.vue';

const sourceWindowItem = ref(0)

</script>

<style scoped>

</style>