import '../Modal.css';
import PropTypes from 'prop-types';

const Modal = ({ show, onClose, children }) => {
    if (!show) return null;

    return (
        <div className="modal-overlay">
            <div className="modal-content">
                <button className="modal-close" onClick={onClose}>X</button>
                {children}
            </div> 
        </div>
    );
};

// PropTypes validation
Modal.propTypes = {
    show: PropTypes.bool.isRequired,  // 'show' should be a boolean
    onClose: PropTypes.func.isRequired,  // 'onClose' should be a function
    children: PropTypes.node.isRequired,  // 'children' can be any node (React elements)
};


export default Modal;
