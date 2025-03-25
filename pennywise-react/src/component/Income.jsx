/* eslint-disable react/prop-types */
import { useState, useEffect } from 'react';
import PropTypes from 'prop-types';
import '../Income.css';

const Income = ({ income, date, user }) => {
    if (!user) {
        console.error("User is null in Income");
    }

    const [amount, setAmount] = useState(income); // State for the textbox value
    const [isEditing, setIsEditing] = useState(false);
    //console.log('income:',income);
    //console.log('amount:', amount)
    const userId = user ? user.id : null;
    if (!userId) {
        console.error("User is null in Income");
    }
        
    useEffect(() => {
        const fetchCurrentIncome = async () => {
            try {
                //const response = await fetch(`http://localhost:5259/api/transactions/Income/${date} ?${userId}`, { method: "GET" });
                const response = await fetch(`http://localhost:5259/api/transactions/Income/${date}/${userId}`, {method: "GET",});

                console.log(response);
                const data = await response.json();
                setAmount(data); // Assuming the API returns { income: value }
            } catch (error) {
                console.error('Error fetching income:', error);
            }
        };

        fetchCurrentIncome();
    }, [date]); 

    // Save updated value to the database
    const handleSave = async () => {
        setIsEditing(false);
        const date = new Date().toISOString();
        const description = "Updated Income";
        try {
            await fetch("http://localhost:5259/api/transactions/AddIncome", {
                method: "POST",
                headers: {
                    "Content-Type": "application/json",
                },
                body: JSON.stringify({ amount, description, date, userId } ),
                
            }); 
            //fetchData();
            console.log(" Data:", { amount, description, date, user }); 
        } catch (error) {
            console.error('Error saving income:', error);
        }
    };

    return (
        <div className="editable-textbox">
            {isEditing ? (
                <div className="edit-mode">
                    <input
                        type="number"
                        value={amount}
                        onChange={(e) => setAmount(e.target.value)}
                        className="textbox-input"
                    />
                    <button className="save-button" onClick={handleSave}>
                        Save
                    </button>
                </div>
            ) : (
                    <div className="view-mode">
                        <span className="textbox-value">{amount}</span>
                    <button
                        className="edit-button"
                        onClick={() => setIsEditing(true)}
                        title="Edit"
                    >
                        ✏️
                    </button>
                </div>
            )}
           
        </div>
    );
};

Income.propTypes = {
    income: PropTypes.number.isRequired,
    date: PropTypes.string.isRequired,
    user: PropTypes.object
};

export default Income;
