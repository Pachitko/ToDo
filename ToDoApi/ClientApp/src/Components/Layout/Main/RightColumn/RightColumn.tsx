import React from 'react'
import styled from 'styled-components'
import { useAppSelector } from 'src/redux/hooks'

interface Props {
    isActive: boolean
}

const RightColumn: React.FC = ({ children }) => {
    const activeTask = useAppSelector(state => state.tasks.activeTask)

    return (
        <SRightColumn isActive={activeTask !== null}>
            {children}
        </SRightColumn>
    )
}

export default RightColumn

const SRightColumn = styled.div<Props>`
    display: ${({ isActive }) => isActive ? "flex" : "none"};
    position: relative;
    flex-direction: column;
    width: 256px;
    background-color: ${p => p.theme.colors.surface};
    padding: 8px;
    border-left: 1px solid #ddd;
    @media ${p => p.theme.media.m} {
        position: absolute;
        top: 0;
        right: 0;
        bottom: 0;
        box-shadow: ${p => p.theme.shadow.light.soft};
    }
`