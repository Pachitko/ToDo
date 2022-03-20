import { AppDispatch, AppState } from "../store"
import { LOGIN_SUCCESS, LOGOUT, LOGGING, LOGIN_ERROR, REGISTRATION_SUCCESS, REGISTERING, REGISTRATION_ERROR } from "./actionTypes"
import { loginAsync as loginApi, getMeAsync as getMeApi, IUserToRegister, registerUser } from "src/libs/api"
import { IUser } from "../reducers/user"

export const loginAsync = (username: string, password: string) => {
    return async (state: AppState, dispatch: AppDispatch) => {
        dispatch(logging())
        try {
            const token = await loginApi(username, password)
            const user = await getMeAsync(token)
            dispatch(loginSuccess(user))
        } catch (error) {
            dispatch(loginError(error))
        }
    }
}

export const loginWithTokenAsync = (token: string | null) => {
    return async (state: AppState, dispatch: AppDispatch) => {
        dispatch(logging())
        try {
            const user = await getMeAsync(token)
            dispatch(loginSuccess(user))
        } catch (error) {
            dispatch(loginError(error))
        }
    }
}

export const registerUserAsync = (userToCreate: IUserToRegister) => {
    return async (state: AppState, dispatch: AppDispatch) => {
        dispatch(registering())
        try {
            const newUser = await registerUser(userToCreate)
            dispatch(registratoinSuccess())
        } catch (error) {
            dispatch(registrationError(error))
        }
    }
}

export const getMeAsync = async (token: string | null): Promise<IUser | null> => {
    return await getMeApi(token)
}

export const logging = () => {
    return {
        type: LOGGING,
    }
}

export const registering = () => {
    return {
        type: REGISTERING,
    }
}

export const loginSuccess = (user: IUser | null) => {
    return {
        type: LOGIN_SUCCESS,
        payload:
        {
            user
        }
    }
}

export const registratoinSuccess = () => {
    return {
        type: REGISTRATION_SUCCESS
    }
}

export const loginError = (error: string) => {
    return {
        type: LOGIN_ERROR,
        payload:
        {
            error
        }
    }
}

export const registrationError = (error: string) => {
    return {
        type: REGISTRATION_ERROR,
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