import {
    PATCH_TASK, DELETE_TASK, HIDE_TASK_DETAILS, SELECT_TASK,
    SELECT_TASK_LIST, SET_SEARCH_TEXT,
    TASKS_LOADING, LOAD_TASKS_SUCCESS, LOAD_TASKS_ERROR, POST_TASK,
    POST_TASK_LIST, DELETE_TASK_LIST, LOGOUT, RENAME_TASK_LIST
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
    isVisible?: boolean,
    isSmart?: boolean,
    iconClass?: string,
    filter?: (task: ITask) => boolean
}

export interface ITasksState {
    isLoading: boolean,
    activeTask: ITask | null,
    activeList: ITaskList | null,
    taskLists: ITaskList[],
    tasks: ITask[],
    smartTaskLists: ITaskList[]
}

const initialState: ITasksState = {
    isLoading: false,
    activeTask: null,
    activeList: null,
    taskLists: [],
    tasks: [],
    smartTaskLists: [
        {
            id: "all",
            title: "All",
            isSmart: true,
            isVisible: true,
            iconClass: "fa-solid fa-infinity",
            filter: (task: ITask) => true
        },
        {
            id: "important",
            title: "Important",
            isSmart: true,
            isVisible: true,
            iconClass: "fa-solid fa-star",
            filter: (task: ITask) => task.isImportant
        },
        {
            id: "planned",
            title: "Planned",
            isSmart: true,
            isVisible: true,
            iconClass: "fa-regular fa-calendar",
            filter: (task: ITask) => !!task.dueDate
        }
    ]
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
                activeTask: task
            };
        }
        case SELECT_TASK_LIST: {
            const { list } = action.payload
            return {
                ...state,
                activeList: list,
                activeTask: null
            };
        }
        case DELETE_TASK: {
            const { listId, taskId } = action.payload
            return {
                ...state,
                tasks: state.tasks.filter(t => t.id !== taskId),
                activeTask: null,
                isLoading: false
            };
        }
        case PATCH_TASK: {
            const { listId, patchedTask } = action.payload
            return {
                ...state,
                activeTask: patchedTask,
                tasks: state.tasks.map(t => t.toDoListId === listId && t.id === patchedTask.id
                    ? patchedTask
                    : t),
                isLoading: false
            };
        }
        case HIDE_TASK_DETAILS: {
            return { ...state, activeTask: null }
        }
        case TASKS_LOADING: {
            return { ...state, isLoading: true }
        }
        case LOAD_TASKS_SUCCESS: {
            const { taskLists, tasks } = action.payload
            return {
                ...state,
                activeList: state.smartTaskLists[0],
                taskLists,
                tasks,
                isLoading: false
            }
        }
        case LOAD_TASKS_ERROR: {
            const { error } = action.payload;
            return { ...state, isLoading: false }
        }
        case POST_TASK: {
            const { createdTask } = action.payload
            return {
                ...state,
                tasks: state.tasks.concat(createdTask),
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
                activeList: null,
                // activeList: state.taskLists.find(l => l.id === listId),
                isLoading: false
            };
        }
        default:
            return state;
    }
};

export default tasks;