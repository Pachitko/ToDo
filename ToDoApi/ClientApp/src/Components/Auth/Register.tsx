import React, { BaseSyntheticEvent, FC, useState } from 'react';
import styled from 'styled-components';
import { useNavigate } from 'react-router-dom';
import { SButton } from 'src/Components/UI'
import { SAuthInput, STitle } from './Auth';
import { registerUserAsync } from 'src/redux/actions/userActions';
import { useDispatch } from 'react-redux';
import { IUserToRegister } from 'src/libs/api';

const SSeparator = styled.div`
    flex-grow: 1;
`

const Register: FC = () => {
    const dispatch = useDispatch();
    const [email, setEmail] = useState("");
    const [username, setUsername] = useState("");
    const [password, setPassword] = useState("");
    const [passwordConfirmation, setPasswordConfirmation] = useState("");

    const handleLogin = (e: BaseSyntheticEvent) => {
        e.preventDefault();
        const userToRegister: IUserToRegister = {
            email,
            username,
            password,
            passwordConfirmation
        }

        dispatch(registerUserAsync(userToRegister))
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