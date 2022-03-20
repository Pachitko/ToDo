import React from 'react'
import styled from 'styled-components'
import { HeaderButtonGroup, SearchTool } from './index'
import Logo from '../../Logo/Logo'

const SHeader = styled.header`
    height: 48px;
    display: flex;
    justify-content: space-between;
    align-items: center;
    top: 0;
    position: sticky;
    background-color: ${p => p.theme.colors.primary};
    z-index: 49;
    border-bottom: 2px solid ${p => p.theme.colors.primaryDark};
`

const Header: React.FC = () => {
    return (
        <SHeader>
            <Logo to={"tasks/today"} />
            <SearchTool />
            <HeaderButtonGroup />
        </SHeader >
    )
}

export default Header;