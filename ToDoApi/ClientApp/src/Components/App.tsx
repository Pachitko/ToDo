import React from "react";
import { Routes, Route, Navigate } from "react-router-dom";
import { Header } from './Layout/Header'
import {
    LeftColumn, TaskLists, CenterColumn, TaskList, RightColumn,
    RightColumnBottomPanel, TaskDetails
} from './Layout/Main'
import { useAppSelector } from "src/redux/hooks";
import styled, { ThemeProvider } from "styled-components";
import { Auth } from './LoginRegister'
import { loginWithTokenAsync } from "src/redux/actions/userActions";
import { useDispatch } from "react-redux";
import { loadThemeFromStorage } from "src/redux/actions/globalActions";

let tryAutoLogin = true;

const App = () => {
    const activeTheme = useAppSelector(state => state.global.theme)
    const dispatch = useDispatch();
    const token = useAppSelector(state => state.user.token)
    const tokenFromLocalStorage = localStorage.getItem("token");

    if (tryAutoLogin && tokenFromLocalStorage && !token) {
        tryAutoLogin = false;
        dispatch(loginWithTokenAsync(tokenFromLocalStorage))
    }

    dispatch(loadThemeFromStorage())

    return <ThemeProvider theme={activeTheme}>
        {!token ?
            <Routes>
                <Route path="*" element={<Auth />} />
            </Routes>
            :
            <SLayout>
                <Header />
                <Routes>
                    <Route path="*" element={<Navigate to={'/tasks/all'} />} />
                    <Route path='/tasks/*' element={
                        <SMain>
                            <LeftColumn>
                                <TaskLists />
                            </LeftColumn>
                            <CenterColumn>
                                <TaskList />
                            </CenterColumn>
                            <RightColumn>
                                <TaskDetails />
                                <RightColumnBottomPanel />
                            </RightColumn>
                        </SMain>
                    } />
                </Routes>
            </SLayout >
        }
    </ThemeProvider>
}

export default (App);

const SLayout = styled.div`
    height: 100%;
    display: flex;
    flex-direction: column;
`

const SMain = styled.div`
    flex-grow: 1;
    display: flex;
    flex-direction: row;
    overflow: hidden;
`