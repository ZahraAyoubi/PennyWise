import { useState } from "react";
import FixedBudgetPopup from "./FixedBudgetPopup"
import PropTypes from "prop-types";

const FixedBudget = ({ onTransactionAdded, onDelete , user }) => {
    const [showFixedbudget, setShowFixedbudget] = useState(false);
    return (

        <div className="card">
            <h2>Fixed budget</h2>
            <button className="add-button" onClick={() => setShowFixedbudget(!showFixedbudget)}>Fixed budget</button>
            {showFixedbudget && <FixedBudgetPopup onTransactionAdded={onTransactionAdded} setShowFixedbudget={setShowFixedbudget}  user={user} onDelete={onDelete} />}
        </div>
    );
};

FixedBudget.propTypes = {
    onTransactionAdded: PropTypes.func.isRequired,
    onDelete: PropTypes.func.isRequired,
    user: PropTypes.object
//    setDate: PropTypes.func.isRequired
};

export default FixedBudget;