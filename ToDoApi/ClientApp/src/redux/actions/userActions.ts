import { AppDispatch, AppState } from "../store"
import { REGISTRATION_SUCCESS, REGISTERING, REGISTRATION_ERROR, SET_IDENTITY } from "./actionTypes"
import { registerUserAsync, registerWithExternalProviderAsync, getMeAsync } from "src/libs/api"
import { ICreatedUser, IExternalLoginPayload, IUserToRegister } from "src/libs/abstractions"
import { IIdentity } from "../reducers/auth"

export const registerUser = (userToCreate: IUserToRegister) => {
    return internalRegistration(() => registerUserAsync(userToCreate))
}

export const registerWithExternalProvider = (externalLoginPayload: IExternalLoginPayload) => {
    return internalRegistration(() => registerWithExternalProviderAsync(externalLoginPayload))
}

const internalRegistration = (registerUser: () => Promise<ICreatedUser>) => {
    return async (state: AppState, dispatch: AppDispatch) => {
        dispatch(registering())
        try {
            const newUser = await registerUser()
            dispatch(registratoinSuccess())
        } catch (error) {
            dispatch(registrationError(error))
        }
    }
}

export const setIdentity = (token: string | null) => {
    return async (state: AppState, dispatch: AppDispatch) => {
        try {
            const currentUser = await getMeAsync(token)
            dispatch(setIdentityAction(currentUser))
        } catch (error) {
            throw error
        }
    }
}

export const registering = () => {
    return {
        type: REGISTERING,
    }
}

export const registratoinSuccess = () => {
    return {
        type: REGISTRATION_SUCCESS
    }
}

export const registrationError = (error: any) => {
    return {
        type: REGISTRATION_ERROR,
        payload:
        {
            error
        }
    }
}

export const setIdentityAction = (identity: IIdentity | null) => {
    return {
        type: SET_IDENTITY,
        payload: {
            identity
        }
    }
}