import { LOAD_THEME_FROM_STORAGE, TOGGLE_THEME } from "./actionTypes";

export const toggleTheme = () => {
    return {
        type: TOGGLE_THEME
    }
}

export const loadThemeFromStorage = () => {
    return {
        type: LOAD_THEME_FROM_STORAGE
    }
}