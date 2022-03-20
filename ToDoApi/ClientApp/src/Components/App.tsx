import React, { useEffect } from "react";
import { Routes, Route, Navigate } from "react-router-dom";
import Layout from './Layout/Layout'
import { Header } from './Layout/Header'
import {
    Main, LeftColumn, TaskLists, CenterColumn, TaskList, RightColumn,
    RightColumnBottomPanel, TaskDetails
} from './Layout/Main'
import { useAppSelector } from "src/redux/hooks";
import { ThemeProvider } from "styled-components";
import { Auth } from './Auth'
import { loginWithTokenAsync } from "src/redux/actions/userActions";
import { useDispatch } from "react-redux";
import { loadThemeFromStorage } from "src/redux/actions/globalActions";

const App = () => {
    const activeTheme = useAppSelector(state => state.global.theme)

    const dispath = useDispatch();
    const isLogging = useAppSelector(state => state.user.isLogging)
    const token = useAppSelector(state => state.user.token)
    const tokenFromLocalStorage = localStorage.getItem("token");

    if (isLogging) {
        return <div>Logging</div>
    }

    if (tokenFromLocalStorage && !token) {
        dispath(loginWithTokenAsync(tokenFromLocalStorage))
    }

    dispath(loadThemeFromStorage())

    return <ThemeProvider theme={activeTheme}>
        {!token ?
            <Routes>
                <Route path="*" element={<Auth />} />
            </Routes>
            :
            <Layout>
                <Header />
                <Routes>
                    <Route path="*" element={<Navigate to={'/tasks/today'} />} />
                    <Route path='/tasks/*' element={
                        <Main>
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
                        </Main>
                    } />
                </Routes>
            </Layout >
        }
    </ThemeProvider>
}

export default (App);