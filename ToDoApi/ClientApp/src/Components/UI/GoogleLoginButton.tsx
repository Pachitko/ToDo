import React, { FC } from 'react';
import { loginWithExternalProvider } from 'src/redux/actions/authActions';
import { useDispatch } from 'react-redux';
import { GoogleLogin, GoogleLoginResponse, GoogleLoginResponseOffline } from 'react-google-login';
import { googleConfig } from 'src/libs/googleConfig';
import styled from 'styled-components';

const GoogleLoginButton: FC = () => {
    const dispatch = useDispatch()

    const handleGoogleSuccess = async (googleResponse: GoogleLoginResponse | GoogleLoginResponseOffline) => {
        dispatch(loginWithExternalProvider("Google", (googleResponse as GoogleLoginResponse).tokenId))
    }

    const handleGoogleFailure = (error: any) => {
        console.log("Google failure:", error);
    }

    return (
        <SGoogleLogin
            clientId={googleConfig.clientId}
            buttonText="Continue with Google"
            onSuccess={handleGoogleSuccess}
            onFailure={handleGoogleFailure}
        >
        </SGoogleLogin>
    )
}

export default GoogleLoginButton

const SGoogleLogin = styled(GoogleLogin)`
    display: flex;
    margin-top: 16px;
`