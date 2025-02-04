import { useState, useEffect } from 'react';
import '../TotalExpense.css';
import PropTypes from 'prop-types';

const TotalExpense = ({ apiGetUrl }) => {
    const [value, setValue] = useState(0); // State for the textbox value

    // Fetch current income when the component loads
    useEffect(() => {
        const fetchTotalExpens = async () => {
            try {
                const response = await fetch(apiGetUrl, { method: "Get" }); // API call to fetch the current value
                
                const expensesData = await response.json();
                setValue(expensesData); // Assuming the API returns { income: value }
            } catch (error) {
                console.error('Error fetching income:', error);
            }
        };

        fetchTotalExpens();
    }, [apiGetUrl]); // Runs only once when the component mounts

    return (
            <div className="value">
                <h2> {value}</h2>
            </div>         
    );
};

TotalExpense.propTypes = {
    apiGetUrl: PropTypes.string.isRequired
};

export default TotalExpense;
