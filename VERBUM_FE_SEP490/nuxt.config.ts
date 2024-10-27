// https://nuxt.com/docs/api/configuration/nuxt-config
import { nodePolyfills } from 'vite-plugin-node-polyfills'
export default defineNuxtConfig({
  compatibilityDate: '2024-04-03',
  devtools: { enabled: true },
  ssr: false,
  vite: {
    optimizeDeps: {
      exclude: ['vee-validate']
    },
    plugins: [nodePolyfills()]
  },
  modules: [
    [
      '@pinia/nuxt',
      {
        autoImports: ['defineStore', 'acceptHMRUpdate']
      }
    ],
    'pinia-plugin-persistedstate/nuxt',
    '@nuxtjs/tailwindcss',
    'shadcn-nuxt',
    'nuxt-lucide-icons',
    '@nuxtjs/color-mode',
    '@nuxt/test-utils/module',
    '@nuxt/eslint',
    '@nuxt/image',
    '@vee-validate/nuxt',
    'nuxt-vuefire'
  ],
  shadcn: {
    prefix: '',
    /**
     * Directory that the component lives in.
     * @default "./components/ui"
     */
    componentDir: './components/ui'
  },
  colorMode: {
    classSuffix: ''
  },
  vuefire: {
    config: {
      apiKey: 'AIzaSyBZZD9tz07To3mz4jiFIxFgPMHMf2-JaSU',
      authDomain: 'verbum-sep490.firebaseapp.com',
      projectId: 'verbum-sep490',
      storageBucket: 'verbum-sep490.appspot.com',
      messagingSenderId: '422241101997',
      appId: '1:422241101997:web:0c2fd1591a1467266dbe24',
      measurementId: 'G-VNKR0RS1MR'
    }
  }
})