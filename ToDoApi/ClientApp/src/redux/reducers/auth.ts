import { LOGGING, LOGIN_SUCCESS, LOGOUT, LOGIN_ERROR, SET_IDENTITY } from "src/redux/actions/actionTypes";

export interface IIdentity {
    username: string,
    email: string,
}

export interface IAuthState {
    token: string | null,
    isLogging: boolean,
    isAuthenticated: boolean,
    loginError: Error | null,
    identity: IIdentity | null
}

const initialState: IAuthState = {
    token: null,
    isLogging: false,
    isAuthenticated: false,
    loginError: null,
    identity: null
};

const auth = (state = initialState, action: any): IAuthState => {
    switch (action.type) {
        case LOGGING: {
            console.log(LOGGING);
            return { ...state, isLogging: true }
        }
        case LOGIN_SUCCESS: {
            const { token } = action.payload;
            console.log(`${LOGIN_SUCCESS}:`, token);
            localStorage.setItem("token", token === null ? '' : token)
            return { ...state, token, isLogging: false, loginError: null }
        }
        case LOGIN_ERROR: {
            const error = action.payload.error as Error;
            console.log(LOGIN_ERROR, error);
            return { ...state, isLogging: false, loginError: error }
        }
        case LOGOUT: {
            localStorage.removeItem("token")
            return initialState;
        }
        case SET_IDENTITY: {
            const identity = action.payload.identity;
            console.log(`${SET_IDENTITY}:`, identity);
            return { ...state, isLogging: false, isAuthenticated: true, identity }
        }
        default:
            return state;
    }
};

export default auth;