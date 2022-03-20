import styled from 'styled-components'

const SPanel = styled.div`
    background-color: ${({ theme }) => theme.colors.surface};
    border-radius: ${({ theme }) => theme.border.radius};
    box-shadow: ${({ theme }) => theme.shadow.light.medium};
    padding: ${({ theme }) => theme.padding.small}px;
    display: flex;
    flex-direction: column;
`

const SInput = styled.input`
    font-size: 1rem;
    transition: all 0.15s ease-in-out;
    border-radius: ${({ theme }) => theme.border.radius};
    color: ${({ theme }) => theme.colors.onPrimary};
    background-color: ${props => props.theme.colors.primary};
    border: 2px solid ${props => props.theme.colors.primary};
    ::placeholder{
        color: ${({ theme }) => theme.colors.disabled};
    }
    &:focus{
        color: ${({ theme }) => theme.colors.onBackground};
        background-color: transparent;
    }
`

const SIconCheckBox = styled.input`
    box-sizing: border-box;
    font-size: 1rem;
    transition: all 0.15s ease-in-out;
    border-radius: ${({ theme }) => theme.border.radius};
    color: ${(p) => p.theme.colors.onPrimary};
    background-color: ${props => props.theme.colors.primary};
    border: 2px solid transparent;
    &:hover{
        color: ${(p) => p.theme.colors.primary};
        background-color: transparent;
        border: 2px solid ${props => props.theme.colors.primary};
    }
`

const SButton = styled.button`
    border-radius: ${({ theme }) => theme.border.radius};
    font-size: 1rem;
    font-weight: bold;
    transition: all 0.15s ease-in-out;
    padding: 8px;
    color: ${p => p.theme.colors.primary};
    background-color: ${p => p.theme.colors.background};
    border: 2px solid ${props => props.theme.colors.primary};
    &:hover{
        background-color: ${props => props.theme.colors.primary};
        border: 2px solid ${props => props.theme.colors.primary};
        color: ${p => p.theme.colors.background};
    };
`

const SButtonFilled = styled.button`
    font-size: 1rem;
    transition: all 0.15s ease-in-out;
    border-radius: ${({ theme }) => theme.border.radius};
    color: ${(p) => p.theme.colors.white};
    background-color: ${props => props.theme.colors.primary};
    border: 2px solid transparent;
    &:hover{
        color: ${(p) => p.theme.colors.primary};
        background-color: transparent;
        border: 2px solid ${props => props.theme.colors.primary};
    }
`

const STextButton = styled.button`
    font-size: 1rem;
    transition: all 0.15s ease-in-out;
    color: ${(p) => p.theme.colors.primary};
    :disabled{
        color: ${p => p.theme.colors.disabled};
    }
`

const SIconButtonFilled = styled.button`
height: 32px;
width: 32px;
 display: flex;
    align-items: center;
    justify-content: center;
    font-size: 1rem;
    transition: all 0.15s ease-in-out;
    border: 2px solid ${p => p.theme.colors.primary};
    border-radius: ${({ theme }) => theme.border.radius};
    color: ${(p) => p.theme.colors.white};
    background-color: ${p => p.theme.colors.primary};
    &:hover{
        background-color: transparent;
        color: ${(p) => p.theme.colors.onSurface};
        border: 2px solid ${props => props.theme.colors.onSurface};
    }
`

const SIconButton = styled.button`
    width: 32px;
    height: 32px;
    cursor: pointer;
    transition: color .1s ease;
    color: ${p => p.theme.colors.onSurface};
    &>i{
        height: 100%;
        line-height: 32px;
        font-size: 1rem;
    }
    &:hover{
        color: ${p => p.theme.colors.primary};
    }
`

export { SPanel, SButton, SInput, SIconButtonFilled, SIconCheckBox, SButtonFilled, STextButton, SIconButton }