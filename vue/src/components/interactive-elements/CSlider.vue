<template>
    <v-slider
    v-model="sliderValue"
    @touchstart="sliderPress()"
    @touchend="sliderRelease()" 
    @update:model-value="(newLevel) => setLevel(newLevel)" 
    v-on:touchstart.stop 
    v-on:touchmove.stop
    v-on:touchend.stop
    >
    </v-slider>

</template>

<script setup>
import { computed, ref, watch } from 'vue'
import { press, release, setAnalog } from '../../composables/useInteractions'
import { useSystemFeedbackStore } from '../../store/useSystemFeedbackStore'

// Props
const props = defineProps({
    joinPress: { type: [Number, String] },
    joinLevel: { type: [Number, String] },
    joinLevelFb: { type: [Number, String] },
    joinEnable: { type: [Number, String] },
    joinShow: { type: [Number, String] },
    timeout: { type: Number, default: 3000 },
})

// Emits
const emit = defineEmits([
    'press',
    'release',
    'sliderValue'
])

// Store
const systemFeedbackStore = useSystemFeedbackStore()

const level = computed(() => systemFeedbackStore.getPropertyValue(props.joinLevelFb, 'analog'))

console.log(props.joinLevelFb)

const sliderValue = ref(level.value);
const sliderInUse = ref(false);
  
// Watch the computed property and update sliderValue when it changes
watch(level, (newValue) => {
    if (!sliderInUse.value) {
        sliderValue.value = newValue;
    }
});
  
function setLevel(v){
    setAnalog(props.joinLevel, v)
}
  
function sliderInteract(state) {
    if (state) {
        sliderInUse.value = true;
    } 
    else {
        sliderInUse.value = false;
        setTimeout(() => {
            sliderValue.value = level.value;
        }, props.timeout);
    }
}
  
function sliderPress(){
    emit('press')
    sliderInteract(true)
    if(props.joinPress) press(props.joinPress)
}
function sliderRelease(){
    emit('release')
    sliderInteract(false)
    if(props.joinPress) release(props.joinPress)
}

// Emit the slider value
watch(sliderValue, (newValue) => {
    emit('sliderValue', newValue)
})

</script>

<style scoped>
::v-deep(.slider-px .v-input__control){
    padding-left: 1rem !important;
    padding-right: 1rem !important;
}
::v-deep(.slider-px-2 .v-input__control){
    padding-left: 2rem !important;
    padding-right: 2rem !important;
}
::v-deep(.slider-px-3 .v-input__control){
    padding-left: 3rem !important;
    padding-right: 3rem !important;
}
::v-deep(.slider-py .v-input__control){
    padding-top: 1rem !important;
    padding-bottom: 1rem !important;
}
::v-deep(.slider-py-2 .v-input__control){
    padding-top: 2rem !important;
    padding-bottom: 2rem !important;
}
::v-deep(.slider-py-3 .v-input__control){
    padding-top: 3rem !important;
    padding-bottom: 3rem !important;
}
</style>