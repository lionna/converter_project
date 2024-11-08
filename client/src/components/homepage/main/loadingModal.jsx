import {CModal, CModalBody, CSpinner} from '@coreui/react';
import PropTypes from 'prop-types';

const LoadingModal = ({visible}) => (
    <CModal
        backdrop="static"
        visible={visible}
        alignment="center"
        aria-labelledby="StaticBackdropExampleLabel">
        <CModalBody alignment="center">
            <div className="d-flex align-items-center justify-content-center">
                <CSpinner
                    component="span"
                    style={{width: '40px', height: '40px'}}
                    aria-hidden="true"
                />
                Loading...
            </div>
        </CModalBody>
    </CModal>
);

LoadingModal.propTypes = {
    visible: PropTypes.bool,
};

LoadingModal.defaultProps = {
    visible: false,
};

export default LoadingModal;
