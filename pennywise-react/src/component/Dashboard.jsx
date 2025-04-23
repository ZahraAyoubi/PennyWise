import PropTypes from "prop-types";
import IncomeOverview from "./IncomeOverview";
import AddTransaction from "./AddTransaction"
import RotatingBudgetList from "./RotatingBudgetList";
import TransactionHistory from "./TransactionHistory";
import FixedBudget from "./FixedBudget";

const Dashboard = ({ income, date, setDate, onDelete, user, refreshTrigger , onRefresh}) => {

    return (
        <div className="dashboard">
            <IncomeOverview  income={income} date={date} user={user} onDelete={onDelete} refreshTrigger={refreshTrigger} />
            <RotatingBudgetList onDelete={onDelete} date={date} user={user} refreshTrigger={refreshTrigger} />
            <FixedBudget user={user} onDelete={onDelete} refreshTrigger={refreshTrigger} onRefresh={onRefresh} />
            <AddTransaction  user={user} onRefresh={onRefresh} />
            <TransactionHistory date={date} setDate={setDate} />
        </div>
    );
};

Dashboard.propTypes = {
    income: PropTypes.number.isRequired,
    date: PropTypes.string.isRequired,
    setDate: PropTypes.func.isRequired,
    onDelete: PropTypes.func.isRequired,
    user: PropTypes.object,
    refreshTrigger: PropTypes.number.isRequired,
    onRefresh: PropTypes.func.isRequired
};

export default Dashboard;