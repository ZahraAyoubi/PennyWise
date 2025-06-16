import Income from "./Income";
import RemainingBudget from "./RemainingBudget"
import TotalExpense from "./TotalExpense"
import PropTypes from "prop-types";

const IncomeOverview = ({ income, date, user, onDelete, refreshTrigger, onRefresh }) => {
    
    return (
        <div className="card overview">
            <h2>Income</h2>
            <Income income={income} date={date} user={user} refreshTrigger={refreshTrigger} onRefresh={onRefresh} />
            <h2>Total Expenses</h2>
            <TotalExpense date={date} user={user} refreshTrigger={refreshTrigger} />
            <h2>Remaining Budget</h2>
            <RemainingBudget  date={date} user={user} onDelete={onDelete} refreshTrigger={refreshTrigger} />
        </div>
    );
};

IncomeOverview.propTypes = {
    onDelete: PropTypes.func.isRequired,
    income: PropTypes.number.isRequired,
    date: PropTypes.string.isRequired,
    user: PropTypes.object,
    refreshTrigger: PropTypes.number.isRequired,
    onRefresh: PropTypes.func.isRequired,
};

export default IncomeOverview;