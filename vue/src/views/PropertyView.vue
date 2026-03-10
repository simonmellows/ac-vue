<template>
    <v-container fluid class="h-100 pb-2 px-2 d-flex flex-column">
        <div class="d-flex py-2" v-if="propertyWindowItem > 0">
            <v-btn
            :showBack="propertyWindowItem > 0"
            icon="mdi-chevron-left" variant="text"
            @click="propertyWindowItem--"
            density="compact">
            </v-btn>
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
        </div>
        <Dashboard
        :title="propertyName"
        :showBack="false"
        :showTitle="propertyWindowItem == 0"
        :showSubtitle="propertyWindowItem == 0"
        :subtitle="propertyWindowItem < 1 ? 'Dashboard' : selectedCategory?.label"
        @back="propertyWindowItem--"
        >
        <template #body>
            <v-window v-model="propertyWindowItem" class="h-100">
            <!-- Property Dashboard -->
            <v-window-item class="h-100"
            v-on:touchstart.stop 
            v-on:touchmove.stop
            v-on:touchend.stop>
                <v-container fluid class="h-75 py-0 overflow-y-auto no-scrollbar" v-if="categories.length">
                    <v-row class="h-100 overflow-y-auto no-scrollbar">
                        <v-col v-for="category in categories" :key="category.id" :cols="12" sm="6" md="4" class="h-50 w-100">
                            <DashboardCard :label="category.label" :icon="category.icon"
                            @click="[propertyWindowItem++, selectedCategory = category]">
                            </DashboardCard>
                        </v-col>
                    </v-row>
                </v-container>
                <v-container fluid class="h-25 py-0" v-if="quickActions.length">
                    <v-row class="h-100 overflow-x-scroll flex-nowrap no-scrollbar">
                        <v-col :cols="12" sm="6" md="4" class="h-100" v-for="quickAction in quickActions" :key="quickAction">
                            <DashboardCard :label="quickAction.label" :icon="quickAction.icon || 'mdi-lightning-bolt'" :iconColor="quickAction.color">
                                <template #append v-if="quickAction?.inProgress">
                                    <v-progress-circular indeterminate size="small" color="primary"></v-progress-circular>
                                </template>
                                <template #text>
                                    <div class="text-subtitle-1 text-truncate text-grey">{{ quickAction.description }}</div>
                                </template>
                            </DashboardCard>
                        </v-col>
                    </v-row>
                </v-container>
            </v-window-item>
            <!-- Category Dashboard -->
            <v-window-item class="h-100"
            v-on:touchstart.stop 
            v-on:touchmove.stop
            v-on:touchend.stop>
                <component 
                :is="propertyComponents[selectedCategory.id]"
                :type="selectedCategory?.id"
                />
            </v-window-item>
        </v-window>
        </template>
        </Dashboard>
    </v-container>
</template>

<script setup>
import { computed, ref, watch } from 'vue';
import useProperty from '@/composables/property/useProperty';
import categoryData from '../data/categories'
import { useInterfaceDataStore } from '../store/useInterfaceDataStore'
import vuetify from '../plugins/vuetify'
import Dashboard from '@/components/general/Dashboard.vue';
import DashboardCard from '@/components/general/DashboardCard.vue';
import propertyComponents from '../components/property'

const interfaceDataStore = useInterfaceDataStore()
const { xs } = vuetify.display

const propertyWindowItem = ref(interfaceDataStore.data?.propertyWindowItem)
const selectedCategory = ref(interfaceDataStore.data?.propertySelectedCategory)

const { 
    name: propertyName, 
    hasLighting, 
    hasShading, 
    hasClimate, 
    hasEntertainment, 
    hasSecurity, 
    hasFloorplan, 
    hasSystem, 
    quickActions,
} = useProperty()

watch(propertyWindowItem, (newValue) => {
    interfaceDataStore.data.propertyWindowItem = newValue
})
watch(selectedCategory, (newValue) => {
    interfaceDataStore.data.propertySelectedCategory = newValue
})

const categories = computed(() => [
    {
        id: categoryData?.lighting?.id,
        label: categoryData?.lighting?.label,
        icon: categoryData?.lighting?.icon,
        visible: hasLighting.value
    },
    {
        id: categoryData?.shading?.id,
        label: categoryData?.shading?.label,
        icon: categoryData?.shading?.icon,
        visible: hasShading.value
    },
    {
        id: categoryData?.climate?.id,
        label: categoryData?.climate?.label,
        icon: categoryData?.climate?.icon,
        visible: hasClimate.value
    },
    {
        id: categoryData?.entertainment?.id,
        label: categoryData?.entertainment?.label,
        icon: categoryData?.entertainment?.icon,
        visible: hasEntertainment.value
    },
    {
        id: categoryData?.security?.id,
        label: categoryData?.security?.label,
        icon: categoryData?.security?.icon,
        visible: hasSecurity.value
    },
    {
        id: categoryData?.floorplan?.id,
        label: categoryData?.floorplan?.label,
        icon: categoryData?.floorplan?.icon,
        visible: hasFloorplan.value
    },
    {
        id: categoryData?.system?.id,
        label: categoryData?.system?.label,
        icon: categoryData?.system?.icon,
        visible: hasSystem.value
    },
].filter(category => category.visible))

const breadcrumbs = computed(() => 
    [
        {
            label: "Property",
            disabled: propertyWindowItem.value < 1 ? true : false,
            click: () => propertyWindowItem.value = 0,
            validator: propertyWindowItem.value > 0,
        },
        {
            label: selectedCategory.value?.label,
            click: () => propertyWindowItem.value = 1,
            validator: propertyWindowItem.value >= 1
        }
    ]
.filter(breadcrumb => breadcrumb.validator))

</script>

<style>
.v-breadcrumbs-item span {
    font-size: 1rem !important;
    line-height: unset !important;
}
</style>


