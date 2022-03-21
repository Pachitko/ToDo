import React from 'react';
import styled, { css } from 'styled-components'
import ContextMenu from 'src/Components/UI/ContextMenu'

interface Props {
    title: string,
    onClickOutside: any
}
const TaskDetailsContextMenu: React.FC<Props> = (props) => {
    const ctxMenuStyles = css`
        top: 100%;
        right: 0;
        left: 0;
    `

    return (
        <ContextMenu onClickOutside={props.onClickOutside} ctxMenuStyles={ctxMenuStyles}>
            <SContextMenuTitle>{props.title}</SContextMenuTitle>
            <SHorizontalLine />
            {props.children}
        </ContextMenu>
    )
}

export default TaskDetailsContextMenu

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