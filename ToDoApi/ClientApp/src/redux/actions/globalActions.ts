import { LOAD_THEME_FROM_STORAGE, TOGGLE_LEFT_COLUMN, TOGGLE_THEME } from "./actionTypes";

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

export const toggleLeftColumn = () => {
    return {
        type: TOGGLE_LEFT_COLUMN
    }
}