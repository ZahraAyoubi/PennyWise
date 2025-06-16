import { useState } from 'react';
import '../App.css'

function ForgotPassword() {
    const [email, setEmail] = useState("");
    const [emailError, setEmailError] = useState("");
    const [emailConfirmed, setEmailConfirmed] = useState("");
    const [error, setError] = useState("");
    const [errorMessage, setErrorMessage] = useState("");

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

        const handleConfirmEmailChange = (e) => {
            setEmailConfirmed(e.target.value);
            if (e.target.value !== email) {
                setError("Emails do not match");
            } else {
                setError("");
            }
    };

    const handleSubmit = async (e) => {
        e.preventDefault();
        await fetch("http://localhost:5046/api/user/send-reset-link", {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ email }),
        });
        alert('If the email exists, a reset link has been sent.');
    };

    return (
        <form onSubmit={handleSubmit}>
        <div className="app" >
            <h2 className="header">Forgot Password</h2>
            <div>
                <p>
                    <input className="input" type="email" placeholder="Email" value={email} onChange={handleEmail} />
                    {emailError && <p data-testid="email-error" className="error">{emailError}</p>}
                </p>
                <p>
                    <input className="input" type="email" placeholder="Confirm Email" value={emailConfirmed} onChange={handleConfirmEmailChange} />
                    {error && <p data-testid="error-message" className="error">{error}</p>}
                </p>
                    <p>
                        <button type="submit" className="add-button"  >Submit</button>
                    {errorMessage && <p data-testid="error-message" className="error">{errorMessage}</p>}
                </p>
            </div>
            </div>
        </form>
    );
};

export default ForgotPassword;