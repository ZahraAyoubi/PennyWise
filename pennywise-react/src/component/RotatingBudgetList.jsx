import { useState, useEffect } from 'react';
import PropTypes from "prop-types";
import RotatingBudget from "./RotatingBudget";

const RotatingBudgetList = ({ onDelete, date, user }) => {
    const [value, setValue] = useState([]); // State for the textbox value
    const userId = user ? user.id : null;
    if (!userId) {
        console.error("User is null ");
    }

    useEffect(() => {
        const fetchTotalExpens = async () => {
            try {
                const response = await fetch(`http://localhost:5259/api/transactions/RotatingBudget/${date}/${userId}`, { method: "Get" }); // API call to fetch the current value

                const data = await response.json();
                setValue(data);
            } catch (error) {
                console.error('Error fetching rotating budget:', error);
            }
        };

        fetchTotalExpens();
    }, [date]); 
    return (
        <div className="card rotating-budget">
            <h2>Rotating Budget</h2>
            <RotatingBudget rotatingBudget={value} onDelete={onDelete} />
        </div>
    );
};

RotatingBudgetList.propTypes = {
    onDelete: PropTypes.func.isRequired,
    date: PropTypes.string.isRequired,
    user: PropTypes.object
};

export default RotatingBudgetList;