import PropTypes from "prop-types";
import IncomeOverview from "./IncomeOverview";
import AddTransaction from "./AddTransaction"
import RotatingBudgetList from "./RotatingBudgetList";
import TransactionHistory from "./TransactionHistory";
import FixedBudget from "./FixedBudget";

const Dashboard = ({  income, date, setDate, onDelete, fetchData, user }) => {
    return (
        <div className="dashboard">
            <IncomeOverview  income={income} date={date} user={user } />
            <RotatingBudgetList onDelete={onDelete} date={date} user={user} />
            <FixedBudget onTransactionAdded={fetchData}  onDelete={onDelete}  user={user} />
            <AddTransaction onTransactionAdded={fetchData} user={user} />
            <TransactionHistory date={date} setDate={setDate} />
        </div>
    );
};

Dashboard.propTypes = {
    income: PropTypes.number.isRequired,
    date: PropTypes.string.isRequired,
    setDate: PropTypes.func.isRequired,
    onDelete: PropTypes.func.isRequired,
    fetchData: PropTypes.func.isRequired,
    user: PropTypes.object
};

export default Dashboard;