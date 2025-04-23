import React from "react";
import { render, screen, fireEvent, waitFor } from "@testing-library/react";
import { expect, test, vi, describe } from "vitest";
import AddTransaction from "../src/component/AddTransaction";

describe("AddTransaction Component", () => {
    test("renders form and submits data correctly", async () => {
        // Arrange
        const mockOnRefresh = vi.fn();

        const mockFetch = vi.fn(() =>
            Promise.resolve({
                ok: true,
                json: () => Promise.resolve({}),
            })
        );
        global.fetch = mockFetch;

        render(<AddTransaction user={{ id: 1 }} onRefresh={mockOnRefresh} />);

        // Act
        fireEvent.click(screen.getByRole("button", { name: /Add Transaction/i }));

        fireEvent.change(screen.getByLabelText(/Description:/i), {
            target: { value: "Groceries" },
        });
        fireEvent.change(screen.getByLabelText(/Amount:/i), {
            target: { value: "250" },
        });
        fireEvent.click(screen.getByLabelText(/It is a fixed budget/i));

        fireEvent.click(screen.getByRole("button", { name: /^Add$/i }));

        // Assert
        await waitFor(() => {
            expect(mockFetch).toHaveBeenCalledTimes(1);
            expect(mockOnRefresh).toHaveBeenCalledTimes(1);
        });
        expect(screen.queryByLabelText(/Description:/i)).not.toBeInTheDocument();

        global.fetch.mockRestore();
    });
});
