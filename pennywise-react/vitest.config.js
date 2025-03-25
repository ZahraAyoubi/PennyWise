//import { defineConfig } from 'vitest/config';

//export default defineConfig({
//    test: {
//        globals: true,  // Enables global `describe`, `test`, `expect`
//        environment: "jsdom",  // Ensures React components can be tested
//        setupFiles: "./setupTests.js", // ✅ Optional: Centralize setup
//    },
//});
import { defineConfig } from "vitest/config";

export default defineConfig({
    test: {
        globals: true,  // ✅ Enables global `describe`, `test`, `expect`
        environment: "jsdom",  // ✅ Ensures React components can be tested
        setupFiles: "./setupTests.js", // ✅ Centralize setup
        coverage: {
            provider: "v8",  // Optional: Enables coverage reporting
            reporter: ["text", "lcov"]
        },
        threads: false,  // Optional: Disable for better debugging
    },
});