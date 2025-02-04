import { useState, useEffect } from 'react';
import axios from 'axios';
import '../TotalExpense.css';
import PropTypes from 'prop-types';

const RemainingBudget = ({ apiGetUrl }) => {
    const [value, setValue] = useState(0); // State for the textbox value

    // Fetch current income when the component loads
    useEffect(() => {
        const fetchCurrentIncome = async () => {
            try {
                const response = await axios.get(apiGetUrl); // API call to fetch the current value
                setValue(response.data); // Assuming the API returns { income: value }
            } catch (error) {
                console.error('Error fetching income:', error);
            }
        };

        fetchCurrentIncome();
    }, [apiGetUrl]); // Runs only once when the component mounts

    return (
        <div className="value">
            <h2> {value}</h2>
        </div>
    );
};

RemainingBudget.propTypes = {
    apiGetUrl: PropTypes.string.isRequired
};

export default RemainingBudget;
