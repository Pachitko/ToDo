import React, { useEffect } from 'react'
import { useAppSelector } from 'src/redux/hooks';
import TaskItem from './TaskItem';
import styled from 'styled-components'
import { Route, Routes } from 'react-router-dom';
import { useDispatch } from 'react-redux';
import { loadTasksAsync } from 'src/redux/actions/taskActions';
import AddTask from './AddTask';

const TaskList = () => {
    const activeListId = useAppSelector(state => state.tasks.activeListId)
    const activeTaskList = useAppSelector(state => state.tasks.taskLists
        .find(l => l.id === activeListId))
    const isTasksLoading = useAppSelector(state => state.tasks.isLoading)

    const dispatch = useDispatch()

    useEffect(() => {
        dispatch(loadTasksAsync())
        console.log(1, activeListId, activeTaskList);
    }, [])

    // if (isTasksLoading) {
    //     return <div>Loading</div>
    // }
    return (
        <Routes>
            <Route path={activeListId} element={
                <STaskListWrapper>
                    <STaskListTitleWrapper>
                        <STaskListTitle>
                            {activeTaskList !== undefined && activeTaskList.title}
                        </STaskListTitle>
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

const STaskListWrapper = styled.div`
    padding: 0 16px;
`

const STaskListTitleWrapper = styled.div`
    margin-bottom: 16px;
`

const STaskListTitle = styled.span`
    font-weight: bold;
    font-size: 2.5rem;
    color: ${p => p.theme.colors.primary}
`

const STaskListItems = styled.div`
`