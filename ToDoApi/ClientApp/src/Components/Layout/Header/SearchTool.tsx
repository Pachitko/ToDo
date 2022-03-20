import React, { useState } from 'react'
import styled from 'styled-components'
import { useAppSelector, useAppDispatch } from 'src/redux/hooks'
import { setSearchText } from 'src/redux/actions/searchToolActions'

const SearchTool: React.FC = () => {
    const [isSearchInputFocused, setIsSearchInputFocused] = useState(false)
    const searchText = useAppSelector(state => state.searchTool.searchText)
    const dispatch = useAppDispatch()

    const handleSearchInputFocus = () => {
        setIsSearchInputFocused(true)
    }

    const handleSearchInputBlur = () => {
        setIsSearchInputFocused(searchText.length > 0)
    }

    const handleSearchInputChange = (e: any) => {
        dispatch(setSearchText(e.target.value))
    }

    const handleClearBtnPressed = () => {
        dispatch(setSearchText(''))
        setIsSearchInputFocused(false)
    }

    return (
        <SSearch>
            <SSearchToolWrapper focus={isSearchInputFocused}>
                <SSearchBtn focus={isSearchInputFocused}>
                    <i className="fa-solid fa-magnifying-glass"></i>
                </SSearchBtn>
                <SSearchInput
                    spellCheck="false"
                    value={searchText}
                    onChange={handleSearchInputChange}
                    onBlur={handleSearchInputBlur}
                    onFocus={handleSearchInputFocus}
                    placeholder="Search"
                />
                {isSearchInputFocused ?
                    (
                        <SClearBtn onClick={handleClearBtnPressed}>
                            <i className="fa-solid fa-times"></i>
                        </SClearBtn>
                    ) : null}
            </SSearchToolWrapper>
        </SSearch>
    )
}

export default SearchTool

const SSearch = styled.div`
    flex-grow: 1;
    display: flex;
    justify-content: center;
    align-items: center;
`

const SSearchToolWrapper = styled.div<{ focus: boolean }>`
    height: 32px;
    width: 100%;
    display: flex;
    justify-content: center;
    align-items: center;
    transition: all .1s ease-in-out;
    border-radius: 5px;
    max-width: 400px;
    min-width: 50px;
    background-color: ${p => !p.focus && p.theme.colors.primaryDark};
    border: 2px solid ${p => p.focus && p.theme.colors.primaryDark || 'transparent'};
`

const SSearchInput = styled.input`
    ::placeholder{
        color: ${p => p.theme.colors.disabled};
    }
    height: 100%;
    color: ${p => p.theme.colors.white};
    padding: 0 5px;
    flex-grow: 1;
    font-size: 1rem;
    caret-color: ${p => p.theme.colors.white};
`

const SBtn = styled.button`
    width: 32px;
    height: 32px;
    cursor: pointer;
    color: ${p => p.theme.colors.white};
    transition: color .15s ease;
    &>i{
        height: 100%;
        line-height: 32px;
        font-size: 16px;
    }
`

const SSearchBtn = styled(SBtn) <{ focus: boolean }>`
    &:hover{
        color: ${p => p.focus && p.theme.colors.primaryDark || p.theme.colors.primary};
    }
`

const SClearBtn = styled(SBtn)`
    &:hover{
        color: ${p => p.theme.colors.primaryDark};
    }
`