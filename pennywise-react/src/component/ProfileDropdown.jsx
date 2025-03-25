import PropTypes from "prop-types";

const ProfileDropdown = ({ user, profile, setShowProfile }) => {
    return (
        <div className="profile-dropdown">
            <h3>Profile: {profile}</h3>
            <p><strong>Email:</strong> {user.email}</p>
            <p><strong>Description:</strong> {user.description}</p>
            <p><strong>Role:</strong> {user.role}</p>
            <button onClick={() => setShowProfile(false)}>Close</button>
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