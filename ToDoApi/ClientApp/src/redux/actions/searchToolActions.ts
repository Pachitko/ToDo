import { SET_SEARCH_TEXT } from './actionTypes'

export const setSearchText = (searchText: string) => {
    return {
        type: SET_SEARCH_TEXT,
        payload: {
            searchText
        }
    }
}