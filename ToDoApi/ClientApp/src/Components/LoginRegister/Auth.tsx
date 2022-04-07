import React, { useEffect } from 'react';
import styled from "styled-components";
import Login from "./Login";
import Registration from "./Registration";
import { SInput, SPanel } from 'src/Components/UI'
import { Navigate, Route, Routes, useNavigate } from 'react-router-dom';
import { ThemeButton } from '../UI/ThemeButton';
import ExternalProviderRegistration from './ExternalProviderRegistration';
import { useAppSelector } from 'src/redux/hooks';
import { useLocation } from 'react-router-dom';

const Auth: React.FC = () => {
    const navigate = useNavigate()
    const externalLoginPayload = useAppSelector(state => state.user.externalLoginPayload)
    const isLogging = useAppSelector(state => state.auth.isLogging)
    const isRegistering = useAppSelector(state => state.user.isRegistering)
    const location = useLocation()

    useEffect(() => {
        if (externalLoginPayload) {
            navigate("/auth/external")
        }
        else if (location.pathname === "/auth/external") {
            navigate("/auth/login")
        }
    }, [externalLoginPayload])

    if (isRegistering) {
        return (
            <SPanel>
                <STitle>Registering</STitle>
            </SPanel>
        )
    }

    if (isLogging) {
        return (
            <SPanel>
                <STitle>Logging</STitle>
            </SPanel>
        )
    }

    return (
        <AuthLayout>
            <SThemeButtonWrapper>
                <ThemeButton />
            </SThemeButtonWrapper>
            <SPanelsWrapper>
                <Routes>
                    <Route path="login" element={<Login />} />
                    <Route path="registration" element={<Registration />} />
                    <Route path="external" element={<ExternalProviderRegistration />} />
                    <Route path="*" element={<Navigate to={'/login'} />} />
                </Routes>
            </SPanelsWrapper>
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

export const AuthForm = styled.form`
    width: 260px;
    display: flex;
    flex-direction: column;
    padding: ${props => props.theme.padding.small}px;
`

export const SAdditionalPanel = styled(SPanel)`
    text-align: center;
    >a{
        color: ${({ theme }) => theme.colors.onSurface};
    }
`

export const SPanelsWrapper = styled.div`
`

export const SSeparator = styled.div`
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

export const SInputFieldErrors = styled.div`
    color: ${p => p.theme.colors.error};
    margin-bottom: 4px;
`

export const SSummary = styled.div`
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
