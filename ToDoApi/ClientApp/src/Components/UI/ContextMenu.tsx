import React from 'react';
import { SPanel } from 'src/Components/UI';
import BackScreen from 'src/Components/UI/BackScreen';
import styled, { css, FlattenSimpleInterpolation } from 'styled-components'

export interface ContextMenuProps {
    onClickOutside: any,
    ctxMenuStyles: FlattenSimpleInterpolation
}

interface ContextMenuStyles {
    top?: string,
    right?: string,
    bottom?: string,
    left?: string,
}

const ContextMenu: React.FC<ContextMenuProps> = (props) => {
    const s = css``
    return (
        <>
            <BackScreen onClick={props.onClickOutside} />
            <SContextMenu ctxMenuStyles={props.ctxMenuStyles}>
                {props.children}
            </SContextMenu>
        </>
    )
}

export default ContextMenu

const SContextMenu = styled(SPanel) <{ ctxMenuStyles: FlattenSimpleInterpolation }>`
    z-index: ${p => p.theme.zIndices.contextMenu};
    background-color: ${p => p.theme.colors.surface};
    color: ${p => p.theme.colors.onSurface};
    display: flex;
    padding: 0;
    position: absolute;
    ${p => p.ctxMenuStyles};
`