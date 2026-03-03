<template>
    <v-bottom-navigation 
    id="bottomNav" 
    grow 
    style="position: absolute !important;" 
    :elevation="0"
    class="mb-responsive"
    >
        
        <v-btn 
        v-for="item in items" 
        :key="item.id"
        :value="item.id"
        class="bottom-nav-btn" 
        @click="router.push(item?.to)"
        :active="item?.active"
        >
            <v-icon :icon="item?.icon"></v-icon>
            <span>{{item?.label}}</span>
        </v-btn>

    </v-bottom-navigation>
</template>

<script setup>
// border-top: 1px solid rgb(128, 128, 128, 0.12);
import { computed } from 'vue';
import { useRouter } from 'vue-router';

const router = useRouter()
const items = computed(() => [
    {
        id: 'property',
        to: '/',
        label: 'Property',
        icon: 'mdi-home',
        active: !!(router.currentRoute?.value.fullPath === '/')
    },
    {
        id: 'room',
        to: '/room',
        label: 'Rooms',
        icon: 'mdi-menu',
        active: !!(router.currentRoute?.value.fullPath === '/room')
    }
])

</script>

<style scoped>

.v-btn--active {
  opacity: 1 !important;
}
.v-btn{
  opacity: 0.3;
}
</style>