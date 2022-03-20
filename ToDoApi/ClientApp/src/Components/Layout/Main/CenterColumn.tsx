import React from 'react'
import styled from 'styled-components'
import { Outlet } from 'react-router-dom'

const SCenterColumn = styled.div`
    flex-grow: 1;
    background-color: ${p => p.theme.colors.background};
    border-left: 1px solid #ddd;
    border-right: 1px solid #ddd;
`

const CenterColumn: React.FC = ({ children }) => (
    <SCenterColumn>
        {children}
        <Outlet />
    </SCenterColumn>
)

export default CenterColumn