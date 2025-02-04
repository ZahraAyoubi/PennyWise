import { useState } from 'react';
import Modal from './Modal';
import PropTypes from 'prop-types';
import '../AddTransaction.css';

const AddTransaction = ({ onTransactionAdded }) => {
    const [description, setDescription] = useState('');
    const [amount, setAmount] = useState('');
    const [date, setDate] = useState('');
    const [showModal, setShowModal] = useState(false);

    const handleSubmit = async (e) => {
        e.preventDefault();
        setDate(new Date().toISOString())
        const response = await fetch("http://localhost:5259/api/transactions/createtransaction", {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
            },
            body: JSON.stringify({ amount, description,date }),
        });

        if (response.ok) {
            setShowModal(false);
            setAmount(""); // Clear input
            setDescription("");
            onTransactionAdded(); // Fetch new data immediately
        } else {
            console.error("Failed to add transaction");
        }
    };
    return (
        <div>
            <button className="add-button" onClick={() => setShowModal(true)}>Add Transaction</button>
            <Modal show={showModal} onClose={() => setShowModal(false)}>
                <h2>Add Transaction</h2>
                <form onSubmit={handleSubmit}>
                    <div className= "label">
                    <div>
                            <label>Description: </label>
                            <input className ="input"
                            type="text"
                            value={description}
                            onChange={(e) => setDescription(e.target.value)}
                            />
                    </div>
                    <div>
                            <label>Amount: </label>
                            <input className = "input"
                            type="number"
                            value={amount}
                            onChange={(e) => setAmount(e.target.value)}
                        />
                        </div>
                    </div>
                    <div>
                        <button className="add-transaction-button" type="submit">
                            Add Transaction
                        </button>

                        <button className="cancel-button" onClick={() => setShowModal(false)}>
                            Cancel
                        </button>
                    </div>
                    
                </form>
            </Modal>
        </div>
    );
};

AddTransaction.propTypes = {
    onTransactionAdded: PropTypes.func.isRequired
};

export default AddTransaction;
