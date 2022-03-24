import React from 'react'
import { SIconButtonFilled } from 'src/Components/UI'
import { toggleLeftColumn } from 'src/redux/actions/globalActions'
import { useAppDispatch, useAppSelector } from 'src/redux/hooks'
import styled from 'styled-components'
import TaskLists from './TaskLists'

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

const SLeftColumn = styled.div`
    border-right: 1px solid #ddd;
    background-color: ${p => p.theme.colors.surface};
    width: 256px;
    display: flex;
    flex-direction: column;
    @media ${p => p.theme.media.m} {
        position: absolute;
        top: 0;
        left: 0;
        bottom: 0;
        box-shadow: ${p => p.theme.shadow.light.soft};
    }
    @media ${p => p.theme.media.l} {
        width: 192px;
    }
`

const SLeftColumnHeader = styled.div`
padding: 8px;
`