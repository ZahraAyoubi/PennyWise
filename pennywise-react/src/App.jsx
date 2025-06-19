import { BrowserRouter as Router, Routes, Route } from "react-router-dom";
import Login from "./component/Login";
import Register from "./component/Register";
import Overview from "./component/Overview";
import EditProfile from "./component/EditProfile";
import ForgotPassword from "./component/ForgotPassword";
import ResetPassword from "./component/ResetPassword";

function App() {
    return (

        <Router>
            <Routes>
                <Route path="/" element={<Login />} />
                <Route path="/register" element={<Register />} />
                <Route path="/overview" element={<Overview />} />
                <Route path="/editprofile" element={<EditProfile />} />
                <Route path="/forgotpassword" element={<ForgotPassword />} />
                <Route path="/resetpassword" element={<ResetPassword /> } />
            </Routes>
        </Router>
    );
}

export default App;
