import React from 'react'
import styled from 'styled-components'
import { useAppSelector } from 'src/redux/hooks'

interface Props {
    isActive: boolean
}

const SRightColumn = styled.div<Props>`
    display: ${({ isActive }) => isActive ? "flex" : "none"};
    position: relative;
    flex-direction: column;
    min-width: 224px;
    background-color: ${p => p.theme.colors.surface};
    padding: 8px;
`

const RightColumn: React.FC = ({ children }) => {
    const activeTaskId = useAppSelector(state => state.tasks.activeTaskId)

    return (
        <SRightColumn isActive={activeTaskId !== ''}>
            {children}
        </SRightColumn>
    )
}

export default RightColumn