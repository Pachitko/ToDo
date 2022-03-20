import React from "react";
import ReactDOM from "react-dom";
import GlobalStyles from './styles/global'
import { Provider } from 'react-redux'
import store from './redux/store'
import App from "./Components/App";
import { BrowserRouter } from 'react-router-dom'

ReactDOM.render(
    <React.StrictMode>
        <BrowserRouter>
            <Provider store={store}>
                <GlobalStyles />
                <App />
            </Provider>
        </BrowserRouter>
    </React.StrictMode>,
    document.querySelector("#root"));