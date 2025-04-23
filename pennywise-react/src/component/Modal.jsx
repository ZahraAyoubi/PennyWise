import React from "react";
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

Modal.propTypes = {
    show: PropTypes.bool.isRequired, 
    onClose: PropTypes.func.isRequired,  
    children: PropTypes.node.isRequired,  
};


export default Modal;
