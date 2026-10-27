import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'

// https://vite.dev/config/
export default defineConfig({
  plugins: [react()],
  server: {
    proxy: {
      '/api': {
        target: 'https://localhost:7078',
        changeOrigin: true,
        secure: false,
      },
      '/register': {
        target: 'https://localhost:7078',
        changeOrigin: true,
        secure: false,
      },
      '/login': {
        target: 'https://localhost:7078',
        changeOrigin: true,
        secure: false,
      },
    },
  },
})
