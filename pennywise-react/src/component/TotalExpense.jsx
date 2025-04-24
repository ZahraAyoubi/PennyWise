import { useState, useEffect } from 'react';
import '../TotalExpense.css';
import PropTypes from 'prop-types';

const TotalExpense = ({  date, user, refreshTrigger }) => {
    const [value, setValue] = useState(0); 
    const userId = user ? user.id : null;
    if (!userId) {
        console.error("User is null ");
    }
    
    useEffect(() => {
        const fetchTotalExpens = async () => {
            try {
                const response = await fetch(`http://localhost:5259/api/transactions/TotalExpenses/${date}/${userId}`, { method: "Get" }); // API call to fetch the current value
                
                const expensesData = await response.json();
                setValue(expensesData); 
            } catch (error) {
                console.error('Error fetching total expenses:', error);
            }
        };

        fetchTotalExpens();
    }, [refreshTrigger, date, userId]); 

    return (
            <div className="value">
                <h2> {value}</h2>
            </div>         
    );
};

TotalExpense.propTypes = {
    date: PropTypes.string.isRequired,
    user: PropTypes.object,
    refreshTrigger: PropTypes.number.isRequired
};

export default TotalExpense;
