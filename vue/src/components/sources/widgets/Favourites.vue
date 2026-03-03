<template>
    <v-menu v-model="favouriteList">
        <template v-slot:activator="{ props }">
            <v-btn
            max-width="250"
            class="mx-auto my-auto"
            prepend-icon="mdi-heart"
            :append-icon="favouriteList ? 'mdi-chevron-up' : 'mdi-chevron-down'"
            v-bind="props"
            variant="tonal"
            >
            Favourites
            </v-btn>
        </template>
        <v-card variant="flat" class="border-thin">
            <v-list>
                <v-list-item
                v-for="(item, index) in 3"
                :key="index"
                :value="index"
                prepend-avatar="https://ichef.bbci.co.uk/images/ic/1920x1080/p05d68tx.jpg"
                >
                <v-list-item-title>Fav {{ index }}</v-list-item-title>
                </v-list-item>
            </v-list>
        </v-card>
    </v-menu>
</template>

<script setup>
import { ref } from 'vue';

const favouriteList = ref(false)

</script>

<style scoped>

</style>