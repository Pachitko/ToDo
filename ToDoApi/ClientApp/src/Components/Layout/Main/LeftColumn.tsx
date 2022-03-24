import React from 'react'
import { SIconButtonFilled } from 'src/Components/UI'
import { toggleLeftColumn } from 'src/redux/actions/globalActions'
import { useAppDispatch, useAppSelector } from 'src/redux/hooks'
import styled from 'styled-components'
import TaskLists from './TaskLists'

const SLeftColumn = styled.div`
    background-color: ${p => p.theme.colors.surface};
    min-width: 224px;
    display: flex;
    flex-direction: column;
`

const LeftColumn: React.FC = () => {
    const isLeftColumnActive = useAppSelector(state => state.global.isLeftColumnActive)
    const dispatch = useAppDispatch()

    const handleLeftColumnToggle = () => {
        dispatch(toggleLeftColumn())
    }

    return isLeftColumnActive ?
        <SLeftColumn>
            <SLeftColumnHeader>
                <SIconButtonFilled onClick={handleLeftColumnToggle}>
                    <i className="fa-solid fa-bars" />
                </SIconButtonFilled>
            </SLeftColumnHeader >
            <TaskLists />
        </SLeftColumn >
        : null
}

export default LeftColumn

const SLeftColumnHeader = styled.div`
    padding: 8px;
`