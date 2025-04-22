import PropTypes from "prop-types";
import { useNavigate } from "react-router-dom";

const ProfileDropdown = ({ user, profile, setShowProfile }) => {
    const navigate = useNavigate();

    const handleEdit = () => {
        navigate("/EditProfile");
    }

    return (
        <div className="profile-dropdown">
            <h3>Profile: {profile}</h3>
            <p><strong>Email:</strong> {user.email}</p>
            <p><strong>Description:</strong> {user.description}</p>
            <p><strong>Role:</strong> {user.role}</p>
            <div>
                <button className="cancel-button" onClick={() => setShowProfile(false)}>Close</button>
                <button className="add-transaction-button" onClick={handleEdit}>Edit</button>
            </div>
        </div>
    );
};

ProfileDropdown.propTypes = {
    user: PropTypes.shape({
        email: PropTypes.string,
        description: PropTypes.string,
        role: PropTypes.string
    }),
    profile: PropTypes.string.isRequired,
    setShowProfile: PropTypes.func.isRequired
};

export default ProfileDropdown;