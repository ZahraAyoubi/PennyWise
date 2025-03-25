import PropTypes from "prop-types";
import IncomeOverview from "./IncomeOverview";
import AddTransaction from "./AddTransaction"
import RotatingBudgetList from "./RotatingBudgetList";
import TransactionHistory from "./TransactionHistory";

const Dashboard = ({ totalExpenses, remainingBudget, income, date, setDate, rotatingBudget, onDelete, fetchData, user }) => {
    return (
        <div className="dashboard">
            <IncomeOverview totalExpenses={totalExpenses} remainingBudget={remainingBudget} income={income} date={date} user={user } />
            <RotatingBudgetList rotatingBudget={rotatingBudget} onDelete={onDelete} />
            <div className="card add-transaction">
                <h2>Add Transaction</h2>
                <AddTransaction onTransactionAdded={fetchData} user={user } />
            </div>
            <TransactionHistory date={date} setDate={setDate} />
        </div>
    );
};

Dashboard.propTypes = {
    totalExpenses: PropTypes.number.isRequired,
    remainingBudget: PropTypes.number.isRequired,
    income: PropTypes.number.isRequired,
    date: PropTypes.string.isRequired,
    setDate: PropTypes.func.isRequired,
    rotatingBudget: PropTypes.array.isRequired,
    onDelete: PropTypes.func.isRequired,
    fetchData: PropTypes.func.isRequired,
    user: PropTypes.object
};

export default Dashboard;