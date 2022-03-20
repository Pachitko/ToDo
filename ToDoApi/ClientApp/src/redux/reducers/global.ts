import { LOAD_THEME_FROM_STORAGE, LOGOUT, TOGGLE_THEME } from "src/redux/actions/actionTypes";
import { ITheme, ThemeType } from "src/styles/styled";
import { lightTheme, darkTheme } from 'src/styles/theme'

export interface IGlobalState {
    theme: ITheme,
    loading: boolean
}

const initialState: IGlobalState = {
    theme: lightTheme,
    loading: false
};

const searchTool = (state = initialState, action: any): IGlobalState => {
    switch (action.type) {
        case TOGGLE_THEME: {
            const newTheme = state.theme.type === ThemeType.light ? darkTheme : lightTheme;
            localStorage.setItem("themeType", newTheme.type)
            return {
                ...state,
                theme: newTheme
            };
        }
        case LOAD_THEME_FROM_STORAGE: {
            const themeType = localStorage.getItem("themeType")
            let newTheme = lightTheme;
            if (themeType === null) {
                localStorage.setItem("themeType", newTheme.type)
            }
            else {
                newTheme = themeType === ThemeType.dark ? darkTheme : lightTheme;
            }

            return {
                ...state,
                theme: newTheme
            };
        }
        case LOGOUT: {
            return initialState;
        }
        default:
            return state;
    }
};

export default searchTool;