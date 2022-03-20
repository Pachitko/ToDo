import React from 'react';
import { SPanel } from 'src/Components/UI';
import BackScreen from 'src/Components/UI/BackScreen';
import styled from 'styled-components'

interface Props {
    title: string,
    onClickOutside: any
}
const ContextMenu: React.FC<Props> = ({ children, title, onClickOutside }) => {
    return (
        <>
            <BackScreen onClick={onClickOutside} />
            <SContextMenu>
                <SContextMenuTitle>{title}</SContextMenuTitle>
                <SHorizontalLine />
                {children}
            </SContextMenu>
        </>
    )
}

export default ContextMenu

const SContextMenuTitle = styled.div`
    display: flex;
    justify-content: center;
    align-items: center;
    padding: 4px 0;
    user-select: none;
`

const SHorizontalLine = styled.div`
    border: 1px solid ${p => p.theme.colors.surfaceActive}
`

const SContextMenu = styled(SPanel)`
    z-index: ${p => p.theme.zIndices.contextMenu};
    background-color: ${p => p.theme.colors.surface};
    color: ${p => p.theme.colors.onSurface};
    display: flex;
    top: 100%;
    left: 0;
    right: 0;
    margin: 0 auto;
    max-width: 200px;
    padding: 0;
    position: absolute;
`