import { useState, useEffect } from "react";
import PropTypes from "prop-types";
import "../Overview.css";

const FixedBudgetPopup = ({ onTransactionAdded, setShowFixedbudget, user, onDelete }) => {
    const [items, setItems] = useState([]);
    const [description, setDescription] = useState('');
    const [amount, setAmount] = useState('');
    const [date, setDate] = useState('');
    const [editIndex, setEditIndex] = useState(null);

    //const [value, setValue] = useState([]); // State for the textbox value
    const userId = user ? user.id : null;
    if (!userId) {
        console.error("User is null ");
    }

    const handleSubmit = async (e) => {
        e.preventDefault();
        setDate(new Date().toISOString())
        const isFixed = true;
        const response = await fetch("http://localhost:5259/api/transactions/createtransaction", {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
            },
            body: JSON.stringify({ amount, description, date, userId, isFixed }),
        });

        if (response.ok) {
            setAmount(""); // Clear input
            setDescription("");
            onTransactionAdded(); // Fetch new data immediately
        } else {
            console.error("Failed to add transaction");
        }
    };

    //const handleAdd = () => {
    //    if (newItem.trim() === "") return;
    //    if (editIndex !== null) {
    //        const updated = [...items];
    //        updated[editIndex] = newItem;
    //        setItems(updated);
    //        setEditIndex(null);
    //    } else {
    //        setItems([...items, newItem]);
    //    }
    //    setNewItem("");
    //};

    //const handleEdit = (index) => {
    //    setNewItem(items[index]);
    //    setEditIndex(index);
    //};

    //const handleRemove = (index) => {
    //    const updated = items.filter((_, i) => i !== index);
    //    setItems(updated);
    //};

    useEffect(() => {
        if (!userId) return; // Don't fetch if userId is not available

        const currentDate = new Date().toISOString().split("T")[0]; // Format as YYYY-MM-DD
        setDate(currentDate);

        const fetchFixedBudget = async () => {
            try {
                const response = await fetch(`http://localhost:5259/api/transactions/FixedBudget/${currentDate}/${userId}`, { method: "GET" });

                if (!response.ok) {
                    throw new Error(`HTTP error! Status: ${response.status}`);
                }

                const data = await response.json();
                setItems(data);
            } catch (error) {
                console.error("Error fetching fixed budget:", error);
            }
        };

        fetchFixedBudget();
    }, []); // Trigger only when userId is available

    return (
        <div className="dropdwon">
            <h3>Fixed Budget Items</h3>
            <ul>
                {items.map((item) => (
                    <li key={item.id} >
                        <div className="item-container">
                            <strong>{item.description}</strong>: {item.amount}sek
                            <button onClick={() => onDelete(item.id, item.amount)} className="delete-button" >
                                ❌
                            </button>
                        </div>
                    </li>
                ))}
            </ul>
            <form onSubmit={handleSubmit}>
                <input
                    type="text"
                    value={description}
                    onChange={(e) => setDescription(e.target.value)}
                    className="input"
                    placeholder="Description"
                />
                <input
                    type="number"
                    value={amount}
                    onChange={(e) => setAmount(e.target.value)}
                    className="input"
                    placeholder="Amount"
                />
                <button type="submit" className="add-transaction-button">
                    {editIndex !== null ? "Update" : "Add"}
                </button>
                <button onClick={() => setShowFixedbudget(false)} className="cancel-button">Close</button>
            </form>
        </div>

    );
};

FixedBudgetPopup.propTypes = {
    onTransactionAdded: PropTypes.func.isRequired,
    setShowFixedbudget: PropTypes.func.isRequired,
    onDelete: PropTypes.func.isRequired,
    user: PropTypes.object
};

export default FixedBudgetPopup;
