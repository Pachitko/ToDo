import { VISIBILITY_FILTER } from "src/constants";
import { SET_FILTER } from "src/redux/actions/actionTypes";

const initialState = VISIBILITY_FILTER.ALL;

const visibilityFilter = (state = initialState, action: any) => {
    switch (action.type) {
        case SET_FILTER: {
            const { filter } = action.payload;
            return filter;
        }
        default:
            return state;
    }
};

export default visibilityFilter;