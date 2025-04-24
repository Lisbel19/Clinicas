import { defineConfig } from 'vite';
import vue from '@vitejs/plugin-vue';
import path from 'path';

export default defineConfig({
  plugins: [vue()],
  resolve: {
    alias: {
      '@': path.resolve(__dirname, './src'),
      // Agregar alias para Bootstrap si es necesario
      'bootstrap': path.resolve(__dirname, 'node_modules/bootstrap')
    }
  },
  css: {
    preprocessorOptions: {
      scss: {
        additionalData: `
          @import "bootstrap/scss/functions";
          @import "bootstrap/scss/variables";
          @import "bootstrap/scss/mixins";
          @import "bootstrap/scss/utilities";
          @import "bootstrap/scss/root";
          @import "bootstrap/scss/reboot";
          @import "bootstrap/scss/type";
          @import "bootstrap/scss/buttons";
          @import "bootstrap/scss/modal";
          // Importa solo los componentes que necesites
        `
      }
    }
  },
  optimizeDeps: {
    include: [
      'bootstrap/dist/js/bootstrap.bundle.min.js'
    ]
  }
})