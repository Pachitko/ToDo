import { LOGOUT, SET_SEARCH_TEXT } from "src/redux/actions/actionTypes";

export interface ISearchToolState {
    searchText: string
}

const initialState: ISearchToolState = {
    searchText: ""
};

const searchTool = (state = initialState, action: any): ISearchToolState => {
    switch (action.type) {
        case LOGOUT: {
            return initialState;
        }
        case SET_SEARCH_TEXT: {
            const { searchText } = action.payload;
            return { searchText };
        }
        default:
            return state;
    }
};

export default searchTool;