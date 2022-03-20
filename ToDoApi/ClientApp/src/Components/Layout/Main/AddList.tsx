import React, { useState } from 'react';
import { SButton, SInput, STextButton } from 'src/Components/UI';
import styled from 'styled-components';
import { useDispatch } from 'react-redux';
import { postTaskListAsync } from 'src/redux/actions/taskActions';

const AddList: React.FC = () => {
    const [isInputActive, setIsInputActive] = useState(false)
    const [taskListTitle, setListTitle] = useState('')
    const dispatch = useDispatch()

    const handleFocus = () => {
        setIsInputActive(true)
    }

    const handleInputChange = (e: any) => {
        setListTitle(e.target.value)
    }

    const handleBlur = () => {
        setIsInputActive(taskListTitle.length > 0)
    }

    const handleAddTask = (e: any) => {
        e.preventDefault()

        setListTitle('')
        setIsInputActive(false)
        dispatch(postTaskListAsync({ title: taskListTitle }))
    }

    return (
        <SAddList>
            <form>
                <SInputWrapper>
                    <STaskTitleInput value={taskListTitle} active={isInputActive} placeholder='New list' spellCheck={false}
                        onFocus={handleFocus} onBlur={handleBlur} onChange={handleInputChange}
                        onSubmit={handleAddTask} />
                </SInputWrapper>
                <SAddButtonWrapper active={isInputActive}>
                    <SAddTaskListButton type='submit' onClick={handleAddTask}
                        disabled={taskListTitle.length === 0}>Add</SAddTaskListButton>
                </SAddButtonWrapper>
            </form>
        </SAddList>
    );
}

export default AddList

const STaskTitleInput = styled(SInput) <{ active: boolean }>`
    color: ${p => p.theme.colors.black};
    padding: 8px;
    width: 100%;
    background-color: ${p => p.active && 'transparent'};
`

const SAddTaskListButton = styled(SButton)`
    padding: 4px;
    width: 100%;
    &:hover{
        color: ${p => p.theme.colors.white};
    }
    :disabled{
        color: ${p => p.theme.colors.disabled};
    }
`

const SInputWrapper = styled.div`
    margin-bottom: 8px;
`

const SAddButtonWrapper = styled.div<{ active: boolean }>`
    display: ${p => p.active ? 'block' : 'none'};
`

const SAddList = styled.div`
    background-color: ${({ theme }) => theme.colors.surface};
    border-radius: ${({ theme }) => theme.border.radius};
    box-shadow: ${({ theme }) => theme.shadow.light.soft};
    padding: ${({ theme }) => theme.padding.small}px;
    display: flex;
    flex-direction: column;
    margin-bottom: 16px;
`