import { getWebXPanel, runsInContainerApp } from "@crestron/ch5-webxpanel"; 
import { ref } from 'vue';
import { useSystemFeedbackStore } from '../store/useSystemFeedbackStore';
import contract from '../data/contract.json';

import * as CrComLib from '@crestron/ch5-crcomlib/build_bundles/amd/cr-com-lib'
import { bridgeReceiveIntegerFromNative, bridgeReceiveBooleanFromNative, bridgeReceiveStringFromNative, bridgeReceiveObjectFromNative } from '@crestron/ch5-crcomlib/build_bundles/amd/cr-com-lib';

window.CrComLib = CrComLib;
window['bridgeReceiveIntegerFromNative'] = bridgeReceiveIntegerFromNative;
window['bridgeReceiveBooleanFromNative'] = bridgeReceiveBooleanFromNative;
window['bridgeReceiveStringFromNative'] = bridgeReceiveStringFromNative;
window['bridgeReceiveObjectFromNative'] = bridgeReceiveObjectFromNative;

const debug = true
const test = true // Make sure to set to false when deployed for production!

const reservedJoins = {
    digital: [
        "Csig.All_Control_Systems_Online_fb",
        "Csig.Control_Systems_Offline_fb",
        "Csig.Landscape_Orientation_fb",
        "Csig.Portrait_Orientation_fb",
    ],
    analog: [

    ],
    serial: [
        "Csig.Ip_Address_fb"
    ]
}

const boolean = Object.values(contract?.signals?.states?.boolean || {})?.flatMap(item => Object.values(item || {}))?.concat(reservedJoins.digital) || []
const numeric = Object.values(contract?.signals?.states?.numeric || {})?.flatMap(item => Object.values(item || {}))?.concat(reservedJoins.analog) || []
const string = Object.values(contract?.signals?.states?.string || {})?.flatMap(item => Object.values(item || {}))?.concat(reservedJoins.serial) || []

// Helper function for Xpanel IP ID entry in the URL
function normalizeIpId(value, defaultValue = "0x03") {
  if (!value) return defaultValue;

  // Ensure it's valid hex (1–2 hex digits, e.g. 3, A, 0F, FE)
  if (/^[0-9a-fA-F]{1,2}$/.test(value)) {
    return "0x" + value.toUpperCase().padStart(2, "0");
  }

  // If invalid → fallback
  return defaultValue;
}

export default function processFeedback() {

    const systemFeedbackStore = useSystemFeedbackStore()

    if(!test){
        systemFeedbackStore.initState();
        boolean.forEach(join => {
            CrComLib.subscribeState('b', `${join}`, (value) => {
                if(debug) console.log(`Received ${value} on digital index ${join}`)
                systemFeedbackStore.digital[`${join}`] = value
            });
        })
        numeric.forEach(join => {
            CrComLib.subscribeState('n', `${join}`, (value) => {
                if(debug) console.log(`Received ${value} on analog join ${join}`)
                systemFeedbackStore.analog[`${join}`] = value
            });
        })
        string.forEach(join => {
            CrComLib.subscribeState('s', `${join}`, (value) => {
                if(debug) console.log(`Received ${value} on serial join ${join}`)
                systemFeedbackStore.serial[`${join}`] = value
            });
        })
    }
}

export function getSerialArray(start, count, span, blanks){
    const systemFeedbackStore = useSystemFeedbackStore()
    let arr = []
    for(let x = 0; x < count; x++){
        if(systemFeedbackStore.getSerialValue(start + (x*span)) && !blanks){
            arr.push(systemFeedbackStore.getSerialValue(start + (x*span)))
        }
        else if(blanks){
            arr.push(systemFeedbackStore.getSerialValue(start + (x*span)))
        }
    }
    return arr
}
export function getAnalogArray(start, count, span, blanks){
    const systemFeedbackStore = useSystemFeedbackStore()
    let arr = []
    for(let x = 0; x < count; x++){
        if(systemFeedbackStore.getAnalogValue(start + (x*span)) && !blanks){
            arr.push(systemFeedbackStore.getAnalogValue(start + (x*span)))
        }
        else if(blanks){
            arr.push(systemFeedbackStore.getAnalogValue(start + (x*span)))
        }
    }
    return arr
}
export function getDigitalArray(start, count, span, blanks){
    const systemFeedbackStore = useSystemFeedbackStore()
    let arr = []
    for(let x = 0; x < count; x++){
        if(systemFeedbackStore.getDigitalValue(start + (x*span)) && !blanks){
            arr.push(systemFeedbackStore.getDigitalValue(start + (x*span)))
        }
        else if(blanks){
            arr.push(systemFeedbackStore.getDigitalValue(start + (x*span)))
        }
    }
    return arr
}

export function webXPanel(){
    const {
        WebXPanel,
        isActive,
        WebXPanelEvents,
        getVersion,
        getBuildDate,
      } = getWebXPanel(!runsInContainerApp());

    const active = ref(false)
    const error = ref(null)
    const connected = ref(false)
    const disconnected = ref(false)
    const licenseInfo = ref(null)

    if(debug){
        console.log(`WebXPanel version: ${getVersion()}`); 
        console.log(`WebXPanel build date: ${getBuildDate()}`);
    } 

    const params = new URLSearchParams(window?.location?.search);
    const ipid = params.get("ipid");

    const configuration = { 
        host: window?.location?.host,
        ipId: normalizeIpId(ipid),
    }; 
    
    // Only initialize the Web XPanel if it's not loaded on a Crestron CIP device and program is NOT in test mode
    if (isActive && !test){
        console.log("Initializing XPanel with the following credentials: ", configuration)
        WebXPanel.initialize(configuration); 
        active.value = true
    } 
    if(debug) console.log("WebXPanel: ", WebXPanel)

    WebXPanel.addEventListener(WebXPanelEvents.CONNECT_WS, () => {
        if(debug) console.log("WebXpanel connection established!");
        disconnected.value = false
        connected.value = true
    });

    WebXPanel.addEventListener(WebXPanelEvents.DISCONNECT_WS, () => {
        if(debug) console.log("WebXpanel disconnected!");
        disconnected.value = true
        connected.value = false
    });

    WebXPanel.addEventListener(WebXPanelEvents.ERROR_WS, () => {
        if(debug) console.log('WebXPanel WebSocket Error');
        error.value = "WebXPanel WebSocket Error"
    });

    WebXPanel.addEventListener(WebXPanelEvents.WEB_WORKER_FAILED, () => {
        if(debug) console.log('WebXPanel Web Worker Failed');
        error.value = "WebXPanel Web Worker Failed"
    });

    WebXPanel.addEventListener(WebXPanelEvents.AUTHENTICATION_FAILED, () => {
        if(debug) console.log('WebXPanel Authentication Failed');
        error.value = "WebXPanel Authentication Failed"
    });

    WebXPanel.addEventListener(WebXPanelEvents.AUTHENTICATION_REQUIRED, () => {
        if(debug) console.log('WebXPanel Authentication Required');
        error.value = "WebXPanel Authentication Required"
    });

    WebXPanel.addEventListener(WebXPanelEvents.CONNECT_CIP, ({detail}) => {
        const { url, ipId, roomId } = detail;
        if(debug) console.log(`WebXPanel Connected to ${url}, 0x${ipId.toString(16)}, ${roomId}`);
        disconnected.value = false
        connected.value = true
    });

    WebXPanel.addEventListener(WebXPanelEvents.DISCONNECT_CIP, ({detail}) => {
        if(debug) console.log(`WebXpanel Disconnected from CIP. Details: ${detail}`);
        disconnected.value = false
        connected.value = true
    });

    WebXPanel.addEventListener(WebXPanelEvents.LICENSE_WS, ({detail}) => {
        if(debug) console.log('WebXPanel License Info: ', detail);
        licenseInfo.value = detail
    });

    WebXPanel.addEventListener(WebXPanelEvents.NOT_AUTHORIZED, () => {
        if(debug) console.log('WebXPanel Not Authorized');
        error.value = "WebXPanel Not Authorized"
    });

    return {
        active,
        error,
        connected,
        disconnected
    }
}
