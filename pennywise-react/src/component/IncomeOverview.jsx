import Income from "./Income";
import RemainingBudget from "./RemainingBudget"
import TotalExpense from "./TotalExpense"
import PropTypes from "prop-types";

const IncomeOverview = ({ income, date, user }) => {
    
    return (
        <div className="card overview">
            <h2>Income</h2>
            <Income income={income} date={date} user={user } />
            <h2>Total Expenses</h2>
            <TotalExpense date={date} user={user} />
            <h2>Remaining Budget</h2>
            <RemainingBudget date={date} user={user} />
        </div>
    );
};

IncomeOverview.propTypes = {
    income: PropTypes.number.isRequired,
    date: PropTypes.string.isRequired,
    user: PropTypes.object
};

export default IncomeOverview;