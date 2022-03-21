import React from 'react';
import { STaskCheckBox, STaskCheckBoxLabel } from './TaskCheckBoxStyles'
import { useDispatch } from 'react-redux';
import { patchTaskAsync } from 'src/redux/actions/taskActions';
import { useAppSelector } from 'src/redux/hooks';
import { replaceTaskIsCompletedPatch } from 'src/libs/jsonPatches';
import { ITask } from 'src/redux/reducers/tasks';

type Props = {
    task: ITask
}

const TaskCompletionCheckBox: React.FC<Props> = ({ task }) => {
    const dispatch = useDispatch();

    const handleCheckBoxChange = (e: any) => {
        dispatch(patchTaskAsync(task.toDoListId, task.id, [replaceTaskIsCompletedPatch(e.target.checked)]))
    }

    return (
        <STaskCheckBoxLabel>
            <i className={task.isCompleted ? 'fa-regular fa-circle-check' : 'fa-regular fa-circle'}></i>
            <STaskCheckBox
                checked={task.isCompleted}
                type='checkbox'
                onChange={handleCheckBoxChange}
            />
        </STaskCheckBoxLabel>
    );
}

export default TaskCompletionCheckBox;