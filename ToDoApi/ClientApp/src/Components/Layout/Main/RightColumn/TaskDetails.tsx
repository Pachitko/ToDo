import React, { useEffect, useState } from 'react';
import { useDispatch } from 'react-redux';
import { PanelCss } from 'src/Components/UI';
import CompleteTaskCheckbox from 'src/Components/UI/TaskCompletionCheckBox';
import ImportantTaskCheckbox from 'src/Components/UI/TaskImportanceCheckbox';
import { replaceTaskTitlePatch } from 'src/libs/jsonPatches';
import { patchTask } from 'src/redux/actions/taskActions';
import { useAppSelector } from 'src/redux/hooks';
import styled from 'styled-components';
import { DueDateSectionItem } from './DueDateSectionItem';
import { RecurrenceSectionItem } from './RecurrenceSectionItem';

const TaskDetails = () => {
    const dispatch = useDispatch();
    const activeTask = useAppSelector(state => state.tasks.activeTask)
    const [title, setTitle] = useState('')

    useEffect(() => {
        if (activeTask)
            setTitle(activeTask.title)
    }, [activeTask?.title])

    if (activeTask === null) {
        return null
    }

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

        dispatch(patchTask(activeTask.toDoListId, activeTask.id,
            [replaceTaskTitlePatch(title ? title : '')]))
    }

    return (activeTask === undefined ? null :
        <STaskDetails>
            <STaskDetailsHeader>
                <CompleteTaskCheckbox task={activeTask} />
                <STaskDetailsTitleInput spellCheck={false}
                    value={title}
                    onKeyDown={handleTitleKeyDown} onChange={handleTitleChange}
                    onBlur={handleTitleBlur} />
                <ImportantTaskCheckbox task={activeTask} />
            </STaskDetailsHeader>
            <SSection>
                <DueDateSectionItem activeTask={activeTask} />
                <RecurrenceSectionItem activeTask={activeTask} />
            </SSection>
        </STaskDetails>
    );
}

export default TaskDetails;

const SSection = styled.div`
    ${PanelCss};
    background-color: ${p => p.theme.colors.surface};
    display: flex;
    margin-bottom: 8px;
    padding: 0;
`

const STaskDetails = styled.div`
    display: flex;
    flex-direction: column;
    height: 100%;
`

const STaskDetailsHeader = styled(SSection)`
    padding: 8px 0;
    flex-direction: row;
`

const STaskDetailsTitleInput = styled.input` // todo textarea
    width: 100%;
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