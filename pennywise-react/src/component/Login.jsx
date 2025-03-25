import React from "react";
import { useState } from "react";
import { Link, useNavigate } from "react-router-dom";
import '../App.css'

const Login = () => {
    const [email, setEmail] = useState("");
    const [password, setPassword] = useState("");
    const [errorMessage, setErrorMessage] = useState("");
    const navigate = useNavigate();

    const handleLogin = async () => {
        const response = await fetch("http://localhost:5046/api/user/login", {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
            },
            body: JSON.stringify({ email, password }),
        });

        const data = await response.json();

        if (response.ok) {
            localStorage.setItem("user", JSON.stringify(data.user)); // Store user info
            navigate("/Overview")
        } else {
            setErrorMessage("Email or password is incrrect.");
        }
    }

    return (
        <div className="app" >
            <h2 className="header">Login</h2>
            <div>
                <p>
                    <input className="input" type="email" placeholder="Email" value={email} onChange={(e) => setEmail(e.target.value)} />
                </p>
                <p>
                    <input className="input" type="password" placeholder="Password" value={password} onChange={(e) => setPassword(e.target.value)} />
                </p>
                <p>
                    <button type="button" className="add-button" onClick={handleLogin} >Login</button>
                    {errorMessage && <p data-testid="error-message" className="error">{errorMessage}</p>}
                </p>
            </div>

            <p >
                <Link to="/forgot-password" className="text-blue-500">Forgot Password</Link>
            </p>
            <p>
                <Link to="/register" className="text-blue-500">Create Account</Link>
            </p>
        </div>
    );
};

export default Login;