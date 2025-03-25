import PropTypes from "prop-types";
import RotatingBudget from "./RotatingBudget";

const RotatingBudgetList = ({ rotatingBudget, onDelete }) => {
    return (
        <div className="card rotating-budget">
            <h2>Rotating Budget</h2>
            <RotatingBudget rotatingBudget={rotatingBudget} onDelete={onDelete} />
        </div>
    );
};

RotatingBudgetList.propTypes = {
    rotatingBudget: PropTypes.array.isRequired,
    onDelete: PropTypes.func.isRequired
};

export default RotatingBudgetList;