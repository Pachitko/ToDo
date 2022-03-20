import React, { useState } from 'react';
import { useDispatch } from 'react-redux';
import { SIconButton, SPanel } from 'src/Components/UI';
import BackScreen from 'src/Components/UI/BackScreen';
import CompleteTaskCheckbox from 'src/Components/UI/TaskCompletionCheckBox';
import ImportantTaskCheckbox from 'src/Components/UI/TaskImportanceCheckbox';
import { removeTaskDueDatePatch, removeTaskRecurrencePatch, replaceTaskDueDatePatch, replaceTaskRecurrencePatch, replaceTaskTitlePatch } from 'src/libs/jsonPatches';
import { patchTaskAsync } from 'src/redux/actions/taskActions';
import { useAppSelector } from 'src/redux/hooks';
import { ITask, RecurenceType, Recurrence } from 'src/redux/reducers/tasks';
import styled from 'styled-components';
import ContextMenu from './ContextMenu';

const TaskDetails = () => {
    const dispatch = useDispatch();
    const activeTask = useAppSelector(state => state.tasks.activeTask)
    const [title, setTitle] = useState(activeTask ? activeTask.title : '')
    if (!activeTask)
        return null

    const handleTitleChange = (e: any) => {
        setTitle(e.target.value)
    }

    const handleTitleKeyDown = (e: any) => {
        if (e.key === 'Enter')
            e.target.blur()
    }

    const handleTitleBlur = () => {
        if (activeTask === undefined)
            return

        dispatch(patchTaskAsync(activeTask.toDoListId, activeTask.id,
            [replaceTaskTitlePatch(title ? title : '')]))
    }

    return (activeTask === undefined ? null :
        <STaskDetails>
            <STaskDetailsHeader>
                <CompleteTaskCheckbox taskId={activeTask.id} isChecked={activeTask.isCompleted} />
                <STaskDetailsTitleInput spellCheck={false}
                    onKeyDown={handleTitleKeyDown} onChange={handleTitleChange}
                    onBlur={handleTitleBlur} defaultValue={activeTask.title} />
                <ImportantTaskCheckbox taskId={activeTask.id} isChecked={activeTask.isImportant} />
            </STaskDetailsHeader>
            <SSection>
                <DueDateSectionItem activeTask={activeTask} />
                <RecurrenceSectionItem activeTask={activeTask} />
            </SSection>
        </STaskDetails>
    );
}

interface SectionItemProps {
    activeTask: ITask
}

const DueDateSectionItem: React.FC<SectionItemProps> = ({ activeTask }) => {
    const [isFocused, setIsFocused] = useState(false)

    const dispatch = useDispatch()

    const handleFocus = () => {
        setIsFocused(!isFocused)
    }

    const handleClear = () => {
        dispatch(patchTaskAsync(activeTask.toDoListId, activeTask.id,
            [removeTaskDueDatePatch(), removeTaskRecurrencePatch()]))
    }

    const handleSet = (e: any) => {
        setIsFocused(false)
        dispatch(patchTaskAsync(activeTask.toDoListId, activeTask.id, [replaceTaskDueDatePatch(e.target.value)]))
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
                <ContextMenu title={"Due date"} onClickOutside={handleFocus}>
                    <SDueDateCalendar autoFocus type="date" name="calendar" onChange={handleSet}></SDueDateCalendar>
                </ContextMenu>
            }
        </SSectionItem >
    )
}

const RecurrenceSectionItem: React.FC<SectionItemProps> = ({ activeTask }) => {
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
                <ContextMenu title={"Recurrence"} onClickOutside={handleFocus}>
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
                </ContextMenu>
            }
        </SSectionItem >
    )
}
export default TaskDetails;

const SDueDateCalendar = styled.input`
    color: ${p => p.theme.colors.onSurface};
`

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

const STaskDetails = styled.div`
    display: flex;
    flex-direction: column;
    height: 100%;
`

const SSection = styled(SPanel)`
    background-color: ${p => p.theme.colors.surface};
    display: flex;
    margin-bottom: 8px;
    padding: 0;
`

const SSectionContent = styled.div<{ isSet: boolean }>`
    color: ${p => p.isSet ? p.theme.colors.onSurface : p.theme.colors.disabled};
    display: flex;
`

const SSectionItem = styled.div<{ isFocused: boolean }>`
    border-bottom: 2px solid ${p => p.theme.colors.surfaceHover};
    position: relative;
    height: 48px;
    display: flex;
    align-items: center;
    background-color: ${p => p.isFocused && p.theme.colors.gray};
    :hover{
        background-color: ${p => p.theme.colors.surfaceHover};
    }
`

const SSectionBodyButton = styled.button`
    height: 100%;
    display: flex;
    align-items: center;
    flex-grow: 1;
    padding: 4px 0;
`

const SSectionIcon = styled.section<{ isSet: boolean }>`
    display: flex;
    justify-content: center;
    align-items: center;
    width: 32px;
    height: 32px;
    &>i{
        color: ${p => p.isSet ? p.theme.colors.primary : p.theme.colors.disabled};
        height: 100%;
        line-height: 32px;
        font-size: 1rem;
    }
`

const STaskDetailsHeader = styled(SSection)`
    padding: 8px 0;
    flex-direction: row;
`

const STaskDetailsTitleInput = styled.input` // todo textarea
    padding: 0 4px;
    height: 100%;
    font-size: 1rem;
    border: 1px solid transparent;
    color:  ${p => p.theme.colors.onSurface};
    :focus{
        border: 1px solid ${p => p.theme.colors.onSurface};
    }
    :hover{
        background-color: ${p => p.theme.colors.surfaceHover};
    }
`