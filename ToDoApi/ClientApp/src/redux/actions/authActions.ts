import { AppDispatch, AppState } from "../store"
import { LOGIN_SUCCESS, LOGOUT, LOGGING, LOGIN_ERROR, CONFIRM_EXTERNAL_REGISTRATION } from './actionTypes'
import { loginAsync, loginWithExternalProviderAsync, verifyTokenAsync } from "src/libs/api"
import axios, { AxiosError } from "axios"
import { IExternalLoginPayload, ILoginResult } from "src/libs/abstractions"

export const loginWithExternalProvider = (providerName: string, tokenId: string) => {
    return internalLogin("Internal error", async () => await loginWithExternalProviderAsync(providerName, tokenId), (state, dispatch, error) => {
        if (axios.isAxiosError(error)) {
            const axiosError = error as AxiosError
            if (axiosError.response?.status == 404) {
                dispatch(confirmExternalRegistration({ providerName, username: "", tokenId }))
            }
        }
    })
}

export const login = (username: string, password: string) => {
    return internalLogin("Invalid email and/or password", async () => await loginAsync(username, password), () => { })
}

const internalLogin = (defaultErrorMessage: string,
    getLoginResult: () => Promise<ILoginResult>,
    onError: (state: AppState, dispatch: AppDispatch, e: any) => void) => {

    return async (state: AppState, dispatch: AppDispatch) => {
        dispatch(logging())
        try {
            const loginResult = await getLoginResult()
            if (loginResult.succeeded) {
                dispatch(loginSuccess(loginResult.token))
            }
            else {
                let errorMessage = defaultErrorMessage
                if (loginResult.isLockedOut)
                    errorMessage = "User is locked out"
                else if (loginResult.isNotAllowed)
                    errorMessage = "Email confirmation required"
                else if (loginResult.requiresTwoFactor)
                    errorMessage = "Requires two factor"
                dispatch(loginError(new Error(errorMessage)))
            }
        } catch (error) {
            dispatch(loginError(new Error(defaultErrorMessage)))
            onError(state, dispatch, error)
        }
    }
}

export const verifyToken = (token: string | null) => {
    return async (state: AppState, dispatch: AppDispatch) => {
        dispatch(logging())
        try {
            const succeeded = await verifyTokenAsync(token)
            if (succeeded) {
                dispatch(loginSuccess(token))
            }
            else {
                throw new Error("Token verification error");
            }
        } catch (error) {
            dispatch(loginError(error))
            throw error
        }
    }
}

export const logging = () => {
    return {
        type: LOGGING,
    }
}

export const loginError = (error: Error) => {
    return {
        type: LOGIN_ERROR,
        payload:
        {
            error
        }
    }
}

export const logout = () => {
    return {
        type: LOGOUT
    }
}

export const loginSuccess = (token: string | null) => {
    return {
        type: LOGIN_SUCCESS,
        payload:
        {
            token
        }
    }
}

export const confirmExternalRegistration = (externalLoginPayload: IExternalLoginPayload | null) => {
    return {
        type: CONFIRM_EXTERNAL_REGISTRATION,
        payload: {
            externalLoginPayload
        }
    }
}
