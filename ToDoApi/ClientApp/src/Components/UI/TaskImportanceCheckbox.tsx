import React from 'react';
import { STaskCheckBox, STaskCheckBoxLabel } from './TaskCheckBoxStyles'
import { useDispatch } from 'react-redux';
import { patchTask } from 'src/redux/actions/taskActions';
import { useAppSelector } from 'src/redux/hooks';
import { replaceTaskIsImportantPatch } from 'src/libs/jsonPatches';
import { ITask } from 'src/redux/reducers/tasks';

type Props = {
    task: ITask,
}

const TaskImportanceCheckbox: React.FC<Props> = ({ task }) => {
    const dispatch = useDispatch();

    const handleCheckBoxChange = (e: any) => {
        dispatch(patchTask(task.toDoListId, task.id, [replaceTaskIsImportantPatch(e.target.checked)]))
    }

    return (
        <STaskCheckBoxLabel>
            <i className={task.isImportant ? 'fa-solid fa-star' : 'fa-regular fa-star'}></i>
            <STaskCheckBox
                checked={task.isImportant}
                type='checkbox'
                onChange={handleCheckBoxChange}
            />
        </STaskCheckBoxLabel>
    );
}

export default TaskImportanceCheckbox;