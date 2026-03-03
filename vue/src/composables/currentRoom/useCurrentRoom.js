import { useSystemFeedbackStore } from "@/store/useSystemFeedbackStore";
import { computed } from "vue";
import { pulse } from "../useInteractions";

export default function useCurrentRoom(){
    
    const systemFeedbackStore = useSystemFeedbackStore()
    const commandPrefix = "currentRoom"

    const label = computed(() => systemFeedbackStore.getPropertyValue(`${commandPrefix}.labelFb`, "serial"))
    const hasLighting = computed(() => systemFeedbackStore.getPropertyValue(`${commandPrefix}.lighting.visibleFb`, "digital"))
    const hasShading = computed(() => systemFeedbackStore.getPropertyValue(`${commandPrefix}.shading.visibleFb`, "digital"))
    const hasClimate = computed(() => systemFeedbackStore.getPropertyValue(`${commandPrefix}.climate.visibleFb`, "digital"))
    const hasWatch = computed(() => systemFeedbackStore.getPropertyValue(`${commandPrefix}.watch.visibleFb`, "digital"))
    const hasListen = computed(() => systemFeedbackStore.getPropertyValue(`${commandPrefix}.listen.visibleFb`, "digital"))

    const lightingToggleFb = computed(() => systemFeedbackStore.getPropertyValue(`${commandPrefix}.lighting.toggleFb`, "digital"))
    function lightingToggle(){
        pulse(`${commandPrefix}.lighting.toggle`)
    }
    const shadingToggleFb = computed(() => systemFeedbackStore.getPropertyValue(`${commandPrefix}.shading.toggleFb`, "digital"))
    function shadingToggle(){
        pulse(`${commandPrefix}.shading.toggle`)
    }
    const climateToggleFb = computed(() => systemFeedbackStore.getPropertyValue(`${commandPrefix}.climate.toggleFb`, "digital"))
    function climateToggle(){
        pulse(`${commandPrefix}.climate.toggle`)
    }
    const watchToggleFb = computed(() => systemFeedbackStore.getPropertyValue(`${commandPrefix}.watch.toggleFb`, "digital"))
    function watchToggle(){
        pulse(`${commandPrefix}.watch.toggle`)
    }
    const listenToggleFb = computed(() => systemFeedbackStore.getPropertyValue(`${commandPrefix}.listen.toggleFb`, "digital"))
    function listenToggle(){
        pulse(`${commandPrefix}.listen.toggle`)
    }

    return {
        commandPrefix,
        
        label,
        hasLighting,
        hasShading,
        hasClimate,
        hasWatch,
        hasListen,

        lightingToggleFb,
        shadingToggleFb,
        climateToggleFb,
        watchToggleFb,
        listenToggleFb,

        lightingToggle,
        shadingToggle,
        climateToggle,
        watchToggle,
        listenToggle,
    }
}