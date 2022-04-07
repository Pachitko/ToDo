import React from 'react'
import { NavLink } from 'react-router-dom'
import styled, { css } from 'styled-components'
import { deleteTaskList, selectTaskListAction } from 'src/redux/actions/taskActions'
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
        dispatch(deleteTaskList(list.id))
    }

    return (
        <STaskListBtnWrapper>
            <STaskListLink to={list.id}
                onClick={handleListSelect}>
                <SListBody>
                    <STaskListIcon>
                        <i className={list.iconClass ? list.iconClass : "fa-solid fa-list"}></i>
                    </STaskListIcon>
                    <SListTitle>
                        <div>
                            {list.title}
                        </div>
                    </SListTitle>
                </SListBody>
            </STaskListLink>
            <SNumberOfTask>
            </SNumberOfTask>
            {
                !list.isSmart ?
                    <SIconButtonClear onClick={handleListDelete}>
                        <i className="fa-solid fa-times"></i>
                    </SIconButtonClear>
                    : null
            }
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
`

const taskListLinkActive = css`
    background-color: ${p => p.theme.colors.surfaceActive};

    &:hover{
        background-color: ${p => p.theme.colors.surfaceActive};
    }
`

const STaskListLink = styled(NavLink)`
    color: ${p => p.theme.colors.onSurface};
    padding-left: 4px;
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

const SListBody = styled.div`
    flex-grow: 1;
    display: flex;
    >span{
        flex-grow: 1;
    }
`

const SListTitle = styled.div`
    overflow: hidden;
    text-overflow: ellipsis;
`

const SIconButtonClear = styled(SIconButton)`
    flex-shrink: 0;
`

const STaskListIcon = styled.div`
    display: flex;
    justify-content: center;
    align-items: center;
    width: 32px;
    flex-shrink: 0;
`
