import React, { useState } from 'react'
import styled from 'styled-components'
import { toggleTheme } from 'src/redux/actions/globalActions'
import { useAppDispatch, useAppSelector } from 'src/redux/hooks'
import BackScreen from 'src/Components/UI/BackScreen';
import { SIconButtonFilled, SPanel } from 'src/Components/UI';
import { logout } from 'src/redux/actions/userActions';

const HeaderButtonGroup: React.FC = () => {
    const dispatch = useAppDispatch();
    const [isUserPanelActive, setIsUserPanelActive] = useState(false)
    const user = useAppSelector(state => state.user)

    const handleThemeToggle = () => {
        dispatch(toggleTheme())
    }

    const handleLogout = () => {
        dispatch(logout())
    }

    const handleUserClick = () => {
        setIsUserPanelActive(!isUserPanelActive)
    }

    return (<SHeaderButtonGroup>
        <SMenuBtnWrapper>
            <SMenuBtn onClick={handleUserClick}>
                <i className="fa-solid fa-user"></i>
            </SMenuBtn>
        </SMenuBtnWrapper>
        {isUserPanelActive && <>
            <BackScreen onClick={handleUserClick} />
            <SUserPanel>
                <SUserButtonWrapper>
                    <SUserButton onClick={handleThemeToggle}>
                        <i className="fa fa-palette"></i>
                    </SUserButton>
                    <SUserButton onClick={handleLogout}>
                        <i className="fa-solid fa-arrow-right-from-bracket"></i>
                    </SUserButton>
                </SUserButtonWrapper>
                <SUserInfoWrapper>
                    <SUserInfoFieldWrapper>
                        <SUserName>{user.name}</SUserName>
                    </SUserInfoFieldWrapper>
                    <SUserInfoFieldWrapper>
                        <span>{user.email}</span>
                    </SUserInfoFieldWrapper>
                </SUserInfoWrapper>
            </SUserPanel>
        </>
        }
    </SHeaderButtonGroup >
    )
}

export default HeaderButtonGroup

const SUserPanel = styled(SPanel)`
    padding: 0;
    position: absolute;
    box-shadow: ${({ theme }) => theme.shadow.light.soft};
    z-index: ${p => p.theme.zIndices.contextMenu};
    background-color: ${p => p.theme.colors.surface};
    display: flex;
    justify-content: right;
    top: 100%;
    right: 0;
    min-width: 200px;
`

const SUserButtonWrapper = styled.div`
    padding: 4px;
    display: flex;
    justify-content: right;
    border-bottom: 1px solid ${p => p.theme.colors.surfaceHover};
`

const SUserButton = styled(SIconButtonFilled)`
    padding: 4px;
    display: flex;
    align-items: center;
    justify-content: center;
    margin-left: 8px;
    &>i {
        font-size: 1rem;
    }
`
const SUserInfoWrapper = styled.div`
    user-select: none;
    padding: 4px;
    color: ${p => p.theme.colors.onSurface}
`

const SUserName = styled.span`
    font-weight: bold;
`

const SUserInfoFieldWrapper = styled.div`
    text-align: right;
`

// Header
const SHeaderButtonGroup = styled.div`
    display: flex;
    flex-direction: row;
    height: 100%;
`

const SMenuBtnWrapper = styled.div`
    position: relative;
    width: 48px;
    height: 100%;
`
const SMenuBtn = styled.button`
    height: 100%;
    box-sizing: border-box;
    width: 100%;
    transition: background-color .25s ease;
    cursor: pointer;
    &:hover{
        background-color: ${p => p.theme.colors.primaryDark};
    }
    &>i{
        color: white;
        font-size: 24px;
    }
`