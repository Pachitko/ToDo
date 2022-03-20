import React from 'react';
import { STaskCheckBox, STaskCheckBoxLabel } from './TaskCheckBoxStyles'
import { useDispatch } from 'react-redux';
import { patchTaskAsync } from 'src/redux/actions/taskActions';
import { useAppSelector } from 'src/redux/hooks';
import { replaceTaskIsImportantPatch } from 'src/libs/jsonPatches';

type Props = {
    taskId: string,
    isChecked: boolean
}

const TaskImportanceCheckbox: React.FC<Props> = ({ taskId, isChecked }) => {
    const dispatch = useDispatch();
    const listId = useAppSelector(state => state.tasks.activeListId)

    const handleCheckBoxChange = (e: any) => {
        dispatch(patchTaskAsync(listId, taskId, [replaceTaskIsImportantPatch(e.target.checked)]))
    }


    return (
        <STaskCheckBoxLabel>
            <i className={isChecked ? 'fa-solid fa-star' : 'fa-regular fa-star'}></i>
            <STaskCheckBox
                checked={isChecked}
                type='checkbox'
                onChange={handleCheckBoxChange}
            />
        </STaskCheckBoxLabel>
    );
}

export default TaskImportanceCheckbox;