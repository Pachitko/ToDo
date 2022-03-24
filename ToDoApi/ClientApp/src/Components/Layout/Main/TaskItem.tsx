import React from 'react'
import { ITask } from 'src/redux/reducers/tasks'
import { useAppDispatch, useAppSelector } from 'src/redux/hooks';
import { hideTaskDetailsAction, selectTaskAction } from 'src/redux/actions/taskActions';
import TaskImportanceCheckbox from 'src/Components/UI/TaskImportanceCheckbox';
import TaskCompletionCheckBox from 'src/Components/UI/TaskCompletionCheckBox';
import styled from 'styled-components'

const TaskItem = ({ task }: { task: ITask }) => {
    const dispatch = useAppDispatch();
    const activeTask = useAppSelector(state => state.tasks.activeTask)

    const handleTaskSelect = () => {
        if (activeTask && activeTask.id === task.id) {
            dispatch(hideTaskDetailsAction())
        }
        else {
            dispatch(selectTaskAction(task))
        }
    }

    return (
        <STaskItem isActive={activeTask?.id === task.id}>
            <TaskCompletionCheckBox task={task} />
            <STaskBodyButton onClick={handleTaskSelect}>
                <STaskTitle>{task.title}</STaskTitle>
                <STaskOptions>
                    <STaskDueDateOption>
                        {task.dueDate && new Date(task.dueDate).toLocaleDateString()}
                    </STaskDueDateOption>
                    <STaskReccurenceOption>
                        {task.recurrence && <i className="fa-solid fa-repeat"></i>}
                    </STaskReccurenceOption>
                </STaskOptions>
            </STaskBodyButton>
            <TaskImportanceCheckbox task={task} />
        </STaskItem>
    )
}

export default TaskItem

const STaskItem = styled.div<{ isActive: boolean }>`
    display: flex;
    flex-direction: row;
    align-items: center;
    width: 100%;
    height: 48px;
    border-bottom: 1px solid ${p => p.theme.colors.surface};
    background-color:${p => p.isActive && p.theme.colors.surfaceHover};
    &:hover{
        background-color:${p => p.isActive ? p.theme.colors.surfaceHover : p.theme.colors.surface};
        border-bottom: 1px solid transparent;
    }
`

const STaskBodyButton = styled.button`
    height: 100%;
    display: flex;
    flex-direction: column;
    justify-content: center;
    flex-grow: 1;
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
    display: flex;
    color: ${p => p.theme.colors.primary};
`

const STaskDueDateOption = styled.div`
    margin-right: 8px;
`

const STaskReccurenceOption = styled.div`
`
