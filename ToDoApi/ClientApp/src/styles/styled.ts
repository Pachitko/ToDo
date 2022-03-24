export enum ThemeType {
    light = "light",
    dark = "dark"
}

export interface ITheme {
    type: ThemeType,
    colors: {
        primary: string
        primaryLight: string
        primaryDark: string

        secondary: string
        secondaryLight: string
        secondaryDark: string

        success: string
        error: string

        white: string,
        black: string,
        gray: string,
        disabled: string,

        background: string,
        surface: string,
        surfaceHover: string,
        surfaceActive: string,
        onPrimary: string,
        onSecondary: string,
        onBackground: string,
        onSurface: string
    }
    border: {
        radius: string
    },
    media: {
        xs: string,
        s: string,
        m: string,
        l: string,
        xl: string
    }

    sizes: {
        header: { height: number }
        container: { width: number }
        footer: { height: number }
        modal: { width: number }
    }
    durations: {
        ms300: number
    }
    zIndices: {
        main: number,
        header: number,
        backScreen: number,
        contextMenu: number
    },
    padding: {
        small: number,
        medium: number,
        big: number
    }
    shadow: {
        bottom: string,
        light: {
            soft: string,
            medium: string,
            strong: string
        },
        dark: {
            soft: string,
            medium: string,
            strong: string
        }
    }
}