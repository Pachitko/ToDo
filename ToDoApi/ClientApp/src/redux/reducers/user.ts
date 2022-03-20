import { LOGIN_SUCCESS, LOGOUT, LOGGING, LOGIN_ERROR, REGISTRATION_SUCCESS } from "src/redux/actions/actionTypes";

export interface IUserState extends IUser {
    isLogging: boolean,
}

export interface IUser {
    name: string,
    email: string,
    token: string | null,
}

const initialState: IUserState = {
    name: '',
    email: '',
    token: null,
    isLogging: false
};

const user = (state = initialState, action: any): IUserState => {
    switch (action.type) {
        case LOGGING: {
            console.log(LOGGING);
            return { ...state, isLogging: true }
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
            return { ...state, isLogging: false }
        }
        case LOGOUT: {
            localStorage.removeItem("token")
            return initialState;
        }
        case REGISTRATION_SUCCESS: {
            console.log(REGISTRATION_SUCCESS);
            return initialState;
        }
        default:
            return state;
    }
};

export default user;