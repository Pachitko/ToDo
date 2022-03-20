import React, { BaseSyntheticEvent, FC, useState } from 'react';
import styled from 'styled-components';
import { useNavigate } from 'react-router-dom';
import { SButton } from 'src/Components/UI'
import { SAuthInput, STitle } from './Auth';

const SSeparator = styled.div`
    flex-grow: 1;
`

const Register: FC = () => {
    const [email, setEmail] = useState("");
    const [username, setUsername] = useState("");
    const [password, setPassword] = useState("");
    const [passwordConfirmation, setPasswordConfirmation] = useState("");
    const navigate = useNavigate();

    const handleLogin = (e: BaseSyntheticEvent) => {
        e.preventDefault();
    }

    return (
        <>
            <STitle>
                REGISTRATION
            </STitle>
            <SAuthInput placeholder='E-mail' type={"email"} onChange={e => setEmail(e.target.value)} />
            <SAuthInput placeholder='Username' type={"text"} onChange={e => setUsername(e.target.value)} />
            <SAuthInput placeholder='Password' type={"password"} onChange={e => setPassword(e.target.value)} />
            <SAuthInput placeholder='Password confirmation' type={"password"} onChange={e => setPasswordConfirmation(e.target.value)} />
            <SSeparator></SSeparator>
            <SButton type='submit' onClick={handleLogin}>Register</SButton>
        </>
    );
}

export default Register;