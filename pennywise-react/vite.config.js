//import { defineConfig } from 'vite';

//import react from "@vitejs/plugin-react";

//// https://vitejs.dev/config/
//export default defineConfig({
//    plugins: [react()],
//    server: {
//        port: 49222,
//    },
    //test: {
    //    environment: "jsdom", // ✅ Ensures Vitest works with React components
    //    globals: true, // ✅ Allows using `test` & `expect` globally
    //    setupFiles: "./setupTests.js", // ✅ Optional: Centralize setup
    //}
//})

import { defineConfig } from "vite";
import react from "@vitejs/plugin-react";

export default defineConfig({
    plugins: [react()],
        server: {
            port: 49222,
        },
    test: {
        globals: true,
        environment: "jsdom",
        setupFiles: "./setupTests.js",
        deps: {
            inline: ["react"], // ✅ Ensures React is included in test environment
        },
    },
});




