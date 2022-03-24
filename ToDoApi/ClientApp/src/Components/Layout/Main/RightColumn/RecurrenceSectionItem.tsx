import React, { useState } from 'react'
import { useDispatch } from 'react-redux'
import { SIconButton } from 'src/Components/UI'
import TaskDetailsContextMenu from 'src/Components/Layout/Main/RightColumn/TaskDetailsContextMenu'
import { removeTaskRecurrencePatch, replaceTaskRecurrencePatch } from 'src/libs/jsonPatches'
import { patchTaskAsync } from 'src/redux/actions/taskActions'
import { SSectionBodyButton, SSectionContent, SSectionIcon, SSectionItem } from './TaskDetailStyles'
import { ITask, RecurenceType, Recurrence } from 'src/redux/reducers/tasks'
import styled from 'styled-components'

export const RecurrenceSectionItem: React.FC<{ activeTask: ITask }> = ({ activeTask }) => {
    const [isFocused, setIsFocused] = useState(false)
    const [interval, setRecurrenceInterval]
        = useState(activeTask.recurrence ? activeTask.recurrence.interval : 1)

    const dispatch = useDispatch()

    const handleFocus = () => {
        setIsFocused(!isFocused)
    }

    const handleClear = () => {
        dispatch(patchTaskAsync(activeTask.toDoListId, activeTask.id, [removeTaskRecurrencePatch()]))
    }

    const handleRecurrenceIntervalChange = (e: any) => {
        setRecurrenceInterval(e.target.value)
    }

    const handleSet = (type: RecurenceType) => {
        const newRecurrence: Recurrence =
        {
            interval,
            type,
            startedAt: activeTask.dueDate ? activeTask.dueDate : new Date()
        }
        setIsFocused(false)
        dispatch(patchTaskAsync(activeTask.toDoListId, activeTask.id, [replaceTaskRecurrencePatch(newRecurrence)]))
    }

    const stringifyRecurrence = (recurrence: Recurrence): string => {

        const parseRecurrenceType = (type: RecurenceType): string => {
            switch (type) {
                case 0:
                    return 'hour'
                case 1:
                    return 'day'
                case 2:
                    return 'week'
                case 3:
                    return 'month'
                case 4:
                    return 'year'
                default:
                    return "none"
            }
        }

        return `Every ${recurrence.interval} ${parseRecurrenceType(recurrence.type)}
            from ${new Date(activeTask.dueDate ? activeTask.dueDate : '').toLocaleDateString()}`
    }

    return (
        <SSectionItem isFocused={isFocused}>
            <SSectionBodyButton onClick={handleFocus}>
                <SSectionIcon isSet={activeTask.recurrence !== null}>
                    <i className="fa-solid fa-repeat"></i>
                </SSectionIcon>
                <SSectionContent isSet={activeTask.recurrence !== null}>
                    {activeTask.recurrence ? stringifyRecurrence(activeTask.recurrence) : 'Recurrence'}
                </SSectionContent>
            </SSectionBodyButton>
            {
                activeTask.recurrence &&
                <SIconButton onClick={handleClear}>
                    <i className="fa-solid fa-times"></i>
                </SIconButton>
            }
            {
                isFocused &&
                <TaskDetailsContextMenu title={"Recurrence"} onClickOutside={handleFocus}>
                    <SIntervalWrapper>
                        <span>Repeat every </span>
                        <SRecurrenceInterval onChange={handleRecurrenceIntervalChange}
                            value={interval} min={1} max={99} type={"number"} />
                    </SIntervalWrapper>
                    <SContextMenuButtons>
                        <SContextMenuButton onClick={e => handleSet(RecurenceType.Hourly)}>Hour</SContextMenuButton>
                        <SContextMenuButton onClick={e => handleSet(RecurenceType.Daily)}>Day</SContextMenuButton>
                        <SContextMenuButton onClick={e => handleSet(RecurenceType.Weekly)}>Week</SContextMenuButton>
                        <SContextMenuButton onClick={e => handleSet(RecurenceType.Monthly)}>Month</SContextMenuButton>
                        <SContextMenuButton onClick={e => handleSet(RecurenceType.Yearly)}>Year</SContextMenuButton>
                    </SContextMenuButtons>
                </TaskDetailsContextMenu>
            }
        </SSectionItem >
    )
}

const SIntervalWrapper = styled.div`
    display: flex;
    align-items: center;
    padding: 4px 8px;
    >span{
        flex-grow: 1;
        margin-right: 8px;
    }
`

const SRecurrenceInterval = styled.input`
    font-size: 1rem;
    padding: 4px 8px;
    border: 1px solid ${p => p.theme.colors.primary};
    color: ${p => p.theme.colors.onSurface};
`

const SContextMenuButtons = styled.div`
    display: flex;
    flex-direction: column;
`

const SContextMenuButton = styled.button`
    padding: 0 8px;
    text-align: left;
    height: 32px;
    color: ${p => p.theme.colors.onSurface};
    :hover{
        background-color: ${p => p.theme.colors.surfaceHover};
    }
`