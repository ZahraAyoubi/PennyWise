import PropTypes from "prop-types";


const TransactionHistory = ({ date, setDate }) => {
    return (
        <div className="card transaction-history">
            <h2>History</h2>
            <input className="month-button" type="date" value={date} onChange={(e) => setDate(e.target.value)} />
        </div>
    );
};

TransactionHistory.propTypes = {
    date: PropTypes.string.isRequired,
    setDate: PropTypes.func.isRequired
};

export default TransactionHistory;