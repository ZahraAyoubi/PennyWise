import { BrowserRouter as Router, Routes, Route } from "react-router-dom";
import Login from "./component/Login";
import Register from "./component/Register";
import Overview from "./component/Overview";

function App() {
    return (

        <Router>
            <Routes>
                <Route path="/" element={<Login />} />
                <Route path="/register" element={<Register />} />
                <Route path="/overview" element={<Overview />} />
            </Routes>
        </Router>
    );
}

export default App;
