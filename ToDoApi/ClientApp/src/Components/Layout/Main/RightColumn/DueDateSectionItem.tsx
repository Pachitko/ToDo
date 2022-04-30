import React, { BaseSyntheticEvent, ChangeEvent, useState } from 'react'
import { useDispatch } from 'react-redux'
import { SIconButton } from 'src/Components/UI'
import TaskDetailsContextMenu from 'src/Components/Layout/Main/RightColumn/TaskDetailsContextMenu'
import { removeTaskDueDatePatch, removeTaskRecurrencePatch, replaceTaskDueDatePatch } from 'src/libs/jsonPatches'
import { patchTask } from 'src/redux/actions/taskActions'
import { SSectionBodyButton, SSectionContent, SSectionIcon, SSectionItem } from './TaskDetailStyles'
import { ITask } from 'src/redux/reducers/tasks'
import styled from 'styled-components'

export const DueDateSectionItem: React.FC<{ activeTask: ITask }> = ({ activeTask }) => {
    const [isFocused, setIsFocused] = useState(false)

    const dispatch = useDispatch()

    const handleFocus = () => {
        setIsFocused(!isFocused)
    }

    const handleClear = () => {
        dispatch(patchTask(activeTask.toDoListId, activeTask.id,
            [removeTaskDueDatePatch(), removeTaskRecurrencePatch()]))
    }

    const handleSet = (e: any) => {
        setIsFocused(false)
        dispatch(patchTask(activeTask.toDoListId, activeTask.id, [replaceTaskDueDatePatch(new Date(e.target.value))]))
    }

    return (
        <SSectionItem isFocused={isFocused}>
            <SSectionBodyButton onClick={handleFocus}>
                <SSectionIcon isSet={activeTask.dueDate !== null}>
                    <i className="fa-regular fa-calendar"></i>
                </SSectionIcon>
                <SSectionContent isSet={activeTask.dueDate !== null}>
                    {activeTask.dueDate ? new Date(activeTask.dueDate).toLocaleDateString() : 'Due date'}
                </SSectionContent>
            </SSectionBodyButton>
            {activeTask.dueDate &&
                <SIconButton onClick={handleClear}>
                    <i className="fa-solid fa-times"></i>
                </SIconButton>
            }
            {isFocused &&
                <TaskDetailsContextMenu title={"Due date"} onClickOutside={handleFocus}>
                    <SDueDateCalendar autoFocus type="date" name="calendar" onChange={handleSet}></SDueDateCalendar>
                </TaskDetailsContextMenu>
            }
        </SSectionItem >
    )
}

const SDueDateCalendar = styled.input`
    color: ${p => p.theme.colors.onSurface};
`
