import { defineStore } from 'pinia'

export const useSystemFeedbackStore = defineStore('systemFeedback', {
    state: () => ({
        digital: {
            // Reserved Joins
            "Csig.Control_Systems_Offline_fb": false,

            // property
            "property.lighting.visibleFb": true,
            "property.shading.visibleFb": true,
            "property.climate.visibleFb": true,
            "property.entertainment.visibleFb": true,
            "property.security.visibleFb": true,
            "property.floorplan.visibleFb": true,
            "property.systemHealth.visibleFb": true,

            "property.quickActions[0].visibleFb": true,
            "property.quickActions[0].inProgressFb": true,

            // ui
            "ui.readyFb": true,  

            // currentRoom.lighting
            "currentRoom.lighting.visibleFb": true,
            "currentRoom.lighting.scenes[0].visibleFb": true,
            "currentRoom.lighting.scenes[1].visibleFb": true,
            "currentRoom.lighting.scenes[2].visibleFb": true,
            "currentRoom.lighting.scenes[3].visibleFb": true,
            "currentRoom.lighting.circuits[0].visibleFb": true,
            "currentRoom.lighting.circuits[1].visibleFb": true,
            "currentRoom.lighting.circuits[2].visibleFb": true,

            // currentRoom.shading
            "currentRoom.shading.visibleFb": true,

            // currentRoom.climate
            "currentRoom.climate.visibleFb": true,
            "currentRoom.climate.zones[0].hasSchedulerFb": true,
            "currentRoom.climate.zones[0].hasEnableFb": true,
            "currentRoom.climate.zones[0].fanSpeeds[0].visibleFb": true,
            "currentRoom.climate.zones[0].fanSpeeds[1].visibleFb": true,
            "currentRoom.climate.zones[0].fanSpeeds[2].visibleFb": true,
            "currentRoom.climate.zones[0].fanSpeeds[3].visibleFb": true,
            "currentRoom.climate.zones[0].fanSpeeds[4].visibleFb": true,
            "currentRoom.climate.zones[0].modes[0].visibleFb": true,
            "currentRoom.climate.zones[0].modes[1].visibleFb": true,
            "currentRoom.climate.zones[0].modes[2].visibleFb": true,
            "currentRoom.climate.zones[0].toggles[0].visibleFb": true,

            // currentRoom.watch
            "currentRoom.watch.visibleFb": true,
            "currentRoom.watch.sources[0].visibleFb": true,
            "currentRoom.watch.sources[1].visibleFb": true,
            "currentRoom.watch.sources[1].inUseFb": true,

            // currentRoom.listen
            "currentRoom.listen.visibleFb": true,
            "currentRoom.listen.sources[0].visibleFb": true,
            "currentRoom.listen.sources[1].visibleFb": true,
            "currentRoom.listen.sources[2].visibleFb": true,
            "currentRoom.listen.sources[1].inUseFb": true,

            // rooms
            "rooms[0].lighting.visibleFb": true,
            "rooms[0].shading.visibleFb": true,
            "rooms[0].favouriteFb": true,
            "rooms[1].favouriteFb": true,
            "rooms[2].favouriteFb": true,
            "rooms[4].favouriteFb": true,
            "rooms[0].visibleFb": true,
            "rooms[1].visibleFb": true,
            "rooms[2].visibleFb": true,
            "rooms[3].visibleFb": true,
            "rooms[4].visibleFb": true,

            "rooms[0].lighting.scenes[0].visibleFb": true,
            "rooms[0].lighting.scenes[1].visibleFb": true,
            "rooms[0].lighting.scenes[2].visibleFb": true,
            "rooms[0].lighting.scenes[3].visibleFb": true,

            "rooms[0].shading.scenes[0].visibleFb": true,
            "rooms[0].shading.scenes[1].visibleFb": true,
            "rooms[0].shading.scenes[2].visibleFb": true,
            "rooms[0].shading.scenes[3].visibleFb": true,

            // roomGroups
            "roomGroups[0].visibleFb": true,
            "roomGroups[1].visibleFb": true,
            "roomGroups[2].visibleFb": true,
            "roomGroups[3].visibleFb": true,
            "roomGroups[4].visibleFb": true,
        },
        analog: {
            // roomGroups
            "rooms[0].roomGroupFb": 0,
            "rooms[1].roomGroupFb": 0,
            "rooms[2].roomGroupFb": 1,
            "rooms[3].roomGroupFb": 1,
            "rooms[4].roomGroupFb": 2,
        },
        serial: {

            // property
            "property.nameFb": "Oak Hill Court",

            "property.quickActions[0].labelFb": "Quick Action 1",
            "property.quickActions[0].descriptionFb": "Quick Action 1",
            "property.quickActions[0].colorFb": "primary",

            // currentRoom
            "currentRoom.labelFb": "Kitchen",

            // currentRoom.lighting
            "currentRoom.lighting.scenes[0].labelFb": "Scene #1",
            "currentRoom.lighting.scenes[1].labelFb": "Scene #2",
            "currentRoom.lighting.scenes[2].labelFb": "Scene #3",
            "currentRoom.lighting.scenes[3].labelFb": "Scene #4",
            "currentRoom.lighting.circuits[0].labelFb": "Circuit #1",
            "currentRoom.lighting.circuits[1].labelFb": "Circuit #2",
            "currentRoom.lighting.circuits[2].labelFb": "Circuit #3",

            // currentRoom.watch
            "currentRoom.watch.sources[0].labelFb": "Apple TV #1",
            "currentRoom.watch.sources[1].labelFb": "Sky Q #1",
            "currentRoom.watch.sources[0].iconFb": "mdi-television",
            "currentRoom.watch.sources[1].iconFb": "mdi-television",

            // currentRoom.listen
            "currentRoom.listen.sources[0].labelFb": "Sonos #1",
            "currentRoom.listen.sources[1].labelFb": "Sonos #2",
            "currentRoom.listen.sources[2].labelFb": "Sonos #3",
            "currentRoom.listen.sources[0].iconFb": "mdi-music",
            "currentRoom.listen.sources[1].iconFb": "mdi-music",
            "currentRoom.listen.sources[2].iconFb": "mdi-music",

            // currentRoom.climate.zones[0]
            "currentRoom.climate.zones[0].setpointFb": "22.0",
            "currentRoom.climate.zones[0].temperatureFb": "22",
            "currentRoom.climate.zones[0].labelFb": "Zone #1",
            "currentRoom.climate.zones[0].fanStateFb": "FANS are OFF",
            "currentRoom.climate.zones[0].fanSpeeds[0].iconFb": "mdi-fan-off",
            "currentRoom.climate.zones[0].fanSpeeds[1].iconFb": "mdi-fan-speed-1",
            "currentRoom.climate.zones[0].fanSpeeds[2].iconFb": "mdi-fan-speed-2",
            "currentRoom.climate.zones[0].fanSpeeds[3].iconFb": "mdi-fan-speed-3",
            "currentRoom.climate.zones[0].fanSpeeds[4].iconFb": "mdi-fan-auto",
            "currentRoom.climate.zones[0].modeStateFb": "MODE is OFF",
            "currentRoom.climate.zones[0].modes[0].iconFb": "mdi-power color-error",
            "currentRoom.climate.zones[0].modes[1].iconFb": "mdi-fire color-orange",
            "currentRoom.climate.zones[0].modes[2].iconFb": "mdi-snowflake color-info",
            "currentRoom.climate.zones[0].toggles[0].labelFb": "TOWEL RAIL",

            // currentRoom.shading
            "currentRoom.shading.scenes[0].labelFb": "Scene #1",
            "currentRoom.shading.scenes[1].labelFb": "Scene #2",
            "currentRoom.shading.scenes[2].labelFb": "Scene #3",
            "currentRoom.shading.scenes[3].labelFb": "Scene #4",
            "currentRoom.climate.circuits[0].labelFb": "Circuit #1",
            "currentRoom.climate.circuits[1].labelFb": "Circuit #2",
            "currentRoom.climate.circuits[2].labelFb": "Circuit #3",

            // currentSourceWatch
            "currentSourceWatch.labelFb": "Sky Q #1",
            "currentSourceWatch.typeFb": "skyHd",

            // roomGroups
            "roomGroups[0].labelFb": "Basement",
            "roomGroups[1].labelFb": "Ground",
            "roomGroups[2].labelFb": "First",
            "roomGroups[3].labelFb": "Second",
            "roomGroups[4].labelFb": "Third",
            "roomGroups[0].floorplanFileNameFb": "floorplan1",
            "roomGroups[1].floorplanFileNameFb": "floorplan2",
            "roomGroups[2].floorplanFileNameFb": "floorplan3",
            "roomGroups[3].floorplanFileNameFb": "floorplan4",
            "roomGroups[4].floorplanFileNameFb": "floorplan5",

            // rooms
            "rooms[0].labelFb": "Kitchen",
            "rooms[1].labelFb": "Living Room",
            "rooms[2].labelFb": "Dining Room",
            "rooms[3].labelFb": "Powder Room",
            "rooms[4].labelFb": "Master Bedroom",
            "rooms[5].labelFb": "Master Bathroom",
            "rooms[6].labelFb": "Bedroom 1",
            "rooms[7].labelFb": "Bathroom 1",

            "rooms[0].lighting.scenes[0].labelFb": "Scene #1",
            "rooms[0].lighting.scenes[1].labelFb": "Scene #2",
            "rooms[0].lighting.scenes[2].labelFb": "Scene #3",
            "rooms[0].lighting.scenes[3].labelFb": "Scene #4",

            "rooms[0].shading.scenes[0].labelFb": "Scene #1",
            "rooms[0].shading.scenes[1].labelFb": "Scene #2",
            "rooms[0].shading.scenes[2].labelFb": "Scene #3",
            "rooms[0].shading.scenes[3].labelFb": "Scene #4",
        }
    }),
    getters: {
        getPropertyValue: (state) => (join, type) => {
            if(join){
                if(type === "digital"){
                    return state.digital[join.toString()]
                }
                else if(type === "analog"){
                    return state.analog[join.toString()]
                }
                else if(type === "serial"){
                    return state.serial[join.toString()]
                }
                else return null
            }
            else return null
        }
    },
    actions: {
        getDigitalArray(prefix, count, suffix){
            let arr = []
            for (let index = 0; index < (count || 0); index++) {
                let command = `${prefix}[${index}].${suffix || ''}`
                arr.push(this.getPropertyValue(command, 'digital') || false)
            }
            return arr
        },
        getAnalogArray(prefix, count, suffix){
            let arr = []
            for (let index = 0; index < (count || 0); index++) {
                let command = `${prefix}[${index}].${suffix || ''}`
                arr.push(this.getPropertyValue(command, 'analog')|| 0)
            }
            return arr
        },
        getSerialArray(prefix, count, suffix){
            let arr = []
            for (let index = 0; index < (count || 0); index++) {
                let command = `${prefix}[${index}].${suffix || ''}`
                arr.push(this.getPropertyValue(command, 'serial') || "")
            }
            return arr
        },
        initState(){
            this.digital = {}
            this.analog = {}
            this.serial = {}
        }
    }
}) 