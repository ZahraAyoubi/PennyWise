import { useState } from "react";
import { useNavigate } from "react-router-dom";
import '../App.css'

function Register() {
    const navigate = useNavigate();
    const [email, setEmail] = useState("");
    const [password, setPassword] = useState("");
    const [name, setName] = useState("");
    const [passwordConfirmed, setPasswordConfirmed] = useState("");
    const [phone, setPhone] = useState("");
    const [error, setError] = useState("");
    const [emailError, setEmailError] = useState("");
    const [successMessage, setSuccessMessage] = useState("");
    const [errorMessage, setErrorMessage] = useState("");
    

    const handleRegister = async () => {
        const username = email;

        const response = await fetch(`http://localhost:5046/api/user/register/${encodeURIComponent(password)}`, {
                method: "POST",
                headers: {
                    "Content-Type": "application/json",
            },
            body: JSON.stringify({ email, name, phone, username }),
            });
            
        if (response.ok) {
            setEmail(""); 
            setPassword("");
            setPasswordConfirmed("");
            setName(""); 
            setPhone("");
            setSuccessMessage("Success!")
            setErrorMessage("");
        } else {
            setErrorMessage("Failed to register");
        }
    }

    const handleConfirmPasswordChange = (e) => {
        setPasswordConfirmed(e.target.value);
        if (e.target.value !== password) {
            setError("Passwords do not match");
        } else {
            setError("");
        }
    };

    const handleEmail = (e) => {
        const value = e.target.value;
        setEmail(value);
        const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
        if (!emailRegex.test(value)) {
            setEmailError("Invalid email format");
        } else {
            setEmailError("");
        }
    }

    const handleCancel = () => {
        navigate("/");
    };

    return (
        <div className="app" >
            <h2 className="header">Register</h2>
            <div>
                <p>
                    <input className="input" type="text" placeholder="Neme" value={name} onChange={(e) => setName(e.target.value)} />
                </p>
                <p>
                    <input className="input" type="email" placeholder="Email" value={email} onChange={handleEmail} />
                    {emailError && <p data-testid="email-error" className="error">{emailError}</p>}
                </p>
                <p>
                    <input className="input" type="text" placeholder="Phone" value={phone} onChange={(e) => setPhone(e.target.value)} />
                </p>
                <p>
                    <input className="input" type="password" placeholder="Password" value={password} onChange={(e) => setPassword(e.target.value)} />
                </p>
                <p>
                    <input className="input" type="password" placeholder="Confirm Password" value={passwordConfirmed} onChange={handleConfirmPasswordChange} />
                    {error && <p data-testid="error-message" className = "error">{error}</p>}
                </p>
                <p>
                    <button onClick={handleRegister} className="add-button" >Register</button>
                    {successMessage && <p data-testid="success-message" className="success">{successMessage}</p>}
                    {errorMessage && <p data-testid="error-message" className="error">{errorMessage}</p>}

                    <button className="btn-red" onClick={handleCancel} >Cancel</button>
                </p>
            </div>
        </div>
    );
};

export default Register;