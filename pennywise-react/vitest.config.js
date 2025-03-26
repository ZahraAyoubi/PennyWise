import { defineConfig } from "vitest/config";

export default defineConfig({
    test: {
        globals: true,  
        environment: "jsdom",  
        setupFiles: "./setupTests.js", 
        coverage: {
            provider: "v8",  
            reporter: ["text", "lcov"]
        },
        threads: false,  
    },
});