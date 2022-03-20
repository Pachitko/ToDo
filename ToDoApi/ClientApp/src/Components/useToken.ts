import { useState } from 'react';
import { IUserToken } from 'src/libs/api';

export default function useToken() {
    const getToken = () => {
        const token = localStorage.getItem('token');
        return token
    };

    const saveToken = (token: IUserToken) => {
        localStorage.setItem("token", token === null ? '' : token)
        setToken(token);
    };

    const [token, setToken] = useState(getToken());

    return {
        token,
        setToken: saveToken,
    }
}