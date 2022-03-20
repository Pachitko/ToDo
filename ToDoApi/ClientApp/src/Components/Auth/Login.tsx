import React, { BaseSyntheticEvent, FC, useState } from 'react';
import styled from 'styled-components';
import { SButton } from 'src/Components/UI'
import { loginAsync } from 'src/redux/actions/userActions';
import { useDispatch } from 'react-redux';
import { SAuthInput, STitle } from './Auth';

const SSeparator = styled.div`
    flex-grow: 1;
`

const Login: FC = () => {
    const dispatch = useDispatch();
    const [username, setUsername] = useState('')
    const [password, setPassword] = useState('')

    const handleLogin = (e: BaseSyntheticEvent) => {
        e.preventDefault();
        dispatch(loginAsync(username, password))
    }

    return (
        <>
            <STitle>
                LOGIN
            </STitle>
            <SAuthInput placeholder='Username' type={"text"} onChange={(e) => setUsername(e.target.value)} />
            <SAuthInput placeholder='Password' type={"password"} onChange={(e) => setPassword(e.target.value)} />
            <SSeparator></SSeparator>
            <SButton type='submit' onClick={handleLogin}>Log in</SButton>
        </>
    );
}

export default Login;