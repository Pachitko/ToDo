import React from 'react';
import styled from "styled-components";
import Login from "./Login";
import Register from "./Register";
import { SInput, SPanel } from 'src/Components/UI'
import { Navigate, NavLink, Route, Routes } from 'react-router-dom';

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
        </AuthLayout>
    );
}

export default Auth;

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

export const SAuthInput = styled(SInput)`
    margin-bottom: 16px;
    padding: 8px;
`

export const STitle = styled.div`
    color: ${({ theme }) => theme.colors.onSurface};
    font-weight: bold;
    font-size: 1.25rem;
    text-align: center;
    padding: 4px 8px;
    margin-bottom: 15px;
`
