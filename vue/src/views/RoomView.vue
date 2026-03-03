<template>
    <v-container fluid class="h-100 pb-2 px-2 d-flex flex-column">
        <div class="d-flex">
            <v-breadcrumbs :items="breadcrumbs" class="py-0 overflow-x-scroll no-scrollbar">
                <template v-slot:title="{ item }">
                    <span 
                    @click="item.click"
                    class="text-overline text-truncate"
                    style="letter-spacing: 0.8px;" 
                    >
                        {{ item?.label }}
                    </span>
                </template>
            </v-breadcrumbs>
            <v-spacer></v-spacer>
            <v-breadcrumbs class="py-0 overflow-x-scroll no-scrollbar" v-if="roomWindowItem === 0 && roomLabel">
                <v-breadcrumbs-item>
                    <div @click="roomWindowItem++" class="d-flex ga-2">
                        <span
                        style="letter-spacing: 0.8px;"
                        variant="text"
                        density="compact" 
                        class="text-overline my-auto px-0">
                        {{ roomLabel }}
                        </span>
                        <v-icon icon="mdi-chevron-right" class="my-auto text-overline" style="font-size: 0.9rem !important;"></v-icon>
                    </div>
                </v-breadcrumbs-item>
            </v-breadcrumbs>
        </div>
        <Dashboard
        :title="(roomWindowItem > 0) ? roomLabel : 'Rooms'"
        :showBack="roomWindowItem > 0"
        :subtitle="roomWindowItem < 1 ? 'Select Room' : roomWindowItem < 2 ? 'Dashboard' : selectedCategory?.label"
        @back="roomWindowItem--"
        >
            <template #body>
                <v-window v-model="roomWindowItem" class="h-100">
                    <!-- Room Select -->
                    <v-window-item class="h-100"
                    v-on:touchstart.stop 
                    v-on:touchmove.stop
                    v-on:touchend.stop>
                        <RoomSelect 
                        :view="roomSelectView"
                        @roomSelected="roomWindowItem++"
                        />
                    </v-window-item>
                    <!-- Room Dashboard -->
                    <v-window-item class="h-100"
                    v-on:touchstart.stop 
                    v-on:touchmove.stop
                    v-on:touchend.stop>
                        <v-container fluid class="h-100 py-0 overflow-y-scroll no-scrollbar">
                            <v-row>
                                <v-col v-for="category in categories" :key="category.id" :cols="12" sm="6" md="4" class="h-100">
                                    <DashboardCard :label="category.label" :icon="category.icon"
                                    @click="[roomWindowItem++, selectedCategory = category]">
                                        <template #append>
                                            <v-btn v-if="category?.toggleType === 'button'" 
                                            @click="category.toggle"
                                            @click.stop @mousedown.stop
                                            class="my-auto"
                                            icon="mdi-power" 
                                            :size="xs ? 'small' : undefined"
                                            :disabled="!category.model"
                                            :variant="category.model ? 'flat' : 'tonal'" 
                                            :color="category.model ? 'error' : undefined" 
                                            density="comfortable"
                                            >
                                            </v-btn>
                                            <v-switch v-else
                                            :inset="true" icon="mdi-chevron-right" variant="text" :density="xs ? 'compact' : 'comfortable'" 
                                            :hide-details="true"
                                            :color="category.color || 'success'"
                                            :model-value="category.model"
                                            @update:model-value="category.toggle"
                                            @click.stop @mousedown.stop
                                            >
                                            </v-switch>
                                        </template>
                                    </DashboardCard>
                                </v-col>
                            </v-row>
                        </v-container>
                    </v-window-item >
                    <!-- Room Category -->
                    <v-window-item class="h-100"
                    v-on:touchstart.stop 
                    v-on:touchmove.stop
                    v-on:touchend.stop  
                    >
                        <component 
                        :is="roomComponents[selectedCategory?.id]"
                        :type="selectedCategory?.id"
                        />
                    </v-window-item>
                    <!-- Source Control -->
                    <v-window-item class="h-100"
                    v-on:touchstart.stop 
                    v-on:touchmove.stop
                    v-on:touchend.stop  
                    >
                        <component 
                        :is="sourceComponents[selectedCategory.id === 'watch' ? currentWatchSourceType : currentListenSourceType] || sourceComponents['genericUncontrolledSource']"
                        />
                    </v-window-item>
                </v-window>
            </template>
        </Dashboard>
    </v-container>
</template>

<script setup>
import { computed, onBeforeUnmount, onMounted, ref, watch } from 'vue'
import roomComponents from '../components/room'
import { emitter } from '../plugins/eventBus'
import sourceComponents from '../components/sources'
import RoomSelect from '@/components/room-select/RoomSelect.vue'
import { useInterfaceDataStore } from '@/store/useInterfaceDataStore'
import useCurrentRoom from '../composables/currentRoom/useCurrentRoom'
import useCurrentSource from '@/composables/useCurrentSource'
import vuetify from '@/plugins/vuetify'
import categoryData from '../data/categories'
import Dashboard from '@/components/general/Dashboard.vue'
import DashboardCard from '@/components/general/DashboardCard.vue'

const interfaceDataStore = useInterfaceDataStore()
const selectedCategory = ref(interfaceDataStore.data?.roomSelectedCategory)
const roomSelectView = ref(interfaceDataStore.data?.roomSelectView)
const roomWindowItem = ref(interfaceDataStore.data?.roomWindowItem)

const { xs, lg } = vuetify.display

watch(roomWindowItem, (newValue) => {
    interfaceDataStore.data.roomWindowItem = newValue
})
watch(selectedCategory, (newValue) => {
    interfaceDataStore.data.roomSelectedCategory = newValue
})
watch(roomSelectView, (newValue) => {
    interfaceDataStore.data.roomSelectView = newValue
})

const {
    label: currentWatchSourceLabel,
    type: currentWatchSourceType,
} = useCurrentSource("watch")
const {
    label: currentListenSourceLabel,
    type: currentListenSourceType,
} = useCurrentSource("listen")

const { 
    label: roomLabel, 
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
} = useCurrentRoom()

const categories = computed(() => ([ 
    {
        id: categoryData.lighting?.id,
        label: categoryData.lighting?.label,
        icon: categoryData.lighting?.icon,
        color: categoryData.lighting?.color,
        model: lightingToggleFb.value,
        toggle: lightingToggle,
        visible: hasLighting.value,
    },
    {
        id: categoryData.shading?.id,
        label: categoryData.shading?.label,
        icon: categoryData.shading?.icon,
        color: categoryData.shading?.color,
        model: shadingToggleFb.value,
        toggle: shadingToggle,
        visible: hasShading.value,
    },
    {
        id: categoryData.climate?.id,
        label: categoryData.climate?.label,
        icon: categoryData.climate?.icon,
        color: categoryData.climate?.color,
        model: climateToggleFb.value,
        toggle: climateToggle,
        visible: hasClimate.value,
    },
    {
        id: categoryData.watch?.id,
        label: categoryData.watch?.label,
        icon: categoryData.watch?.icon,
        color: categoryData.watch?.color,
        model: watchToggleFb.value,
        toggle: watchToggle,
        toggleType: 'button',
        visible: hasWatch.value,
        sublabel: computed(() => roomWindowItem.value === 3 ? `${currentWatchSourceLabel.value}` : undefined),
        sublabelClass: 'text-primary',
    },
    {
        id: categoryData.listen?.id,
        label: categoryData.listen?.label,
        icon: categoryData.listen?.icon,
        color: categoryData.listen?.color,
        model: listenToggleFb.value,
        toggle: listenToggle,
        toggleType: 'button',
        visible: hasListen.value,
        sublabel: computed(() => roomWindowItem.value === 3 ? `${currentListenSourceLabel.value}` : undefined),
        sublabelClass: 'text-primary',
    },
].filter(category => category.visible)))

const breadcrumbs = computed(() => 
    [
        {
            label: "Rooms",
            disabled: roomWindowItem.value < 1 ? true : false,
            click: () => roomWindowItem.value = 0,
            validator: roomWindowItem.value > 0,
        },
        {
            label: roomLabel.value,
            click: () => roomWindowItem.value = 1,
            disabled: roomWindowItem.value != 1 ? false : true,
            validator: roomLabel.value && roomWindowItem.value > 0
        },
        {
            label: selectedCategory.value?.label,
            click: () => roomWindowItem.value = 2,
            validator: roomWindowItem.value >= 2
        },
        {
            label: selectedCategory.value?.id === 'watch' ? currentWatchSourceLabel.value : currentListenSourceLabel.value,
            click: () => roomWindowItem.value = 3,
            validator: roomWindowItem.value >= 3
        }
    ]
.filter(breadcrumb => breadcrumb.validator))


// EVENT BUS
onMounted(() => {
    // Listen for if source is selected
    emitter.on('sourceSelected', () => {
        roomWindowItem.value = 3
    })
})
onBeforeUnmount(() => {
    emitter.off('sourceSelected')
})

</script>

<style scoped>

</style>
