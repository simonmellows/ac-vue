import { useSystemFeedbackStore } from "@/store/useSystemFeedbackStore";
import { computed } from "vue";
import { press, pulse, release } from "../useInteractions";

export default function useClimateZone(index){

    const systemFeedbackStore = useSystemFeedbackStore()
    const commandPrefix = `currentRoom.climate.zones[${index || 0}]`

    const label = computed(() => systemFeedbackStore.getPropertyValue(`${commandPrefix}.labelFb`, 'serial'))
    const setpoint = computed(() => systemFeedbackStore.getPropertyValue(`${commandPrefix}.setpointFb`, 'serial'))
    const temperature = computed(() => systemFeedbackStore.getPropertyValue(`${commandPrefix}.temperatureFb`, 'serial'))
    const fanSpeeds = computed(() => getButtonArray(`${commandPrefix}.fanSpeeds`))
    const fanSpeedState = computed(() => systemFeedbackStore.getPropertyValue(`${commandPrefix}.fanStateFb`, 'serial'))
    const modes = computed(() => getButtonArray(`${commandPrefix}.modes`))
    const modeState = computed(() => systemFeedbackStore.getPropertyValue(`${commandPrefix}.modeStateFb`, 'serial'))
    const toggles = computed(() => getButtonArray(`${commandPrefix}.toggles`, 3))
    const enableFb = computed(() => systemFeedbackStore.getPropertyValue(`${commandPrefix}.enableFb`, 'digital'))
    
    // Visibility
    const hasEnable = computed(() => systemFeedbackStore.getPropertyValue(`${commandPrefix}.hasEnableFb`, 'digital'))
    const hasScheduler = computed(() => systemFeedbackStore.getPropertyValue(`${commandPrefix}.hasSchedulerFb`, 'digital'))

    function enable(){
        pulse(`${commandPrefix}.enable`)
    }
    function setpointRaise(value){
        if(value === true) press(`${commandPrefix}.setpointRaise`)
        else if(value === false) release(`${commandPrefix}.setpointRaise`)
        else pulse(`${commandPrefix}.setpointRaise`)
    }
    function setpointLower(value){
        if(value === true) press(`${commandPrefix}.setpointLower`)
        else if(value === false) release(`${commandPrefix}.setpointLower`)
        else pulse(`${commandPrefix}.setpointLower`)
    }

    // Helper
    function getButtonArray(commandPrefix, max = 5){
        let arr = []
        for (let index = 0; index < max; index++) {
            arr.push({
                icon: systemFeedbackStore.getPropertyValue(`${commandPrefix}[${index}].iconFb`, 'serial'),
                label: systemFeedbackStore.getPropertyValue(`${commandPrefix}[${index}].labelFb`, 'serial'),
                visible: systemFeedbackStore.getPropertyValue(`${commandPrefix}[${index}].visibleFb`, 'digital'),
                model: systemFeedbackStore.getPropertyValue(`${commandPrefix}.pressFb`, 'digital'),
                click: () => pulse(`${commandPrefix}[${index}].press`),
            })
        }
        return arr.filter(item => item.visible)
    }

    return {
        commandPrefix,
        
        label,
        setpoint,
        temperature,
        fanSpeeds,
        fanSpeedState,
        modes,
        modeState,
        toggles,
        hasEnable,
        hasScheduler,
        enableFb,

        enable,
        setpointRaise,
        setpointLower,
    }
}