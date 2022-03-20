import React from 'react';
import { STaskCheckBox, STaskCheckBoxLabel } from './TaskCheckBoxStyles'
import { useDispatch } from 'react-redux';
import { patchTaskAsync } from 'src/redux/actions/taskActions';
import { useAppSelector } from 'src/redux/hooks';
import { replaceTaskIsCompletedPatch } from 'src/libs/jsonPatches';

type Props = {
    taskId: string,
    isChecked: boolean
}

const TaskCompletionCheckBox: React.FC<Props> = ({ taskId, isChecked }) => {
    const dispatch = useDispatch();
    const listId = useAppSelector(state => state.tasks.activeListId)

    const handleCheckBoxChange = (e: any) => {
        dispatch(patchTaskAsync(listId, taskId, [replaceTaskIsCompletedPatch(e.target.checked)]))
    }

    return (
        <STaskCheckBoxLabel>
            <i className={isChecked ? 'fa-regular fa-circle-check' : 'fa-regular fa-circle'}></i>
            <STaskCheckBox
                checked={isChecked}
                type='checkbox'
                onChange={handleCheckBoxChange}
            />
        </STaskCheckBoxLabel>
    );
}

export default TaskCompletionCheckBox;