import React from 'react'
import { useAppSelector } from 'src/redux/hooks'
import styled from 'styled-components'
import TaskListButton from '../../Navigation/TaskListButton'
import AddList from './AddList'

const TaskLists = () => {
    const taskLists = useAppSelector(state => state.tasks.taskLists)
    const smartTaskLists = useAppSelector(state => state.tasks.smartTaskLists)
    return (
        <nav>
            <SSmartLists>
                {smartTaskLists.map(smartTaskList => smartTaskList.isVisible &&
                    <TaskListButton key={smartTaskList.id} list={smartTaskList} />
                )}
            </SSmartLists>
            <SUserDefinedLists>
                {
                    taskLists.map((list) =>
                        <TaskListButton key={list.id} list={list} />
                    )}
            </SUserDefinedLists>
            <AddList />
        </nav>
    )
}

export default TaskLists

const SSmartLists = styled.div`
    color: ${p => p.theme.colors.onSurface};
    margin-bottom: 16px;
`

const SUserDefinedLists = styled.div`
    margin-bottom: 16px;
`