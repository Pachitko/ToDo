import React, { useState } from 'react'
import styled, { css } from 'styled-components'
import { useAppDispatch, useAppSelector } from 'src/redux/hooks'
import { SIconButtonFilled } from 'src/Components/UI';
import { logout } from 'src/redux/actions/authActions';
import { ThemeButton } from 'src/Components/UI/ThemeButton';
import ContextMenu from 'src/Components/UI/ContextMenu';

const HeaderButtonGroup: React.FC = () => {
    const [isUserPanelActive, setIsUserPanelActive] = useState(false)
    const dispatch = useAppDispatch();
    const identity = useAppSelector(state => state.auth.identity)

    const handleLogout = () => {
        dispatch(logout())
    }

    const handleUserClick = () => {
        setIsUserPanelActive(!isUserPanelActive)
    }

    return (
        <SHeaderButtonGroup>
            <SMenuBtnWrapper>
                <SMenuBtn onClick={() => document.location = "https://github.com/Pachitko/ToDo"}>
                    <i className="fa-brands fa-github"></i>
                </SMenuBtn>
            </SMenuBtnWrapper>
            <SMenuBtnWrapper>
                <SMenuBtn onClick={handleUserClick}>
                    <i className="fa-solid fa-user"></i>
                </SMenuBtn>
            </SMenuBtnWrapper>
            {isUserPanelActive &&
                <ContextMenu onClickOutside={handleUserClick} ctxMenuStyles={UserPanelStyle}>
                    <SUserButtonWrapper>
                        <ThemeButton />
                        <SUserButton onClick={handleLogout}>
                            <i className="fa-solid fa-arrow-right-from-bracket"></i>
                        </SUserButton>
                    </SUserButtonWrapper>
                    <SUserInfoWrapper>
                        <SUserInfoFieldWrapper>
                            <SUserName>{identity?.username}</SUserName>
                        </SUserInfoFieldWrapper>
                        <SUserInfoFieldWrapper>
                            <span>{identity?.email}</span>
                        </SUserInfoFieldWrapper>
                    </SUserInfoWrapper>
                </ContextMenu>
            }
        </SHeaderButtonGroup >
    )
}

export default HeaderButtonGroup

const UserPanelStyle = css`
    width: 200px;
    top: 100%;
    right: 0;
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
    position: relative;
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
        color: ${p => p.theme.colors.white};
        font-size: 1.5rem;
    }
`