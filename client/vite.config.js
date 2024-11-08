import react from '@vitejs/plugin-react';
import {defineConfig} from 'vite';

export default defineConfig({
    base: '/',
    plugins: [react()],
    preview: {
        port: 5333,
        strictPort: true,
    },
    server: {
        port: 5333,
        strictPort: true,
        host: true,
        origin: 'http://0.0.0.0:8080',
    },
});
