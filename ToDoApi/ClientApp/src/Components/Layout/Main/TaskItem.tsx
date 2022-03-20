import React from 'react'
import { ITask } from 'src/redux/reducers/tasks'
import { useAppDispatch } from 'src/redux/hooks';
import { selectTaskAction } from 'src/redux/actions/taskActions';
import TaskImportanceCheckbox from 'src/Components/UI/TaskImportanceCheckbox';
import TaskCompletionCheckBox from 'src/Components/UI/TaskCompletionCheckBox';
import styled from 'styled-components'

const TaskItem = ({ task }: { task: ITask }) => {
    const dispatch = useAppDispatch();

    const handleTaskSelect = () => {
        dispatch(selectTaskAction(task))
    }

    return (
        <STaskItem>
            <TaskCompletionCheckBox taskId={task.id} isChecked={task.isCompleted} />
            <STaskBody onClick={handleTaskSelect}>
                <STaskTitle>{task.title}</STaskTitle>
                <STaskOptions>Task options</STaskOptions>
            </STaskBody>
            <TaskImportanceCheckbox taskId={task.id} isChecked={task.isImportant} />
        </STaskItem>
    )
}

export default TaskItem

const STaskItem = styled.div`
    display: flex;
    flex-direction: row;
    align-items: center;
    width: 100%;
    height: 48px;
    border-bottom: 1px solid ${p => p.theme.colors.surface};
    border-top-left-radius: 5px;
    border-top-right-radius: 5px;
    &:hover{
        background-color:${p => p.theme.colors.surface};
        border-bottom: 1px solid transparent;
    }
`

const STaskBody = styled.button`
    display: flex;
    flex-direction: column;
    flex-grow: 1;
    padding: 5px 0;
    cursor: pointer;
    text-align: left;
    margin-left: 5px;
`

const STaskTitle = styled.span`
    color: ${p => p.theme.colors.onSurface};
    font-weight: bold;
    margin-bottom: 5px;
`

const STaskOptions = styled.span`
    color: ${p => p.theme.colors.onSurface};
`