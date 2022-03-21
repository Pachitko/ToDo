import React, { useEffect, useState } from 'react'
import { useAppSelector } from 'src/redux/hooks';
import TaskItem from './TaskItem';
import styled from 'styled-components'
import { Route, Routes } from 'react-router-dom';
import { useDispatch } from 'react-redux';
import { loadTasksAsync, renameTaskListAsync } from 'src/redux/actions/taskActions';
import AddTask from './AddTask';

const TaskList = () => {
    const activeListId = useAppSelector(state => state.tasks.activeListId)
    const activeTaskList = useAppSelector(state => state.tasks.taskLists
        .find(l => l.id === activeListId))
    const isTasksLoading = useAppSelector(state => state.tasks.isLoading)
    const [title, setTitle] = useState('')
    const dispatch = useDispatch()

    useEffect(() => {
        dispatch(loadTasksAsync())
        console.log(1, activeListId, activeTaskList);
    }, [])

    useEffect(() => {
        if (activeTaskList)
            setTitle(activeTaskList.title)
    }, [activeTaskList?.title])

    // if (isTasksLoading) {
    //     return <div>Loading</div>
    // }

    const handleTitleChange = (e: any) => {
        setTitle(e.target.value)
    }

    const handleTitleKeyDown = (e: any) => {
        if (e.key === 'Enter')
            e.target.blur()
    }

    const handleTitleBlur = () => {
        if (activeTaskList === undefined)
            return

        dispatch(renameTaskListAsync(activeTaskList.id, title))
    }

    return (
        <Routes>
            <Route path={activeListId} element={
                <STaskListWrapper>
                    <STaskListTitleWrapper>
                        <STaskListTitle spellCheck={false}
                            value={title}
                            onKeyDown={handleTitleKeyDown} onChange={handleTitleChange}
                            onBlur={handleTitleBlur} />
                    </STaskListTitleWrapper>
                    <AddTask />
                    <STaskListItems>
                        {activeTaskList !== undefined && activeTaskList.tasks.map((task, i) =>
                            <TaskItem key={task.id} task={task} />
                        )}
                    </STaskListItems>
                </STaskListWrapper>
            } />
            <Route path='*' element={<span>Список не найден</span>} />
        </Routes>
    )
}

export default TaskList

const STaskListTitle = styled.input`
    padding: 0 4px;
    width: 100%;
    font-weight: bold;
    font-size: 2.5rem;
    border: 1px solid transparent;
    color: ${p => p.theme.colors.primary};
    :focus{
        border: 1px solid ${p => p.theme.colors.onSurface};
    }
    :hover{
        background-color: ${p => p.theme.colors.surfaceHover};
    }
`

const STaskListWrapper = styled.div`
    padding: 16px;
`

const STaskListTitleWrapper = styled.div`
    margin-bottom: 16px;
`

const STaskListItems = styled.div`
`