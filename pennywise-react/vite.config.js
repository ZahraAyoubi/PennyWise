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
            inline: ["react"], 
        },
    },
});




