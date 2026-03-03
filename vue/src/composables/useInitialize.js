import { computed, ref, watch } from "vue";
import processFeedback, { webXPanel } from '@/plugins/crestron'
import { useSystemFeedbackStore } from "@/store/useSystemFeedbackStore";

// Set to true to log notices to console
const debug = true

export default function useInitialize(){

    const systemFeedbackStore = useSystemFeedbackStore()

    const initializing = ref(false)

    async function initialize(){
        initializing.value = true

        // Register the web xpanel. This will run regardless of interface type.
        if(debug) console.log("Init: Registering Web XPanel.")
        webXPanel()

        // Subscribe to all the feedback joins registered in the crestron.js plugin
        if(debug) console.log("Init: Subscribing to feedback joins.")
        processFeedback()

        // Ready feedback received from control system to signal initialization complete
        const ready = computed(() => systemFeedbackStore.getPropertyValue('ui.readyFb', 'digital'))

        if (ready.value) {
            initializing.value = false
        } else {
            console.log("UI not ready...")   

            // Await the ready feedback asynchronously
            await new Promise(resolve => {
                const stop = watch(ready, (val) => {
                    if (val) {
                        stop()        // stop watching once ready
                        resolve()
                    }
                })
            })

            initializing.value = false
        }  
    }
    

    return {
        initialize,
        initializing
    }
}