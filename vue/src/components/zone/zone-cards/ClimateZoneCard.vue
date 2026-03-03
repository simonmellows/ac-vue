<template>
    <v-card class="border-thin h-100" :class="[cardPaddingClass, {'mx-auto' : !xs && !sm}]" rounded="xl" variant="flat">
        <v-card-item v-if="label || hasEnable" :title="label" style="height: 60px;">
            <template #append v-if="hasEnable || hasScheduler" >
                <div class="d-flex ga-2">
                    <v-btn icon="mdi-calendar" variant="text" class="my-auto" v-if="hasScheduler"></v-btn>
                    <v-switch 
                    v-if="hasEnable"
                    @update:model-value="enable"
                    :model-value="enableFb"
                    :inset="true"
                    density="compact"
                    color="success"
                    class="my-auto"
                    :hide-details="true"
                    >
                    </v-switch>
                </div>
            </template>
        </v-card-item>
        <v-card-text class="overflow-y-scroll no-scrollbar pb-0"
        :class="[cardTextHeightClass, {'disabled' : (!enableFb && hasEnable)}]"
        > 
            <div class="d-flex flex-column h-100">
                <v-spacer></v-spacer>

                <div class="d-flex justify-center">
                    <v-btn variant="text" icon="mdi-minus" size="x-large" @click="setpointLower"></v-btn>
                    <span class="my-auto text-center" style="font-size: 3rem; width: 120px;">{{setpoint}}°</span>
                    <v-btn variant="text" icon="mdi-plus" size="x-large" @click="setpointRaise"></v-btn>
                </div>
                <span class="text-center py-3">CURRENT TEMPERATURE is {{ temperature }}°</span>

                <v-spacer></v-spacer>

                <v-card class="border-thin mx-auto mw-100 flex-shrink-0 my-1" variant="flat" rounded="xl" v-if="fanSpeeds?.length">
                    <template #text>
                        <div class="d-flex ga-3 overflow-x-scroll no-scrollbar">
                            <v-spacer></v-spacer>
                            <v-btn v-for="button in fanSpeeds" 
                            :variant="button.model ? 'flat' : 'tonal'" 
                            :color="extractTextColor(button.icon) || (button.model ? 'primary' : button.color) || undefined"
                            :icon="button.icon"  @click="button.click"
                            ></v-btn>
                            <v-spacer></v-spacer>
                        </div>
                    </template>
                </v-card>
                <span class="text-center my-2" v-if="fanSpeeds?.length && fanSpeedState">
                    {{ fanSpeedState }}
                </span>
                <v-card v-if="modes?.length" class="border-thin mx-auto mw-100 flex-shrink-0 my-1" variant="flat" rounded="xl">
                    <template #text>
                        <div class="d-flex ga-3 overflow-x-scroll no-scrollbar">
                            <v-spacer></v-spacer>
                            <v-btn v-for="button in modes" :variant="button.model ? 'flat' : 'tonal'" 
                            :color="extractTextColor(button.icon) || (button.model ? 'primary' : button.color) || undefined"
                            :icon="button.icon"  @click="button.click"></v-btn>
                            <v-spacer></v-spacer>
                        </div>
                    </template>
                </v-card>
                <span class="text-center my-2" v-if="modes?.length && modeState">
                    {{ modeState }}
                </span>
                <v-spacer></v-spacer>
                <!-- Toggles -->
               
                    <v-switch
                    v-for="toggle in toggles"
                    @click="toggle.click"
                    class="text-truncate flex-shrink-0 ms-2"
                    v-model="toggle.model"
                    :inset="true"
                    density="compact"
                    :color="toggle.color || 'success'"
                    :hide-details="true"
                    :label="toggle.label"
                    ></v-switch>
            </div>
        </v-card-text>
    </v-card>
</template>

<script setup>
import vuetify from '@/plugins/vuetify';
import { useSystemFeedbackStore } from '@/store/useSystemFeedbackStore';
import { computed } from 'vue';
import { pulse } from '../../../composables/useInteractions'
import useClimateZone from '../../../composables/currentRoom/useClimateZone'

const props = defineProps(['index'])

const systemFeedbackStore = useSystemFeedbackStore()

const { xs, sm } = vuetify.display

const commandPrefix = `climateZone[${props.index || 0}]`

const { 
    label,
    fanSpeeds, 
    modes, 
    toggles, 
    modeState, 
    fanSpeedState, 
    setpoint,
    temperature,
    hasEnable,
    hasScheduler,
    enableFb,
    enable,
    setpointRaise, 
    setpointLower 
} = useClimateZone(props.index)

const cardTextHeightClass = computed(() => {
    if((label.value || hasEnable.value) && hasScheduler.value) return 'climate-zone-card-text-title'
    else if(label.value || hasEnable.value) return 'climate-zone-card-text-title'
    else if(hasScheduler.value) return 'climate-zone-card-text-actions'
    else return 'climate-zone-card-text'
})

const cardPaddingClass = computed(() => {
    if(!label.value && !hasEnable.value && !hasScheduler.value) return 'py-4'
    else if(!label.value && !hasEnable.value && !hasScheduler.value) return 'pt-4'
    //else if(!hasScheduler.value) return 'pb-4'
    else return 'pb-4'
})

function extractTextColor(str) {
  const match = str.match(/\bcolor-([^\s]+)/);
  return match ? match[1] : null;
}

</script>

<style scoped>
.climate-zone-card-text{
    height: calc(100%) !important;
}
.climate-zone-card-text-actions{
    height: calc(100% - 64px) !important;
}
.climate-zone-card-text-title{
    height: calc(100% - 60px) !important;
}
.climate-zone-card-text-actions-title{
    height: calc(100% - 64px - 60px) !important;
}
</style>