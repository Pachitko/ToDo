import { DefaultTheme } from 'styled-components'
import { ThemeType } from './styled'

const dp = (dp: number) => `${72 / 160 * dp}pt`

export const lightTheme: DefaultTheme = {
    type: ThemeType.light,
    colors: {
        primary: '#1e88e5',
        primaryLight: '#6ab7ff',
        primaryDark: '#005cb2',

        secondary: '#673ab7',
        secondaryLight: '#9a67ea',
        secondaryDark: '#320b86',

        // /* #ffd60a; */ffc300
        // secondary: '#FB8C00',
        // secondaryLight: '#FF7043',
        // secondaryDark: '#D84315',

        success: '#43A047',
        error: '#B00020 ',

        white: '#fff',
        black: '#212121',
        gray: "#e5e5e5",
        disabled: '#c5c5c5',

        background: '#fff',
        surface: '#f2f2f2',
        surfaceHover: '#e5e5e5',
        surfaceActive: '#d9d9d9',
        onPrimary: '#fff',
        onSecondary: '#fff',
        onBackground: '#121212',
        onSurface: '#121212'
    },
    border: {
        radius: '2px'
    },
    media: {
        xs: '(max-width: 320px)',
        s: '(max-width: 540px)',
        m: '(max-width: 720px)',
        l: '(max-width: 960px)',
        xl: '(max-width: 1140px)',
    },

    // in px
    sizes: {
        header: { height: 48 },
        container: { width: 1200 },
        footer: { height: 128 },
        modal: { width: 540 },
    },

    // in ms
    durations: {
        ms300: 300,
    },

    // z-index
    zIndices: {
        main: 25,
        header: 50,
        backScreen: 75,
        contextMenu: 100,
    },
    padding: {
        small: 8,
        medium: 16,
        big: 32,
    },
    shadow: {
        bottom: '0px 4px 16px 2px #BDC3C7',
        light: {
            soft: `0px 0px 16px 1px #BDC3C7`,
            medium: `0px 0px 16px 2px #BDC3C7`,
            strong: `0px 0px 16px 4px #BDC3C7`,
        },
        dark: {
            soft: `0px 0px 16px 1px #34495E`,
            medium: `0px 0px 16px 2px #34495E`,
            strong: `0px 0px 16px 4px #34495E`,
        }
    }
}

export const darkTheme: DefaultTheme = {
    ...lightTheme,
    type: ThemeType.dark,
    colors: {
        primary: '#3f51b5',
        primaryLight: '#757de8',
        primaryDark: '#002984',

        secondary: '#4db6ac',
        secondaryLight: '#82e9de',
        secondaryDark: '#00867d',

        success: '#43A047',
        error: '#B00020 ',

        white: '#fff',
        black: '#212121',
        gray: "#e5e5e5",
        disabled: '#b5b5b5',

        background: '#212121',
        surface: '#2c2c2c',
        surfaceHover: '#363636',
        surfaceActive: '#404040',
        onPrimary: '#121212',
        onSecondary: '#121212',
        onBackground: '#fff',
        onSurface: '#fff'
    },
    border: {
        radius: '4px'
    },
    sizes: {
        header: {
            height: 0
        },
        container: {
            width: 0
        },
        footer: {
            height: 0
        },
        modal: {
            width: 0
        }
    },
    durations: {
        ms300: 0
    },
    shadow: {
        bottom: '0px 4px 16px 2px #BDC3C7',
        light: {
            soft: `0px 0px 16px 1px #212121`,
            medium: `0px 0px 16px 2px #212121`,
            strong: `0px 0px 16px 4px #212121`,
        },
        dark: {
            soft: `0px 0px 16px 1px #BDC3C7`,
            medium: `0px 0px 16px 2px #34495E`,
            strong: `0px 0px 16px 4px #34495E`,
        }
    }
}