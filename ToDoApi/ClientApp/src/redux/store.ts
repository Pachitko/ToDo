import { createStore, applyMiddleware, compose } from "redux";
import rootReducer from "./reducers/index";

const loggerMiddleware = (store: any) => (next: any) => (action: any) => {
    const result = next(action);
    return result;
}

const asyncFunctionMiddleware = (store: any) => (next: any) => (action: any) => {
    if (typeof action === 'function') {
        return action(store.getState(), store.dispatch)
    }
    return next(action)
}

const composeEnhancers =
    typeof window === 'object' && window.__REDUX_DEVTOOLS_EXTENSION_COMPOSE__ ?
        window.__REDUX_DEVTOOLS_EXTENSION_COMPOSE__({
            // Specify extension’s options like name, actionsDenylist, actionsCreators, serialize...
        })
        : compose; // from redux module

const store = createStore(rootReducer, composeEnhancers(applyMiddleware(loggerMiddleware, asyncFunctionMiddleware)));

// const store = createStore(rootReducer, applyMiddleware(loggerMiddleware, asyncFunctionMiddleware))

export type AppState = ReturnType<typeof store.getState>
export type AppDispatch = typeof store.dispatch
export default store