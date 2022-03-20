import React from 'react';
import styled from "styled-components";
import Login from "./Login";
import Register from "./Register";
import { SInput, SPanel } from 'src/Components/UI'
import { Navigate, NavLink, Route, Routes } from 'react-router-dom';
import { ThemeButton } from '../UI/ThemeButton';

const Auth: React.FC = () => {
    return (
        <AuthLayout>
            <SPanelsWrapper>
                <SPanel>
                    <AuthForm>
                        <Routes>
                            {/* <Route index element={<Login />} /> */}
                            <Route path='/login' element={<Login />} />
                            <Route path='/register' element={<Register />} />
                            <Route path='*' element={<Navigate to={'/login'} />} />
                        </Routes>
                    </AuthForm>
                </SPanel>
                <SSeparator />
                <SAdditionalPanel>
                    <Routes>
                        <Route path='/login' element={<NavLink to={"/register"}>Register</NavLink>} />
                        <Route path='/register' element={<NavLink to={"/login"}>Login</NavLink>} />
                    </Routes>
                </SAdditionalPanel>
            </SPanelsWrapper>
            <SThemeButtonWrapper>
                <ThemeButton />
            </SThemeButtonWrapper>
        </AuthLayout>
    );
}

export default Auth;

const SThemeButtonWrapper = styled.div`
    position: fixed;
    top: 0;
    right: 0;
`

const AuthLayout = styled.div`
    background-color: ${({ theme }) => theme.colors.background};
    height: 100%;
    display: flex;
    flex-direction: column;
    align-items: center;
    justify-content: center;
    &>div{
        margin-bottom: 16px;
    }
`

const AuthForm = styled.form`
    width: 260px;
    display: flex;
    flex-direction: column;
    padding: ${props => props.theme.padding.small}px;
`

const SAdditionalPanel = styled(SPanel)`
    text-align: center;
    >a{
        color: ${({ theme }) => theme.colors.onSurface};
    }
`

const SPanelsWrapper = styled.div`
`

const SSeparator = styled.div`
    height: 16px;
`

export const SAuthInput = styled(SInput) <{ isValid: boolean }>`
    width: 100%;
    border-color: ${p => !p.isValid && p.theme.colors.error};
    padding: 8px;
`

export const SAuthInputWrapper = styled.div`
    margin-bottom: 16px;
`

export const SAuthError = styled.div`
    color: ${p => p.theme.colors.error};
    margin-bottom: 4px;
`

export const STitle = styled.div`
    color: ${({ theme }) => theme.colors.onSurface};
    font-weight: bold;
    font-size: 1.25rem;
    text-align: center;
    padding: 4px 8px;
`
