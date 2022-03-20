import React from 'react'
import { useAppDispatch } from 'src/redux/hooks';
import styled from 'styled-components'
import { SIconButtonFilled } from './StyledElements';
import { toggleTheme } from 'src/redux/actions/globalActions'

export const ThemeButton = () => {
    const dispatch = useAppDispatch();

    const handleThemeToggle = () => {
        dispatch(toggleTheme())
    }

    return (
        <SThemeButton onClick={handleThemeToggle}>
            <i className="fa fa-palette"></i>
        </SThemeButton>
    )
}


const SThemeButton = styled(SIconButtonFilled)`
`