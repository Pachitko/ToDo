import React from 'react'
import styled from 'styled-components'

const SLeftColumn = styled.div`
    background-color: ${p => p.theme.colors.surface};
    min-width: 224px;
    display: flex;
    flex-direction: column;
`
const LeftColumn: React.FC = ({ children }) => (
    <SLeftColumn>
        {children}
    </SLeftColumn>
)

export default LeftColumn 
