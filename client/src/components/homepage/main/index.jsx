import {useEffect, useState} from 'react';

import {
    deleteConvertedDocumentsService,
    getConvertedDocumentsService,
} from '../../../services';
import TodoList from '../todo-list';
import FileUploader from './fileUploader.jsx';
import styles from './index.module.scss';
import LoadingModal from './loadingModal.jsx';
import {CAlert} from '@coreui/react';

const MainBlock = () => {
    const [isLoading, setLoading] = useState(false);
    const [visibleItems, setVisibleItems] = useState([]);
    const [uploadErrorMessage, setUploadErrorMessage] = useState('');

    useEffect(() => {
        const fetchPostcodeSuggestions = async () => {
            setLoading(true);
            setUploadErrorMessage();
            const {message, hasError, result} =
                await getConvertedDocumentsService();
            if (hasError) {
                const logMessage =
                    message || 'Something went wrong. Please try again.';
                console.log(logMessage);
                setUploadErrorMessage(logMessage);
            } else if (Array.isArray(result)) {
                setVisibleItems(result);
            }
            setLoading(false);
        };

        fetchPostcodeSuggestions();
    }, []);

    const onDelete = async id => {
        setLoading(true);
        setUploadErrorMessage();
        const {message, hasError} = await deleteConvertedDocumentsService(id);
        if (hasError) {
            const logMessage =
                message || 'Something went wrong. Please try again.';
            console.log(logMessage);
            setUploadErrorMessage(logMessage);
        } else {
            const filteredItems = visibleItems.filter(item => item.id !== id);

            setVisibleItems(filteredItems);
        }

        setLoading(false);
    };

    const addItem = item => {
        setVisibleItems([...visibleItems, item]);
    };

    return (
        <div className={styles.mainBlock}>
            <FileUploader
                addItem={addItem}
                isLoading={isLoading}
                setLoading={setLoading}
                setUploadErrorMessage={setUploadErrorMessage}
            />
            {!!uploadErrorMessage && (
                <CAlert color="danger">{uploadErrorMessage}</CAlert>
            )}
            <TodoList items={visibleItems} onDelete={onDelete} />
            <LoadingModal visible={isLoading} />
        </div>
    );
};

export default MainBlock;
