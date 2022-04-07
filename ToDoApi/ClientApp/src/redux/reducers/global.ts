import { TOGGLE_LEFT_COLUMN, LOAD_THEME_FROM_STORAGE, LOGOUT, TOGGLE_THEME } from "src/redux/actions/actionTypes";
import { ITheme, ThemeType } from "src/styles/styled";
import { lightTheme, darkTheme } from 'src/styles/theme'

export interface IGlobalState {
    theme: ITheme,
    loading: boolean,
    isLeftColumnActive: boolean
}

const initialState: IGlobalState = {
    theme: lightTheme,
    loading: false,
    isLeftColumnActive: true
};

const searchTool = (state = initialState, action: any): IGlobalState => {
    console.log(action.type);
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
        case TOGGLE_LEFT_COLUMN: {
            return { ...state, isLeftColumnActive: !state.isLeftColumnActive }
        }
        default:
            return state;
    }
};

export default searchTool;