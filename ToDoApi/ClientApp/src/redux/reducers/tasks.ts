import {
    PATCH_TASK, DELETE_TASK, HIDE_TASK_DETAILS, SELECT_TASK, SELECT_TASK_LIST, SET_SEARCH_TEXT,
    TASKS_LOADING, LOAD_TASKS_SUCCESS, LOAD_TASKS_ERROR, POST_TASK, POST_TASK_LIST, DELETE_TASK_LIST, LOGOUT, RENAME_TASK_LIST
} from "src/redux/actions/actionTypes";

export interface ITask {
    id: string,
    toDoListId: string,
    title: string,
    isCompleted: boolean,
    isImportant: boolean,
    dueDate: Date | null,
    createdAt: Date | null,
    modifiedAt: Date | null,
    recurrence: Recurrence | null
}

export interface Recurrence {
    interval: number,
    startedAt: Date,
    type: RecurenceType
}

export enum RecurenceType {
    Hourly,
    Daily,
    Weekly,
    Monthly,
    Yearly
}

export interface ITaskList {
    id: string,
    title: string,
    tasks: ITask[]
}

export interface ITasksState {
    isLoading: boolean,
    activeListId: string,
    activeTaskId: string,
    activeTask: ITask | null,
    activeList: ITaskList | null,
    taskLists: ITaskList[]
}

const initialState: ITasksState = {
    isLoading: false,
    activeListId: "",
    activeTaskId: "",
    activeTask: null,
    activeList: null,
    taskLists: []
};

const tasks = (state = initialState, action: any): ITasksState => {
    switch (action.type) {
        case LOGOUT: {
            return initialState;
        }
        case SELECT_TASK: {
            const { task } = action.payload
            return {
                ...state,
                activeTask: task,
                activeTaskId: task.id
            };
        }
        case SELECT_TASK_LIST: {
            return {
                ...state, activeListId: action.payload.listId, activeTaskId: ''
            };
        }
        case DELETE_TASK: {
            const { listId, taskId } = action.payload
            return {
                ...state,
                taskLists: state.taskLists
                    .map(list => list.id === listId
                        ? { ...list, tasks: list.tasks.filter(t => t.id !== taskId) }
                        : list),
                activeTaskId: '',
                isLoading: false
            };
        }
        case PATCH_TASK: {
            const { listId, patchedTask } = action.payload
            return {
                ...state,
                activeTask: patchedTask,
                taskLists: state.taskLists
                    .map(list => list.id === listId
                        ? {
                            ...list,
                            tasks: list.tasks.map(t => t.id === patchedTask.id ? patchedTask : t)
                        }
                        : list),
                isLoading: false
            };
        }
        case HIDE_TASK_DETAILS: {
            return { ...state, activeTaskId: '' }
        }
        case TASKS_LOADING: {
            return { ...state, isLoading: true }
        }
        case LOAD_TASKS_SUCCESS: {
            const { taskLists } = action.payload
            console.log(taskLists);
            return {
                ...state,
                activeListId: taskLists[0].id,
                taskLists,
                isLoading: false
            }
        }
        case LOAD_TASKS_ERROR: {
            const { error } = action.payload;
            console.log(error);
            return { ...state, isLoading: false }
        }
        case POST_TASK: {
            const { createdTask } = action.payload
            return {
                ...state,
                taskLists: state.taskLists
                    .map(list => list.id === createdTask.toDoListId
                        ? { ...list, tasks: list.tasks.concat(createdTask) }
                        : list),
                isLoading: false
            };
        }
        case POST_TASK_LIST: {
            const { createdTaskList } = action.payload
            createdTaskList.tasks = []
            return {
                ...state,
                taskLists: state.taskLists.concat(createdTaskList),
                isLoading: false
            };
        }
        case RENAME_TASK_LIST: {
            console.log(RENAME_TASK_LIST);
            const { renamedTaskList } = action.payload
            return {
                ...state,
                taskLists: state.taskLists.map(l => l.id === renamedTaskList.id
                    ? { ...l, title: renamedTaskList.title }
                    : l),
                isLoading: false
            };
        }
        case DELETE_TASK_LIST: {
            const { listId } = action.payload
            return {
                ...state,
                taskLists: state.taskLists.filter(l => l.id !== listId),
                activeTask: null,
                activeTaskId: '',
                activeListId: state.activeListId === listId ? '' : state.activeListId,
                isLoading: false
            };
        }
        default:
            return state;
    }
};

export default tasks;