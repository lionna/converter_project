import {CButton} from '@coreui/react';
import PropTypes from 'prop-types';
import {useState} from 'react';
import {useDropzone} from 'react-dropzone';

import {downloadDocumentService} from '../../../services';
import styles from './index.module.scss';

const FileUploader = ({addItem, setLoading, setUploadErrorMessage}) => {
    const handleUploadingCV = async file => {
        setLoading(true);
        const formData = new FormData();
        formData.append('htmlFile', file);
        const {message, hasError, result} =
            await downloadDocumentService(formData);
        if (hasError) {
            const logMessage =
                message || 'Something went wrong. Please try again.';
            console.log(logMessage);

            setUploadErrorMessage(logMessage);
        } else {
            addItem(result);
        }
        setLoading(false);
    };

    const onDrop = acceptedFiles => {
        setUploadErrorMessage();

        if (acceptedFiles.length > 1) {
            setUploadErrorMessage('You attempted to upload multiple files.');
        } else if (acceptedFiles.length) {
            handleUploadingCV(acceptedFiles[0]);
        }
    };
    const {getRootProps, getInputProps, open} = useDropzone({
        noClick: true,
        accept: {
            'text/plain': ['.html'],
        },
        onDrop,
    });
    return (
        <div className={styles.dropzone} {...getRootProps()}>
            <input data-qa="drop-input" {...getInputProps()} />
            <h3>Drop your file here or click to browse</h3>
            <div className={styles.wrapper}>
                <i className="fa-solid fa-download fa-2xl" />
            </div>

            <CButton
                color="primary"
                variant="outline"
                onClick={open}
                className={styles.button}>
                Choose
            </CButton>
        </div>
    );
};

FileUploader.propTypes = {
    addItem: PropTypes.func.isRequired,
    setLoading: PropTypes.func.isRequired,
    setUploadErrorMessage: PropTypes.func.isRequired,
};

export default FileUploader;
