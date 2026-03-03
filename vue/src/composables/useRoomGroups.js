import { useSystemFeedbackStore } from "@/store/useSystemFeedbackStore"
import { computed } from "vue"

export default function useProperty(){

    const systemFeedbackStore = useSystemFeedbackStore()
    const commandPrefix = "roomGroups"


    return {
        commandPrefix,
    }
} 