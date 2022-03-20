// import 'styled-components';
import { ITheme } from './styles/styled';

declare module 'styled-components' {
    export interface DefaultTheme extends ITheme {

    }
}