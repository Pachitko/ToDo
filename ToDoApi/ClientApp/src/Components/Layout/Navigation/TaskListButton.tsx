import React from 'react'
import { NavLink } from 'react-router-dom'
import styled, { css } from 'styled-components'
import { deleteTaskListAsync, selectTaskListAction } from 'src/redux/actions/taskActions'
import { useDispatch } from 'react-redux'
import { SIconButton } from 'src/Components/UI'
import { useAppSelector } from 'src/redux/hooks'
import { ITaskList } from 'src/redux/reducers/tasks'

type Props = {
    list: ITaskList
}

const TaskListButton: React.FC<Props> = ({ list }) => {
    const dispatch = useDispatch()
    const activeList = useAppSelector(state => state.tasks.activeList)

    const handleListSelect = () => {
        if (activeList?.id !== list.id)
            dispatch(selectTaskListAction(list))
    }

    const handleListDelete = () => {
        dispatch(deleteTaskListAsync(list.id))
    }

    return (
        <STaskListBtnWrapper>
            <STaskListLink to={list.id}
                onClick={handleListSelect}>
                <SListTitle>{list.title}</SListTitle>
            </STaskListLink>
            <SNumberOfTask>
            </SNumberOfTask>
            {!list.isSmart ?
                <SIconButton onClick={handleListDelete}>
                    <i className="fa-solid fa-times"></i>
                </SIconButton>
                : null}
        </STaskListBtnWrapper >
    )
}

export default TaskListButton

const SNumberOfTask = styled.div`

`

const STaskListBtnWrapper = styled.div`
    display: flex;
    box-sizing: border-box;
    width: 100%;
    height: 32px;
    line-height: 32px;
    & &>.active{
        background-color: red;
    }
`

const taskListLinkActive = css`
    background-color: ${p => p.theme.colors.surfaceActive};

    &:hover{
        background-color: ${p => p.theme.colors.surfaceActive};
    }
`

const STaskListLink = styled(NavLink)`
    color: ${p => p.theme.colors.onSurface};
    padding-left: 16px;
    display: flex;
    height: 100%;
    flex-grow: 1;

    &:hover{
        background-color: ${p => p.theme.colors.surfaceHover};
    }
    &.active{
        ${taskListLinkActive}
    }
`

const SListTitle = styled.div`
    flex-grow: 1;
`