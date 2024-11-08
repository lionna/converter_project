import PropTypes from 'prop-types';

import TodoListItem from '../todo-list-item';
import styles from './index.module.scss';

const TodoList = ({items, onDelete}) => {
    const elements = items.map(item => {
        const {id, ...itemProps} = item;
        return (
            <li key={id} className={styles.listGroupItem}>
                <TodoListItem
                    id={id}
                    {...itemProps}
                    onDelete={() => onDelete(id)}
                />
            </li>
        );
    });

    return <ul className="todo-list list-group">{elements}</ul>;
};

TodoList.propTypes = {
    items: PropTypes.array,
    onDelete: PropTypes.func.isRequired,
};

TodoList.defaultProps = {
    items: [],
};

const TodoListContainer = ({items, ...props}) => {
    if (!items.length) {
        return <div className={styles.listGroupItem}>No items</div>;
    }
    return <TodoList items={items} {...props} />;
};

TodoListContainer.propTypes = {
    items: PropTypes.array,
    onDelete: PropTypes.func.isRequired,
};

TodoListContainer.defaultProps = {
    items: [],
};

export default TodoListContainer;
