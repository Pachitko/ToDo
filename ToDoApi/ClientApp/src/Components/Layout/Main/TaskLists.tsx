import React from 'react'
import { useAppSelector } from 'src/redux/hooks'
import styled from 'styled-components'
import TaskListButton from '../Navigation/TaskListButton'
import AddList from './AddList'

const TaskLists = () => {
    const taskLists = useAppSelector(state => state.tasks.taskLists)
    return (
        <div>
            <SSmartLists>
                Smart lists
            </SSmartLists>
            <SUserDefinedLists>
                {
                    taskLists.map((list) => (
                        <TaskListButton listTitle={list.title} key={list.id} listId={list.id} isUserDefined />
                    ))
                }
            </SUserDefinedLists>
            <AddList />
        </div>
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

const STaskListDelimiter = styled.div`
    margin: 16px 0;
`