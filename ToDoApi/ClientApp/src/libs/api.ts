import axios, { AxiosError } from "axios"
import { ITask, ITaskList, Recurrence } from "src/redux/reducers/tasks";
import { IUser } from "src/redux/reducers/user"
import { JsonPatch } from "./jsonPatches";

const baseURL = 'https://localhost:5001/api/';

export type IUserToken = string | null

// const withToken = () => {
// }

const getToken = () => localStorage.getItem("token")

interface Config {
    [key: string]: any
}

const getConfig = (token: IUserToken): Config => ({
    baseURL,
    headers: {
        "Authorization": `Bearer ${token}`
    }
})

export const loginAsync = async (username: string, password: string): Promise<IUserToken> => {
    try {
        const response = await axios.post("users/token", {
            "Username": username,
            "Password": password,
        }, { baseURL })
        const token = response.data.token

        if (!token)
            throw new Error("Token in null");

        return token
    }
    catch (error) {
        throw error
    }
}

export const getMeAsync = async (token: IUserToken): Promise<IUser | null> => {
    if (!token)
        throw new Error("Token in null");

    try {
        const config = getConfig(token)
        config.headers["Accept"] = "application/vnd.todo.user.friendly.hateoas+json"

        const response = await axios.get("users/me?fields=username,email", config)
        const user = response.data;
        console.log(response);

        if (!user)
            throw new Error("User info not found");

        return {
            name: user.userName,
            email: user.email,
            token: token,
        }
    } catch (error) {
        throw error
    }
}

export const getUserTaskLists = async (): Promise<ITaskList[]> => {
    try {
        const response = await axios.get(`taskLists`, getConfig(getToken()))
        const taskLists: ITaskList[] = response.data.map((l: ITaskList) => {
            return {
                ...l
            }
        })
        for (const list of taskLists) {
            list.tasks = await getListTasks(list.id)
        }
        return taskLists
    } catch (error) {
        throw error
    }
}

export const getListTasks = async (listId: string): Promise<ITask[]> => {
    try {
        const response = await axios.get(`taskLists/${listId}/tasks`, getConfig(getToken()))
        const tasks: ITask[] = response.data.map((t: ITask) => {
            return {
                ...t
            }
        })
        return tasks
    } catch (error) {
        throw error
    }
}

export const deleteTask = async (listId: string, taskId: string) => {
    try {
        const response = await axios.delete(`taskLists/${listId}/tasks/${taskId}`, getConfig(getToken()))
        return response.data
    } catch (error) {
        throw error
    }
}

export interface ITaskToCreate {
    title: string,
    isCompleted: boolean | null,
    isImportant: boolean | null,
    dueDate: Date | null,
    recurrence: Recurrence | null
}

export const postTask = async (listId: string, taskToCreate: ITaskToCreate): Promise<ITask> => {
    try {
        const response = await axios.post(`taskLists/${listId}/tasks`,
            taskToCreate, getConfig(getToken()))
        const createdTask = response.data as ITask
        console.log(createdTask);
        return createdTask
    } catch (error) {
        throw error
    }
}

export const patchTask = async (listId: string, taskId: string, jsonPatchDocument: JsonPatch[]): Promise<ITask> => {
    try {
        const data = {
            jsonPatchDocument
        }
        const response = await axios.patch(`taskLists/${listId}/tasks/${taskId}`,
            data, getConfig(getToken()))
        const patchedTask = response.data as ITask
        console.log(patchedTask);
        return patchedTask
    } catch (error) {
        throw error
    }
}

export interface ITaskListToCreate {
    title: string
}
export const postTaskList = async (taskListToCreate: ITaskListToCreate): Promise<ITaskList> => {
    try {
        const response = await axios.post(`taskLists`,
            taskListToCreate, getConfig(getToken()))
        const createdTaskList = response.data as ITaskList
        console.log(createdTaskList);
        return createdTaskList
    } catch (error) {
        throw error
    }
}

export const deleteTaskList = async (listId: string) => {
    try {
        const response = await axios.delete(`taskLists/${listId}`, getConfig(getToken()))
        return response.data
    } catch (error) {
        throw error
    }
}

export interface IUserToRegister {
    username: string,
    email: string,
    password: string,
    passwordConfirmation: string,
}

export const registerUser = async (userToCreate: IUserToRegister): Promise<IUser> => {
    try {
        const response = await axios.post(`users`, userToCreate, getConfig(getToken()))
        const createdUser = response.data as IUser
        console.log("CreatedUser:", createdUser);
        return createdUser
    } catch (error) {
        throw error
    }
}
