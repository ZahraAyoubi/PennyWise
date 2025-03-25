import { Link } from "react-router-dom";
import ProfileDropdown from "./ProfileDropdown";
import PropTypes from "prop-types";

const Header = ({ onLogout, user, showProfile, setShowProfile, profile }) => {
    return (
        <div className="header-container">
            <h1 className="header">PennyWise</h1>
            <div className="user-actions">
                <Link className="logout-button" onClick={onLogout}>Logout</Link>
                <div className="profile-settings">
                    <button className="profile-button" onClick={() => setShowProfile(!showProfile)}>
                        ⚙️ Settings
                    </button>
                    {showProfile && user && <ProfileDropdown user={user} profile={profile} setShowProfile={setShowProfile} />}
                </div>
            </div>
        </div>
    );
};

Header.propTypes = {
    onLogout: PropTypes.func.isRequired,
    user: PropTypes.object,
    showProfile: PropTypes.bool.isRequired,
    setShowProfile: PropTypes.func.isRequired,
    profile: PropTypes.string
};


export default Header;