import { combineReducers } from 'redux'
import searchTool from './searchTool'
import tasks from './tasks'
import global from './global'
import user from './user'

export default combineReducers({ global, searchTool, tasks, user });