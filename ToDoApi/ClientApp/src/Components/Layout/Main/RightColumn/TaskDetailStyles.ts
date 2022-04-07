import styled from "styled-components"

const SSectionContent = styled.div<{ isSet: boolean }>`
    text-align: left;
    color: ${p => p.isSet ? p.theme.colors.onSurface : p.theme.colors.disabled};
    display: flex;
`

const SSectionItem = styled.div<{ isFocused: boolean }>`
    border-bottom: 2px solid ${p => p.theme.colors.surfaceHover};
    position: relative;
    height: 48px;
    display: flex;
    align-items: center;
    background-color: ${p => p.isFocused && p.theme.colors.gray};
    :hover{
        background-color: ${p => p.theme.colors.surfaceHover};
    }
`

const SSectionBodyButton = styled.button`
    height: 100%;
    display: flex;
    align-items: center;
    flex-grow: 1;
    padding: 4px 0;
`

const SSectionIcon = styled.section<{ isSet: boolean }>`
    display: flex;
    justify-content: center;
    align-items: center;
    width: 32px;
    height: 32px;
    &>i{
        color: ${p => p.isSet ? p.theme.colors.primary : p.theme.colors.disabled};
        height: 100%;
        line-height: 32px;
        font-size: 1rem;
    }
`

export { SSectionContent, SSectionIcon, SSectionItem, SSectionBodyButton }