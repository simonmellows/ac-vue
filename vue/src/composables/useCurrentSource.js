import { useSystemFeedbackStore } from "@/store/useSystemFeedbackStore";
import { computed } from "vue";
import { pulse } from "./useInteractions";

export default function useCurrentSource(scope){
    
    const systemFeedbackStore = useSystemFeedbackStore()
    const commandPrefix = (scope === "watch") ? "currentSourceWatch" : "currentSourceListen" 

    const label = computed(() => systemFeedbackStore.getPropertyValue(`${commandPrefix}.labelFb`, "serial") || "genericUncontrolledSource")
    const type = computed(() => systemFeedbackStore.getPropertyValue(`${commandPrefix}.typeFb`, "serial") || "unnamed")

    function play(){
        pulse(`${commandPrefix}.play`)
    }
    function stop(){
        pulse(`${commandPrefix}.stop`)
    }
    function pause(){
        pulse(`${commandPrefix}.pause`)
    }

    return {
        label,
        type,

        play,
        stop,
        pause,
    }
}