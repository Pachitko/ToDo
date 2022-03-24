import { createGlobalStyle } from 'styled-components'

export default createGlobalStyle`
  :root {
  }

  * {
    margin: 0;
    padding: 0;
    box-sizing: border-box;
  }

  html,
  body, 
  #root {
    font-size: 16px;
    font-family: -apple-system, BlinkMacSystemFont, 'Segoe UI', 'Roboto', 'Oxygen',
      'Ubuntu', 'Cantarell', 'Fira Sans', 'Droid Sans', 'Helvetica Neue',
      sans-serif;
    -webkit-font-smoothing: antialiased;
    -moz-osx-font-smoothing: grayscale;
    height: 100vh;
  }

  code {
    font-family: source-code-pro, Menlo, Monaco, Consolas, 'Courier New',
      monospace;
  }

  a {
    text-decoration: none;
    color: white;
  }

  button {
    background-color: transparent;
    outline: none;
    border: none;
    cursor: pointer;
  }

  input {
    background-color: transparent;
    outline: none;
    border: none;
  }
`