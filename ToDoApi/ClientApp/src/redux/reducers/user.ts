import { IExternalLoginPayload } from "src/libs/abstractions";
import { REGISTRATION_SUCCESS, REGISTRATION_ERROR, REGISTERING, CONFIRM_EXTERNAL_REGISTRATION } from "src/redux/actions/actionTypes";

export interface IUserState {
    isRegistering: boolean,
    registrationErrors: {
        [key: string]: string[]
    },
    externalLoginPayload: IExternalLoginPayload | null
}

const initialState: IUserState = {
    isRegistering: false,
    registrationErrors: {},
    externalLoginPayload: null
};

const user = (state = initialState, action: any): IUserState => {
    switch (action.type) {
        case REGISTERING: {
            return { ...state, isRegistering: true, registrationErrors: {} }
        }
        case REGISTRATION_SUCCESS: {
            return { ...state, isRegistering: false, externalLoginPayload: null, registrationErrors: {} }
        }
        case REGISTRATION_ERROR: {
            const { response } = action.payload.error;
            return { ...state, isRegistering: false, registrationErrors: response.data.errors }
        }
        case CONFIRM_EXTERNAL_REGISTRATION: {
            const externalLoginPayload: IExternalLoginPayload = action.payload.externalLoginPayload
            return { ...state, externalLoginPayload }
        }
        default:
            return state;
    }
};

export default user;