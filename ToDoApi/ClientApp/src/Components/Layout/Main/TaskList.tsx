import React, { useEffect, useState } from 'react'
import { useAppSelector } from 'src/redux/hooks';
import TaskItem from './TaskItem';
import styled from 'styled-components'
import { Route, Routes } from 'react-router-dom';
import { useDispatch } from 'react-redux';
import { loadTasks, renameTaskList } from 'src/redux/actions/taskActions';
import AddTask from './AddTask';
import { SIconButtonFilled } from 'src/Components/UI';
import { toggleLeftColumn } from 'src/redux/actions/globalActions';

const TaskList = () => {
    // const activeListId = useAppSelector(state => state.tasks.activeListId)
    let activeTaskList = useAppSelector(state => state.tasks.activeList)
    const tasks = useAppSelector(state => state.tasks.tasks)
    const isTasksLoading = useAppSelector(state => state.tasks.isLoading)
    const [title, setTitle] = useState('')
    const dispatch = useDispatch()
    const searchText = useAppSelector(state => state.searchTool.searchText)
    const isLeftColumnActive = useAppSelector(state => state.global.isLeftColumnActive)

    useEffect(() => {
        if (activeTaskList) {
            setTitle(activeTaskList.title)
        }
    }, [activeTaskList?.title])

    useEffect(() => {
        dispatch(loadTasks())
    }, [])

    // if (isTasksLoading) {
    //     return <div>Loading</div>
    // }

    if (searchText) {
        return (
            <STaskListWrapper>
                <STaskListTitleWrapper>
                    <STaskListTitle>
                        <span>Search by query: &quot;{searchText}&quot;</span>
                    </STaskListTitle>
                </STaskListTitleWrapper>
                {/* todo change for smart lists */}
                <STaskListItems>
                    {
                        tasks.filter(t => t.title.includes(searchText)).map((task, i) =>
                            <TaskItem key={task.id} task={task} />)
                    }
                </STaskListItems>
            </STaskListWrapper>)
    }

    const handleTitleChange = (e: any) => {
        setTitle(e.target.value)
    }

    const handleTitleKeyDown = (e: any) => {
        if (e.key === 'Enter')
            e.target.blur()
    }

    const handleTitleBlur = () => {
        if (activeTaskList === null)
            return

        dispatch(renameTaskList(activeTaskList.id, title))
    }

    const handleLeftColumnToggle = () => {
        dispatch(toggleLeftColumn())
    }

    if (activeTaskList === null)
        return <span>Список не найден</span>

    return (
        <Routes>
            <Route path={activeTaskList.id} element={
                <STaskListWrapper>
                    <STaskListTitleWrapper>
                        {!isLeftColumnActive &&
                            <SIconButtonFilled onClick={handleLeftColumnToggle}>
                                <i className="fa-solid fa-bars" />
                            </SIconButtonFilled>
                        }
                        <STaskListTitle>
                            {activeTaskList.isSmart
                                ? <span>{title}</span>
                                : <STaskListTitleInput spellCheck={false}
                                    value={title}
                                    onKeyDown={handleTitleKeyDown} onChange={handleTitleChange}
                                    onBlur={handleTitleBlur} />
                            }
                        </STaskListTitle>
                    </STaskListTitleWrapper>
                    {/* todo change for smart lists */}
                    {!activeTaskList.isSmart && <AddTask />}
                    <STaskListItems>
                        {
                            tasks.filter(t => activeTaskList?.isSmart ? activeTaskList.filter(t) : t.toDoListId === activeTaskList.id).map((task, i) =>
                                <TaskItem key={task.id} task={task} />)
                        }
                    </STaskListItems>
                </STaskListWrapper>
            } />
            <Route path='*' element={<span>Список не найден</span>} />
        </Routes>
    )
}

export default TaskList

const STaskListTitleWrapper = styled.div`
    margin: 0 8px;
    display: flex;
    align-items: center;
    flex-shrink: 0;
`

const STaskListTitle = styled.div`
    margin-left: 8px;
    width: 100%;
    font-weight: bold;
    font-size: 2rem;
    border: 1px solid transparent;
    color: ${p => p.theme.colors.primary};
    >span{
        padding-left: 4px;
    }
`

const STaskListTitleInput = styled.input`
    padding: inherit;
    width: inherit;
    font-weight: inherit;
    font-size: inherit;
    border: inherit;
    color: inherit;
    padding-left: 4px;
    overflow: hidden;
    text-overflow: ellipsis;
    :focus{
        border: 1px solid ${p => p.theme.colors.onSurface};
    }
    :hover{
        background-color: ${p => p.theme.colors.surfaceHover};
    }
`

const STaskListWrapper = styled.div`
    height: 100%;
    display: flex;
    flex-direction: column;
`

const STaskListItems = styled.div`
    margin: 8px;
    overflow-y: auto;
    overflow-x: hidden;
`