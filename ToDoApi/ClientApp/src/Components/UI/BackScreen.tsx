import React from 'react'
import styled from 'styled-components'

interface Props {
    onClick: any
}
const BackScreen: React.FC<Props> = ({ onClick }) => {
    return (
        <SBackScreen onClick={onClick} />
    )
}

export default BackScreen

const SBackScreen = styled.div`
    z-index: ${p => p.theme.zIndices.backScreen};
    position: fixed;
    background-color: black;
    opacity: 0.1;
    left: 0;
    top: 0;
    height: 100vh;
    width: 100vw;
`
