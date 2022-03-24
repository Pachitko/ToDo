import React from 'react'
import styled from 'styled-components'
import { HeaderButtonGroup, SearchTool } from './index'
import Logo from '../../Logo/Logo'

const Header: React.FC = () => {
    return (
        <SHeader>
            <Logo to={"tasks/all"} />
            <SearchTool />
            <HeaderButtonGroup />
        </SHeader >
    )
}

export default Header;

const SHeader = styled.header`
    flex-shrink: 0;
    height: 48px;
    display: flex;
    justify-content: space-between;
    align-items: center;
    background-color: ${p => p.theme.colors.primary};
    z-index: 49;
`