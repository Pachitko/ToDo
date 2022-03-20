import React from 'react'
import styled from 'styled-components'
// import { Outlet } from 'react-router-dom'

const Main: React.FC = ({ children }) => (
    <SMain>
        {children}
        {/* <Outlet /> */}
    </SMain>
)

export default Main

const SMain = styled.div`
    height: 100%;
    display: flex;
    flex-direction: row;
    flex-grow: 1;
`