import React from "react";
import { useState } from 'react';
import Modal from './Modal';
import PropTypes from 'prop-types';
import '../AddTransaction.css';

const AddTransaction = ({  user, onRefresh}) => {
    const [description, setDescription] = useState('');
    const [amount, setAmount] = useState('');
    const [showModal, setShowModal] = useState(false);
    const [isFixed, setIsFixed] = useState(false);

    const userId = user.id;
    const handleSubmit = async (e) => {
        e.preventDefault();

        const date = new Date().toISOString(); 

        const response = await fetch("http://localhost:5259/api/transactions/createtransaction", {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
            },
            body: JSON.stringify({ amount, description, date, userId, isFixed }),
        });

        if (response.ok) {
            setShowModal(false);
            setAmount("");
            setDescription("");
            onRefresh();
            setIsFixed(false);
        } else {
            console.error("Failed to add transaction");
        }
    };

    const handleChange = (e) => {
        setIsFixed(e.target.checked);
    };

    return (
        <div className="card">
            <h2>Add Transaction</h2>
            <button className="add-button" onClick={() => setShowModal(true)}>Add Transaction</button>
            <Modal show={showModal} onClose={() => setShowModal(false)}>
                <h2>Add Transaction</h2>
                <form onSubmit={handleSubmit}>
                    <div className="label">
                        <div>
                            <label htmlFor="description">Description:</label>
                            <input
                                id="description"
                                className="input"
                                type="text"
                                value={description}
                                onChange={(e) => setDescription(e.target.value)}
                            />
                        </div>
                        <div>
                            <label htmlFor="amount">Amount:</label>
                            <input
                                id="amount"
                                className="input"
                                type="number"
                                value={amount}
                                onChange={(e) => setAmount(e.target.value)}
                            />
                        </div>
                        <div>
                            <label>
                                <input
                                    type="checkbox"
                                    checked={isFixed}
                                    onChange={handleChange}
                                />
                                It is a fixed budget
                            </label>
                        </div>
                    </div>
                    <div>
                        <button className="add-transaction-button" type="submit">
                            Add
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
    onRefresh: PropTypes.func.isRequired,
    user: PropTypes.object
};

export default AddTransaction;
