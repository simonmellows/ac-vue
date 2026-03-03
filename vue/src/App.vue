<template>
  <div class="w-100 offline-indicator" v-if="offline"></div>
  <router-view v-if="!initializing"/>
  <div v-else class="h-100 w-100 d-flex justify-center">
    <div class="my-auto">
      <v-icon icon="mdi-loading" color="white" class="spinner mb-5" style="font-size: 100px;"></v-icon> <br>
      <span class="text-overline text-white" style="font-size: 18px !important;">Loading...</span>
    </div>
  </div>

  <v-snackbar
  v-model="snackbar"
  multi-line
  timeout="-1"
  rounded="xl"
  color="error"
  >
    <div class="text-subtitle-1">Offline with control system</div>

    <template v-slot:actions>
      <v-btn
        variant="text"
        @click="snackbar = false"
        icon="mdi-close"
      >
      </v-btn>
    </template>
  </v-snackbar>
</template>

<script setup >
import { computed, ref, watch, watchEffect } from 'vue'
import useInitialize from './composables/useInitialize'
import { useSystemFeedbackStore } from './store/useSystemFeedbackStore'

const systemFeedbackStore = useSystemFeedbackStore()

// Initialize the user interface
// This function will:  
//  - Register the Web Xpanel (if being used)
//  - Subscribe to all relevant signals for feedback from the control system
const { initialize, initializing } = useInitialize()
initialize()

// Reserved join triggered when panel is offline with control system
const offline = computed(() => systemFeedbackStore.getPropertyValue("Csig.Control_Systems_Offline_fb", 'digital'))

// Display/hide snackbar message
const snackbar = ref(false)

watchEffect(() => {
  if(offline.value){
    snackbar.value = true
  }
  else {
    snackbar.value = false
  }
})

</script>

<style>
#app {
  font-family: Avenir, Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  text-align: center;
  color: #2c3e50;
}

.offline-indicator{
  position: absolute;
  height: 3px;
  background-color: rgb(var(--v-theme-error));
}

nav {
  padding: 30px;
}

nav a {
  font-weight: bold;
  color: #2c3e50;
}

nav a.router-link-exact-active {
  color: #42b983;
}
</style>
