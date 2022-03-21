import {
    getUserTaskLists, deleteTask, ITaskToCreate, postTask,
    patchTask, ITaskListToCreate, postTaskList, deleteTaskList, renameTaskList
} from 'src/libs/api';
import { JsonPatch } from 'src/libs/jsonPatches';
import { ITask, ITaskList } from '../reducers/tasks'
import { AppDispatch, AppState } from '../store';
import {
    SELECT_TASK, DELETE_TASK, SELECT_TASK_LIST, PATCH_TASK, HIDE_TASK_DETAILS,
    TASKS_LOADING, LOAD_TASKS_SUCCESS, LOAD_TASKS_ERROR, POST_TASK, POST_TASK_LIST, DELETE_TASK_LIST, RENAME_TASK_LIST
} from './actionTypes'

export const selectTaskAction = (task: ITask) => {
    return {
        type: SELECT_TASK,
        payload: {
            task
        }
    }
}

export const postTaskAction = (createdTask: ITask) => {
    return {
        type: POST_TASK,
        payload: {
            createdTask
        }
    }
}

export const postTaskListAction = (createdTaskList: ITaskList) => {
    return {
        type: POST_TASK_LIST,
        payload: {
            createdTaskList
        }
    }
}

export const renameTaskListAction = (renamedTaskList: ITaskList) => {
    return {
        type: RENAME_TASK_LIST,
        payload: {
            renamedTaskList
        }
    }
}

export const deleteTaskAction = (listId: string, taskId: string) => {
    return {
        type: DELETE_TASK,
        payload: {
            listId,
            taskId
        }
    }
}

export const deleteTaskListAction = (listId: string) => {
    return {
        type: DELETE_TASK_LIST,
        payload: {
            listId
        }
    }
}

export const selectTaskListAction = (listId: string) => {
    return {
        type: SELECT_TASK_LIST,
        payload: {
            listId
        }
    }
}

export const patchTaskAcion = (listId: string, patchedTask: ITask) => {
    return {
        type: PATCH_TASK,
        payload: {
            listId,
            patchedTask
        }
    }
}

export const hideTaskDetailsAction = () => {
    return {
        type: HIDE_TASK_DETAILS,
    }
}

export const tasksLoadingAction = () => {
    return {
        type: TASKS_LOADING,
    }
}

export const loadTasksSuccessAction = (taskLists: ITaskList[]) => {
    return {
        type: LOAD_TASKS_SUCCESS,
        payload: {
            taskLists
        }
    }
}

export const taskErrorAction = (error: string) => {
    return {
        type: LOAD_TASKS_ERROR,
        payload:
        {
            error
        }
    }
}

export const deleteTaskAsync = (listId: string, taskId: string) => {
    return async (state: AppState, dispatch: AppDispatch) => {
        dispatch(tasksLoadingAction())
        try {
            await deleteTask(listId, taskId)
            dispatch(deleteTaskAction(listId, taskId))
        } catch (error) {
            dispatch(taskErrorAction(error))
        }
    }
}

export const loadTasksAsync = () => {
    return async (state: AppState, dispatch: AppDispatch) => {
        dispatch(tasksLoadingAction())
        try {
            const tasks = await getUserTaskLists()
            dispatch(loadTasksSuccessAction(tasks))
        } catch (error) {
            dispatch(taskErrorAction(error))
        }
    }
}

export const postTaskAsync = (listId: string, taskToCreate: ITaskToCreate) => {
    return async (state: AppState, dispatch: AppDispatch) => {
        dispatch(tasksLoadingAction())
        try {
            const createdTask = await postTask(listId, taskToCreate)
            dispatch(postTaskAction(createdTask))
        } catch (error) {
            dispatch(taskErrorAction(error))
        }
    }
}

export const postTaskListAsync = (taskListToCreate: ITaskListToCreate) => {
    return async (state: AppState, dispatch: AppDispatch) => {
        dispatch(tasksLoadingAction())
        try {
            const createdTaskList = await postTaskList(taskListToCreate)
            dispatch(postTaskListAction(createdTaskList))
        } catch (error) {
            dispatch(taskErrorAction(error))
        }
    }
}

export const patchTaskAsync = (listId: string, taskId: string, jsonPatchDocument: JsonPatch[]) => {
    return async (state: AppState, dispatch: AppDispatch) => {
        console.log(jsonPatchDocument);
        dispatch(tasksLoadingAction())
        try {
            const patchedTask = await patchTask(listId, taskId, jsonPatchDocument)
            dispatch(patchTaskAcion(listId, patchedTask))
        } catch (error) {
            dispatch(taskErrorAction(error))
        }
    }
}

export const deleteTaskListAsync = (listId: string) => {
    return async (state: AppState, dispatch: AppDispatch) => {
        dispatch(tasksLoadingAction())
        try {
            await deleteTaskList(listId)
            dispatch(deleteTaskListAction(listId))
        } catch (error) {
            dispatch(taskErrorAction(error))
        }
    }
}

export const renameTaskListAsync = (listId: string, newTitle: string) => {
    return async (state: AppState, dispatch: AppDispatch) => {
        dispatch(tasksLoadingAction())
        try {
            const renamedTaskList = await renameTaskList(listId, newTitle)
            dispatch(renameTaskListAction(renamedTaskList))
        } catch (error) {
            dispatch(taskErrorAction(error))
        }
    }
}
