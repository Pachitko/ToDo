import React, { useState } from 'react';
import { SInput, STextButton } from 'src/Components/UI';
import styled from 'styled-components';
import { useDispatch } from 'react-redux';
import { postTask } from 'src/redux/actions/taskActions';
import { ITaskToCreate } from 'src/libs/api';
import { useAppSelector } from 'src/redux/hooks';

const AddTask: React.FC = () => {
    const [isInputActive, setIsInputActive] = useState(false)
    const [taskTitle, setTaskTitle] = useState('')
    const dispatch = useDispatch()
    const activeList = useAppSelector(state => state.tasks.activeList)

    const handleFocus = () => {
        setIsInputActive(true)
    }

    const handleInputChange = (e: any) => {
        setTaskTitle(e.target.value)
    }

    const handleBlur = () => {
        setIsInputActive(taskTitle.length > 0)
    }

    const handleAddTask = (e: any) => {
        e.preventDefault()
        if (activeList === null)
            return

        const taskToCreate: ITaskToCreate = {
            title: taskTitle,
            isCompleted: false,
            isImportant: false,
            dueDate: null,
            recurrence: null
        }
        setTaskTitle('')
        setIsInputActive(false)
        dispatch(postTask(activeList.id, taskToCreate))
    }

    return (
        <SAddTask>
            <form>
                <SInputWrapper>
                    <STaskTitleInput value={taskTitle} active={isInputActive} placeholder='New task' spellCheck={false}
                        onFocus={handleFocus} onBlur={handleBlur} onChange={handleInputChange}
                        onSubmit={handleAddTask} />
                </SInputWrapper>
                <SPropsWrapper active={isInputActive}>
                    <span>Task props</span>
                    <div style={{ flexGrow: 1 }}></div>
                    <SAddTaskButton type='submit' onClick={handleAddTask}
                        disabled={taskTitle.length === 0}>Add</SAddTaskButton>
                </SPropsWrapper>
            </form>
        </SAddTask>
    );
}

export default AddTask

const STaskTitleInput = styled(SInput) <{ active: boolean }>`
    color: ${p => p.theme.colors.black};
    padding: 8px;
    width: 100%;
    background-color: ${p => p.active && 'transparent'};
    ::placeholder{
        color: ${props => props.theme.colors.disabled};
    }
`

const SAddTaskButton = styled(STextButton)`
    padding: 4px;
`

const SInputWrapper = styled.div`
    margin-bottom: 8px;
`

const SPropsWrapper = styled.div<{ active: boolean }>`
    display: ${p => p.active ? 'flex' : 'none'};
    padding-left: 8px;
    color: ${p => p.theme.colors.onSurface};
`

const SAddTask = styled.div`
    border-radius: ${({ theme }) => theme.border.radius};
    background-color: ${({ theme }) => theme.colors.surface};
    box-shadow: ${({ theme }) => theme.shadow.light.soft};
    padding: ${({ theme }) => theme.padding.small}px;
    display: flex;
    flex-direction: column;
    margin: 0 8px;
    margin-bottom: 16px;
`