import Income from "./Income";
import PropTypes from "prop-types";

const IncomeOverview = ({ totalExpenses, remainingBudget, income, date, user }) => {
    
    return (
        <div className="card overview">
            <h2>Income</h2>
            <Income income={income} date={date} user={user } />
            <h2>Total Expenses</h2>
            <div className="value">{totalExpenses}</div>
            <h2>Remaining Budget</h2>
            <div className="value">{remainingBudget}</div>
        </div>
    );
};

IncomeOverview.propTypes = {
    totalExpenses: PropTypes.number.isRequired,
    remainingBudget: PropTypes.number.isRequired,
    income: PropTypes.number.isRequired,
    date: PropTypes.string.isRequired,
    user: PropTypes.object
};

export default IncomeOverview;