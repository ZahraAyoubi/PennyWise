/* eslint-disable react/prop-types */
import { useState, useEffect } from 'react';
import PropTypes from 'prop-types';
import '../EditableTextbox.css';

const EditableTextbox = ({ fetchData, date }) => {
    const [amount, setAmount] = useState(0); // State for the textbox value
    const [isEditing, setIsEditing] = useState(false);

    useEffect(() => {
        const fetchCurrentIncome = async () => {
            try {
                const response = await fetch(`http://localhost:5259/api/transactions/Income/${date}`, { method: "GET" }); 
                const data = await response.json();
                setAmount(data.amount); // Assuming the API returns { income: value }
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
                body: JSON.stringify({ amount, description, date }),
                
            }); 
            fetchData();
            console.log(" Data:", { amount, description, date }); 
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

EditableTextbox.propTypes = {
    fetchData: PropTypes.func.isRequired,
    date: PropTypes.string.isRequired
};

export default EditableTextbox;
