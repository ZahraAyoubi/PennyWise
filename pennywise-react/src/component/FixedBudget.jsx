import { useState } from "react";
import FixedBudgetPopup from "./FixedBudgetPopup"
import PropTypes from "prop-types";

const FixedBudget = ({ onDelete, user, refreshTrigger, onRefresh }) => {
    const [showFixedbudget, setShowFixedbudget] = useState(false);

    return (

        <div className="card">
            <h2>Fixed budget</h2>
            <button className="add-button" onClick={() => setShowFixedbudget(!showFixedbudget)}>Fixed budget</button>
            {showFixedbudget && <FixedBudgetPopup setShowFixedbudget={setShowFixedbudget} user={user} onDelete={onDelete} refreshTrigger={refreshTrigger} onRefresh={onRefresh} />}
        </div>
    );
};

FixedBudget.propTypes = {

    onDelete: PropTypes.func.isRequired,
    user: PropTypes.object,
    refreshTrigger: PropTypes.number.isRequired
};

export default FixedBudget;