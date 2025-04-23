import { useState, useEffect } from 'react';
import '../TotalExpense.css';
import PropTypes from 'prop-types';

const RemainingBudget = ({ refreshTrigger, date, user, onDelete }) => {
    const [value, setValue] = useState(0); 
    const userId = user ? user.id : null;
    if (!userId) {
        console.error("User is null ");
    }

    useEffect(() => {
        const fetchCurrentIncome = async () => {
            try {
                const response = await fetch(`http://localhost:5259/api/transactions/RamainingBudget/${date}/${userId}`, { method: "GET", }); // API call to fetch the current value
                const data = await response.json();
                setValue(data);  
            } catch (error) {
                console.error('Error fetching remaining budget:', error);
            }
        };

        fetchCurrentIncome();
    }, [date, refreshTrigger, onDelete]); 

    return (
        <div className="value">
            <h2> {value}</h2>
        </div>
    );
};

RemainingBudget.propTypes = {
    refreshTrigger: PropTypes.number.isRequired,
    onDelete: PropTypes.func.isRequired,
    date: PropTypes.string.isRequired,
    user: PropTypes.object
};

export default RemainingBudget;
