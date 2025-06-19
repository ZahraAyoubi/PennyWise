import { useState } from 'react';
import { useNavigate } from "react-router-dom";
import '../App.css'

function ForgotPassword() {
    const [email, setEmail] = useState("");
    const [emailError, setEmailError] = useState("");
    const [error, setError] = useState("");
    const [errorMessage, setErrorMessage] = useState("");
    const navigate = useNavigate();

    const handleCancel = () => {
        navigate("/");
    };

    const handleEmail = (e) => {
        const value = e.target.value;
        setEmail(value.trim());
        const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
        if (!emailRegex.test(value)) {
            setEmailError("Invalid email format");
        } else {
            setEmailError("");
        }
    }



    const handleSubmit = async (e) => {
        e.preventDefault();

        // Recheck before sending
        if (emailError || error || !email) {
            setErrorMessage("Please fix the errors before submitting.");
            return;
        }
        try {
            const response = await fetch("http://localhost:5046/api/user/send-reset-link", {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({ email }),
            });

            if (response.ok) {
                alert('If the email exists, a reset link has been sent.');
            } else {
                const contentType = response.headers.get("content-type");
                const data = contentType && contentType.includes("application/json")
                    ? await response.json()
                    : await response.text();

                console.error("Error from backend:", data);
                setErrorMessage(data.message || "Something went wrong.");
            }
        } catch (err) {
            console.error("Error sending request:", err);
            setErrorMessage("Failed to send request. Please try again later.");
        }
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
                        <button type="submit" className="add-button" disabled={!!emailError || !!error} >Submit</button>
                        {errorMessage && <p data-testid="error-message" className="error">{errorMessage}</p>}
                        <button className="btn-red" onClick={handleCancel}>Cancel</button>
                    </p>
                </div>
            </div>
        </form>
    );
};

export default ForgotPassword;