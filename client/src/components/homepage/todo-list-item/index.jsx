import PropTypes from 'prop-types';

import styles from './index.module.scss';

const TodoListItem = ({id, name, onDelete}) => {
    const baseURL = import.meta.env.VITE_API_URL;
    const url = `${baseURL}/${id}`;

    return (
        <div className={styles.todoListItem}>
            <a
                className={styles.label}
                href={url}
                target="_blank"
                rel="noreferrer">
                {name}
            </a>
            <div>
                <button
                    type="button"
                    aria-label="Delete"
                    className="btn btn-outline-danger btn-sm float-right"
                    onClick={onDelete}>
                    <i className="fa fa-trash-o" />
                </button>
            </div>
        </div>
    );
};
TodoListItem.propTypes = {
    id: PropTypes.string.isRequired,
    name: PropTypes.string.isRequired,
    onDelete: PropTypes.func.isRequired,
};
export default TodoListItem;
