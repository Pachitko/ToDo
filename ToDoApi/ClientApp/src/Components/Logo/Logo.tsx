import React from 'react';
import styled from 'styled-components'
import { NavLink } from 'react-router-dom'

type LogoProps = {
    to: string
}

const SLogo = styled.div`
    margin: 0 16px;
    display: flex;
    justify-content: center;
    align-items: center;
    &>a{
        display: block;
        font-size: 24px;
        font-weight: bold;
        color:  ${p => p.theme.colors.white};
        &:hover{
            text-decoration: underline;
        }
    }
`

const Logo: React.FC<LogoProps> = ({ to }) => (
    <SLogo>
        <NavLink to={to}>
            To&nbsp;Do
        </NavLink>
    </SLogo>
)

export default Logo 