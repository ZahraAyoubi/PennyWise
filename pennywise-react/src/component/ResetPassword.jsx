import { useState } from 'react';
import { useNavigate, useSearchParams } from "react-router-dom";
import '../App.css'

function ResetPassword() {
    const [searchParams] = useSearchParams();
    const resetCode = decodeURIComponent(searchParams.get("token"));
    const email = searchParams.get("email");

    const [password, setPassword] = useState("");
    const [confirmPassword, setConfirmPassword] = useState("");
    const [error, setError] = useState("");
    const [success, setSuccess] = useState("");

    const navigate = useNavigate();

    const handleCancel = () => {
        navigate("/");
    };

    const handleSubmit = async (e) => {
        e.preventDefault();

        if (password !== confirmPassword) {
            setError("Passwords do not match");
            return;
        }

        const response = await fetch("http://localhost:5046/api/user/resetpassword", {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify({ email, resetCode, newPassword: password }),
        });

        if (response.ok) {
            setSuccess("Password reset successfully.");
        } else {
            setError("Something went wrong.");
        }
    };

    return (
        <form onSubmit={handleSubmit}>
            <div className="app" >
                <h2 className="header">Reset your password</h2>
                <div>
                    <p>
                        <input className="input" type="password" placeholder="Password" value={password} onChange={(e) => setPassword(e.target.value)} />
                        
                    </p>
                    <p>
                        <input className="input" type="password" placeholder="Confirm Password" value={confirmPassword} onChange={(e) => setConfirmPassword(e.target.value)} />
                       
                    </p>
                    <p>
                        <button type="submit" className="add-button" >Submit</button>
                        <button className="btn-red" onClick={handleCancel}>Cancel</button>
                    </p>
                </div>
            </div>
        </form>
    );
};

export default ResetPassword;