import React from 'react'
import styled from 'styled-components'
import { Outlet } from 'react-router-dom'

const CenterColumn: React.FC = ({ children }) => (
    <SCenterColumn>
        {children}
        <Outlet />
    </SCenterColumn>
)

export default CenterColumn

const SCenterColumn = styled.div`
    flex-grow: 1;
    background-color: ${p => p.theme.colors.background};
`