import { LOGIN_SUCCESS, LOGOUT, LOGGING, LOGIN_ERROR, REGISTRATION_SUCCESS, REGISTRATION_ERROR, REGISTERING } from "src/redux/actions/actionTypes";

export interface IUserState extends IUser {
    isLogging: boolean,
    isRegistering: boolean,
    loginError: string,
    registrationErrors: {
        [key: string]: string[]
    }
}

export interface IUser {
    name: string,
    email: string,
    token: string | null,
}

const initialState: IUserState = {
    name: "",
    email: "",
    token: null,
    isLogging: false,
    isRegistering: false,
    loginError: "",
    registrationErrors: {}
};

const user = (state = initialState, action: any): IUserState => {
    switch (action.type) {
        case LOGGING: {
            console.log(LOGGING);
            return { ...state, isLogging: true }
        }
        case REGISTERING: {
            console.log(REGISTERING);
            return { ...state, isRegistering: true, registrationErrors: {} }
        }
        case LOGIN_SUCCESS: {
            console.log(LOGIN_SUCCESS);
            const { user } = action.payload;
            if (user === null)
                return state
            console.log(user);
            localStorage.setItem("token", user.token === null ? '' : user.token)
            return { ...state, ...user, isLogging: false }
        };
        case LOGIN_ERROR: {
            const { error } = action.payload;
            console.log(error);
            return { ...state, isLogging: false, loginError: error }
        }
        case LOGOUT: {
            localStorage.removeItem("token")
            return initialState;
        }
        case REGISTRATION_SUCCESS: {
            console.log(REGISTRATION_SUCCESS);
            return { ...state, isRegistering: false }
        }
        case REGISTRATION_ERROR: {
            console.log(REGISTRATION_ERROR);
            const { response } = action.payload.error;
            console.log(response);

            return { ...state, isRegistering: false, registrationErrors: response.data.errors }
        }
        default:
            return state;
    }
};

export default user;