import { useSystemFeedbackStore } from "@/store/useSystemFeedbackStore"
import { computed } from "vue"
import { pulse } from "../useInteractions"

const maxQuickActions = 20

export default function useProperty(){

    // Constants
    const systemFeedbackStore = useSystemFeedbackStore()
    const commandPrefix = "property"
    const quickActionCommandPrefix = "property.quickActions"

    // Computed properties
    const name = computed(() => systemFeedbackStore.getPropertyValue(`${commandPrefix}.nameFb`, 'serial'))

    const hasLighting = computed(() => systemFeedbackStore.getPropertyValue(`${commandPrefix}.lighting.visibleFb`, 'digital'))
    const hasShading = computed(() => systemFeedbackStore.getPropertyValue(`${commandPrefix}.shading.visibleFb`, 'digital'))
    const hasClimate = computed(() => systemFeedbackStore.getPropertyValue(`${commandPrefix}.climate.visibleFb`, 'digital'))
    const hasEntertainment = computed(() => systemFeedbackStore.getPropertyValue(`${commandPrefix}.entertainment.visibleFb`, 'digital'))
    const hasSecurity = computed(() => systemFeedbackStore.getPropertyValue(`${commandPrefix}.security.visibleFb`, 'digital'))
    const hasFloorplan = computed(() => systemFeedbackStore.getPropertyValue(`${commandPrefix}.floorplan.visibleFb`, 'digital'))
    const hasSystem = computed(() => systemFeedbackStore.getPropertyValue(`${commandPrefix}.systemHealth.visibleFb`, 'digital'))
    const hasQuickActions = computed(() => systemFeedbackStore.getPropertyValue(`${commandPrefix}.quickActions.visibleFb`, 'digital')) 

    const allQuickActionLabels = computed(() => systemFeedbackStore.getSerialArray(quickActionCommandPrefix, maxQuickActions, 'labelFb'))
    const quickActions = computed(() => allQuickActionLabels.value?.map((quickActionName, quickActionIndex) => {
        let commandPrefix = `${quickActionCommandPrefix}[${quickActionIndex}]`
        return {
            label: quickActionName,
            commandPrefix: commandPrefix,
            index: quickActionIndex,
            id: quickActionIndex + 1,
            description: systemFeedbackStore.getPropertyValue(`${commandPrefix}.descriptionFb`, 'serial'),
            icon: systemFeedbackStore.getPropertyValue(`${commandPrefix}.iconFb`, 'serial'),
            color: systemFeedbackStore.getPropertyValue(`${commandPrefix}.colorFb`, 'serial'),
            visible: systemFeedbackStore.getPropertyValue(`${commandPrefix}.visibleFb`, 'digital'),
            model: systemFeedbackStore.getPropertyValue(`${commandPrefix}.pressFb`, 'digital'),
            inProgress: systemFeedbackStore.getPropertyValue(`${commandPrefix}.inProgressFb`, 'digital'),

            // Methods
            'click': () => pulse(`${commandPrefix}.press`),
        }
    }).filter(quickAction => quickAction.visible))

    // Methods


    return {
        commandPrefix,

        name,
        hasLighting,
        hasShading,
        hasClimate,
        hasSecurity,
        hasFloorplan,
        hasEntertainment,
        hasSystem,
        hasQuickActions,

        quickActions
    }
} 