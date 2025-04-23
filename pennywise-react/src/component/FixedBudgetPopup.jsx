import { useState, useEffect } from "react";
import PropTypes from "prop-types";

const FixedBudgetPopup = ({ setShowFixedbudget, user, onDelete ,refreshTrigger, onRefresh}) => {
    const [items, setItems] = useState([]);
    const [description, setDescription] = useState('');
    const [amount, setAmount] = useState('');
    const [date, setDate] = useState('');
    const [editIndex, setEditIndex] = useState(null);

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
            setAmount("");
            setDescription("");
            onRefresh();
        } else {
            console.error("Failed to add transaction");
        }
    };

    useEffect(() => {
        if (!userId) return; // Don't fetch if userId is not available

        const fetchFixedBudget = async () => {
            const currentDate = new Date().toISOString().split("T")[0];
            setDate(currentDate)

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

    }, [ refreshTrigger]);

    return (
        <div className="dropdwon">
            <h3>Fixed Budget Items</h3>
            <ul>
                {items.map((item) => (
                    <li key={item.id} >
                        <div className="item-container">
                            <strong>{item.description}</strong>: {item.amount}sek
                            <button onClick={() => onDelete(item.id)} className="delete-button" >
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
                <button  type="submit" className="add-transaction-button">
                    {editIndex !== null ? "Update" : "Add"}
                </button>
                <button onClick={() => setShowFixedbudget(false)} className="cancel-button">Close</button>
            </form>
        </div>

    );
};

FixedBudgetPopup.propTypes = {
    setShowFixedbudget: PropTypes.func.isRequired,
    onDelete: PropTypes.func.isRequired,
    user: PropTypes.object,
    refreshTrigger: PropTypes.number.isRequired,
    onRefresh: PropTypes.func.isRequired
};

export default FixedBudgetPopup;
