import styled from "styled-components";
import { SIconCheckBox } from "./StyledElements";

export const STaskCheckBoxLabel = styled.label`
    display: flex;
    justify-content: center;
    align-items: center;
    width: 32px;
    height: 32px;
    cursor: pointer;
    &>i{
        font-size: 1.25rem;
        color: ${p => p.theme.colors.primary};
    }
`

export const STaskCheckBox = styled(SIconCheckBox)`
    cursor: pointer;
    position: absolute;
    opacity: 0;
    height: 0;
    width: 0;

    &>i{
        height: 100%;
        display: block;

        &>input{
            display: 0;
        }
    }
`