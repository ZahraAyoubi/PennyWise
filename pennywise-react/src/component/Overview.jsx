import { useState, useEffect } from "react";
import { useNavigate } from "react-router-dom";
import Header from "./Header";
import Dashboard from "./Dashboard";
import '../TotalExpense.css';
import '../Overview.css'

const Overview = () => {
    const [income, setIncome] = useState(0);
    const [date, setDate] = useState(new Date().toISOString().split("T")[0]);
    const [showProfile, setShowProfile] = useState(false);
    const [user, setUser] = useState(() => {
        const storedUser = localStorage.getItem("user");
        return storedUser ? JSON.parse(storedUser) : {}; // Default to empty object if null
    });
    const [profile, setProfile] = useState(null);
    const navigate = useNavigate();
    const userId = user.id;

    const fetchData = async () => {
        if (userId == null) {
            console.warn("User not found, skipping data fetch.");
            return;
        }
        try {
            const incomeResponse = await fetch(`http://localhost:5259/api/transactions/Income/${date}/${userId}`, { method: "GET", });          

            setIncome(await incomeResponse.json());

        } catch (error) {
            console.error("Error fetching data:", error);
        }
    };

    const handleDeleteRotatingItem = async (itemId) => {
        try {
            const response = await fetch(`http://localhost:5259/api/transactions/DeleteRotatingBudget/${itemId}`, { method: "DELETE" });
            if (!response.ok) throw new Error(`Error deleting item: ${response.status}`);
            fetchData();
        } catch (error) {
            console.error("Error deleting item:", error);
        }
    };

    const getProfile = async () => {
        const storedUser = JSON.parse(localStorage.getItem("user"));
        if (storedUser) {
            const response = await fetch(`http://localhost:5046/api/profile/${storedUser.email}`);
            const data = await response.json();
            setProfile(data.profile.name || "N/A");
            localStorage.setItem("profile", JSON.stringify(data.profile)); 
        }
    };

    useEffect(() => {
        const storedUser = localStorage.getItem("user");

        if (storedUser) {
            try {
                const parsedUser = JSON.parse(storedUser);
                setUser(parsedUser); 
                getProfile();

            } catch (error) {
                console.error("Error parsing stored user:", error);
                setUser(null);
            }
            fetchData();
        }
    }, []);

    const handleLogout = async () => {
        try {
            const response = await fetch("http://localhost:5046/api/user/logout", { method: "POST", headers: { "Content-Type": "application/json" } });
            if (response.ok) {
                localStorage.removeItem("user");
                navigate("/");
            }
        } catch (error) {
            console.error("Logout failed", error);
        }
    };

    return (
        <div className="app">
            <Header onLogout={handleLogout} user={user} showProfile={showProfile} setShowProfile={setShowProfile} profile={profile} />
            <Dashboard
                income={income}
                date={date}
                setDate={setDate}
                onDelete={handleDeleteRotatingItem}
                fetchData={fetchData}
                user={user}
            />
        </div>
    );
};

export default Overview;