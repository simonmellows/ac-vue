import '@mdi/font/css/materialdesignicons.css'
import 'vuetify/styles'
import { createVuetify } from 'vuetify'
import * as components from 'vuetify/components'
import * as directives from 'vuetify/directives'

import { mdi } from "vuetify/iconsets/mdi";

// https://vuetifyjs.com/en/introduction/why-vuetify/#feature-guides

export default createVuetify({
  components,
  directives,
  theme: {
    defaultTheme: 'dark',
    themes: {
      light: {
        colors: {
          //surface: "#F5F5F5",
          primary: '#1867C0',
          secondary: '#5CBBF6',
        },
      },
    },
  },
  icons: {
    defaultSet: 'mdi',
    sets: {
      mdi
    }
  }
})
