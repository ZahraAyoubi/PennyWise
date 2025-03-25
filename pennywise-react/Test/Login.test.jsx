import React from "react";
import { render, screen, fireEvent } from "@testing-library/react";
import { MemoryRouter } from "react-router-dom";
import { expect, test, vi } from 'vitest';
import Login from "../src/component/Login"; 
import { describe } from "../node_modules/vitest/dist/index";

vi.mock("react-router-dom", async () => {
    const actual = await vi.importActual("react-router-dom");
    return {
        ...actual,
        useNavigate: vi.fn(), // Mock only `useNavigate`
    };
});
describe("Login Component", () => {
    //test1
    test("Should render the login form correctly", () => {
        //Arrange
        render(
            <MemoryRouter>
                <Login />
            </MemoryRouter>
        );

        //Assert
        expect(screen.getByRole("heading", { level: 2 })).toHaveTextContent("Login");
        expect(screen.getByPlaceholderText("Email")).toBeInTheDocument();
        expect(screen.getByPlaceholderText("Password")).toBeInTheDocument();
        expect(screen.getByRole("button", { name: /login/i })).toBeInTheDocument();
    });

    //test2
    test("Should have input fields for email and password", () => {
        //Arrange 
        render(
            <MemoryRouter>
                <Login />
            </MemoryRouter>
        );
        const passwordInput = screen.getByPlaceholderText("Password");
        const emailInput = screen.getByPlaceholderText("Email");

        //Assert
        expect(emailInput).toBeInTheDocument();
        expect(emailInput).toHaveAttribute("type", "email");
        expect(passwordInput).toBeInTheDocument();
        expect(passwordInput).toHaveAttribute("type", "password");
    });

    //test3
    test("Should have a login button", () => {
        //Arrange 
        render(
            <MemoryRouter>
                <Login />
            </MemoryRouter>
        );
        //screen.debug();
        const loginButton = screen.getByRole("button", { name: /login/i });

        //Assert
        expect(loginButton).toBeInTheDocument();
        expect(loginButton).toHaveAttribute("type", "button");
        
    });

    //test4
    test("Should allow the user to type an email and a password", () => {
        //Arrange 
        render(
            <MemoryRouter>
                <Login />
            </MemoryRouter>
        );
        const emailInput = screen.getByPlaceholderText("Email");
        const passwordInput = screen.getByPlaceholderText("Password");

        //Act
        fireEvent.change(emailInput, { target: { value: "test@example.com" } });
        fireEvent.change(passwordInput, { target: { value: "password123" } });

        //Assert
        expect(emailInput.value).toBe("test@example.com");
        expect(passwordInput.value).toBe("password123");


    });

    //test5
    test("Should call API with correct credentials when login button is clicked", async () => {
        window.fetch = vi.fn(() =>
            Promise.resolve({
                json: () => Promise.resolve({ token: "fake-jwt-token" }),
            })
        );

        // Arrange
        render(
            <MemoryRouter>
                <Login />
            </MemoryRouter>
        );

        const emailInput = screen.getByPlaceholderText("Email");
        const passwordInput = screen.getByPlaceholderText("Password");
        const loginButton = screen.getByRole("button", { name: /login/i });

        // Act
        fireEvent.change(emailInput, { target: { value: "test@example.com" } });
        fireEvent.change(passwordInput, { target: { value: "password123" } });
        fireEvent.click(loginButton);

        // Assert
        expect(window.fetch).toHaveBeenCalledWith(
            "http://localhost:5046/api/user/login",
            expect.objectContaining({
                method: "POST",
                headers: expect.objectContaining({ "Content-Type": "application/json" }),
                body: JSON.stringify({ email: "test@example.com", password: "password123" }),
            })
        );

        // Clean up mock
        window.fetch.mockRestore();
    });
});
