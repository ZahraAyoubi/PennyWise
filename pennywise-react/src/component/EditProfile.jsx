import { useState, useEffect } from "react";
import { useNavigate } from "react-router-dom";
import '../EditProfile.css';

const EditProfile = () => {
    const navigate = useNavigate();
    const [user, setUser] = useState(() => {
        const storedUser = localStorage.getItem("user");
        return storedUser ? JSON.parse(storedUser) : {};
    });

    const [profile, setProfile] = useState(() => {
        const storedProfile = localStorage.getItem("profile");
        return storedProfile ? JSON.parse(storedProfile) : {}; 
    });

    const [description, setDescription] = useState(profile.description);
    const [profileName, setProfileName] = useState(profile.name);

    useEffect(() => {
        const storedUser = localStorage.getItem("user");

        if (storedUser) {
            try {
                const parsedUser = JSON.parse(storedUser);
                setUser(parsedUser); 
                console.log(user)
            } catch (error) {
                console.error("Error parsing stored user:", error);
                setUser(null);
            }
        }

        const storedProfile = localStorage.getItem("profile");
        if (storedProfile) {
            try {
                const parsedProfile = JSON.parse(storedProfile);
                //parsedProfile.description = description;
                setProfile(parsedProfile);
                setDescription(parsedProfile.description);
                setProfileName(parsedProfile.name);
                console.log(profile)
            } catch (error) {
                console.error("Error parsing stored profile:", error);
                setProfile(null);
            }
        }
    }, []);

    useEffect(() => {
        setProfile((prevProfile) => ({
            ...prevProfile,
            description: description,
            name: profileName
        }));
    }, [description, profileName]);

    // Handle cancel button click
    const handleCancel = () => {
        navigate("/Overview");
    };


    const handleEdit = async () => {     
        try {
            const response = await fetch("http://localhost:5046/api/profile", {
                method: "PUT",
                headers: {
                    "Content-Type": "application/json",
                },
                body: JSON.stringify( profile ),
            });
            if (response.ok) {
                // Successfully updated, navigate to Overview page
                navigate("/Overview");
            } else {
                console.error("Failed to update profile");
            }
        } catch (error) {
            console.error("Logout failed", error);
        }
    }

    return (
        <div className="app">
            <div className="card">
                <div className="input-group">
                    <label className="label">Profile:</label>
                    <input type="text" className="input"
                        value={profileName}
                    onChange={(e) => setProfileName(e.target.value)}                    />
                </div>

                <div className="input-group">
                    <label className="label">Email:</label>
                    <input type="text" className="input" value={user.email} />
                </div>

                <div className="input-group">
                    <label className="label">Description:</label>
                    <input type="text" className="input"
                        value={description}
                        onChange={(e) => setDescription(e.target.value)} />
                </div>
                <div className="btn">
                    <button className="btn-red" onClick={handleCancel}>Cancel</button>
                    <button className="btn-green" onClick={handleEdit}>Update</button>
                </div>
            </div>
        </div>
    );
}

export default EditProfile;