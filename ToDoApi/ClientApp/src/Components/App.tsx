import React, { useEffect } from "react";
import { Routes, Route, Navigate } from "react-router-dom";
import { Header } from './Layout/Header'
import {
    LeftColumn, CenterColumn, TaskList, RightColumn,
    RightColumnBottomPanel, TaskDetails
} from './Layout/Main'
import { useAppSelector } from "src/redux/hooks";
import styled, { ThemeProvider } from "styled-components";
import { Auth } from './LoginRegister'
import { verifyToken } from "src/redux/actions/authActions";
import { setIdentity } from "src/redux/actions/userActions";
import { useDispatch } from "react-redux";
import { loadThemeFromStorage } from "src/redux/actions/globalActions";

let tryAutoLogin = true;

const App = () => {
    const dispatch = useDispatch();
    const activeTheme = useAppSelector(state => state.global.theme)
    const isAuthenticated = useAppSelector(state => state.auth.isAuthenticated)
    const token = useAppSelector(state => state.auth.token)

    dispatch(loadThemeFromStorage())

    const tokenFromLocalStorage = localStorage.getItem("token");
    if (tryAutoLogin && tokenFromLocalStorage && !isAuthenticated) {
        tryAutoLogin = false;
        dispatch(verifyToken(tokenFromLocalStorage))
    }

    useEffect(() => {
        if (!isAuthenticated && token) {
            dispatch(setIdentity(token))
        }
    }, [token])

    return <ThemeProvider theme={activeTheme}>
        {isAuthenticated
            ? <SLayout>
                <Header />
                <Routes>
                    <Route path="*" element={<Navigate to={'/tasks/all'} />} />
                    <Route path='/tasks/*' element={
                        <SMain>
                            <LeftColumn />
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
            : <Routes>
                <Route path="auth/*" element={<Auth />} />
                <Route path="*" element={<Navigate to={'auth/login'} />} />
            </Routes>
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
    position: relative;
    flex-grow: 1;
    display: flex;
    flex-direction: row;
    overflow: hidden;
`