import '../RotatingBudget.css';
import PropTypes from 'prop-types';

const RotatingBudget = ({ rotatingBudget , onDelete }) => {

    return (
        <ul>
            {rotatingBudget.map((item) => (
                <li key={item.id} >
                <div className="item-container">
                    <strong>{item.description}</strong>: {item.amount}sek
                    <button onClick={() => onDelete(item.id)} className="delete-button" >
                        ❌ 
                        </button>
                    </div>
                </li>
            ))}
            </ul>
    );
};

RotatingBudget.propTypes = {
    rotatingBudget: PropTypes.array.isRequired,
    onDelete: PropTypes.func.isRequired,
};

export default RotatingBudget;
