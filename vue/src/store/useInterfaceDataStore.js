import { defineStore } from 'pinia'

export const useInterfaceDataStore = defineStore('interfaceData', {

    state: () => ({
        data: {
            room: 0,
            roomGroup: 'all',
            roomWindowItem: 0,
            roomSelectView: 0,
            roomSelectedCategory: undefined,
            
            propertySelectedCategory: undefined,
            propertyWindowItem: 0,
        }
    }),
    getters: {

    },
    actions: {

    }
})