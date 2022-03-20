import React from 'react'
import styled from 'styled-components'

const SLayout = styled.div`
    min-height: 100vh;
    display: flex;
    flex-direction: column;
`

const Layout: React.FC = ({ children }) => {
    return (
        <SLayout>
            {children}
        </SLayout>
    )
}

export default Layout;