import React from 'react'
import { NavLink } from 'react-router-dom'
import styled, { css } from 'styled-components'
import { deleteTaskListAsync, selectTaskListAction } from 'src/redux/actions/taskActions'
import { useDispatch } from 'react-redux'
import { SIconButton } from 'src/Components/UI'
import { useAppSelector } from 'src/redux/hooks'

type Props = {
    listTitle: string,
    listId: string
    isUserDefined: boolean
}

const TaskListButton: React.FC<Props> = ({ listTitle, listId, isUserDefined }) => {
    const dispatch = useDispatch()
    const activeListId = useAppSelector(state => state.tasks.activeListId)

    const handleListSelect = () => {
        if (activeListId !== listId)
            dispatch(selectTaskListAction(listId))
    }

    const handleListDelete = () => {
        dispatch(deleteTaskListAsync(listId))
    }

    return (
        <STaskListBtnWrapper>
            <STaskListLink to={listId}
                onClick={handleListSelect}>
                <SListTitle>{listTitle}</SListTitle>
            </STaskListLink>
            {isUserDefined ?
                <SIconButton onClick={handleListDelete}>
                    <i className="fa-solid fa-times"></i>
                </SIconButton>
                : null}
        </STaskListBtnWrapper >
    )
}

export default TaskListButton

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