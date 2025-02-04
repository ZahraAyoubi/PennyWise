import { useState, useEffect } from 'react';
import axios from 'axios';
import '../AddTransaction.css'
const TransactionList = () => {
    const [transactions, setTransactions] = useState([]);

    useEffect(() => {
        axios.get('http://localhost:5000/api/transaction')
            .then(response => {
                setTransactions(response.data);
            })
            .catch(error => {
                console.error('There was an error!', error);
            });
    }, []);

    return (
        <div>
            <h1>Transaction List</h1>
            <ul>
                {transactions.map((transaction) => (
                    <li key={transaction.id}>
                        {transaction.description} - ${transaction.amount} - {new Date(transaction.date).toLocaleString()}
                    </li>
                ))}
            </ul>
        </div>
    );
}

export default TransactionList;
