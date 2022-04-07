import axios, { AxiosRequestConfig } from "axios"
import { ITask, ITaskList } from "src/redux/reducers/tasks";
import { IIdentity } from "src/redux/reducers/auth"
import { JsonPatch } from "./jsonPatches";
import { ILoginResult, ITaskToCreate, ITaskListToCreate, IUserToRegister, ICreatedUser, IExternalLoginPayload, IUserToken } from "./abstractions/index";

const getToken = () => localStorage.getItem("token")

const instance = (token: IUserToken = null) => {
    const config: AxiosRequestConfig<any> = {
        timeout: 5000,
        baseURL: 'https://localhost:5001/api/',
    }

    if (token) {
        config.headers =
        {
            ...config.headers,
            "Authorization": `Bearer ${token}`
        }
    }

    return axios.create(config)
}

export const loginWithExternalProviderAsync = async (providerName: string, tokenId: string): Promise<ILoginResult> => {
    try {
        const response = await instance().post("auth/external",
            {
                providerName: providerName,
                tokenId: tokenId
            }, {
            headers: {
                'Access-Control-Allow-Origin': "true",
                'Cache-Control': 'no-cache',
                'Pragma': 'no-cache',
                'Expires': '0',
            }
        })
        return response.data
    } catch (error) {
        throw error
    }
}

export const loginAsync = async (username: string, password: string): Promise<ILoginResult> => {
    try {
        const response = await instance().post("auth/login", {
            "Username": username,
            "Password": password,
        })
        return response.data
    }
    catch (error) {
        throw error
    }
}

export const verifyTokenAsync = async (token: IUserToken): Promise<boolean> => {
    try {
        const response = await instance(token).get("auth/verifyToken")
        return response.data
    }
    catch (error) {
        throw error
    }
}

export const getMeAsync = async (token: IUserToken): Promise<IIdentity | null> => {
    if (!token) {
        throw new Error("Token in null");
    }

    try {
        const config: AxiosRequestConfig<any> = {
            headers: {
                "Accept": "application/json"
            }
        }

        const response = await instance(token).get("users/me?fields=username,email", config)
        const identity = response.data;

        return {
            username: identity.userName,
            email: identity.email
        }
    } catch (error) {
        throw error
    }
}

export const getTaskListsAsync = async (): Promise<ITaskList[]> => {
    try {
        const response = await instance(getToken()).get(`taskLists`)
        const taskLists: ITaskList[] = response.data.map((l: ITaskList) => {
            return {
                ...l
            }
        })
        return taskLists
    } catch (error) {
        throw error
    }
}

export const getListTasksAsync = async (taskLists: ITaskList[]): Promise<ITask[]> => {
    try {
        const result: ITask[] = []
        for (const list of taskLists) {
            const response = await instance(getToken()).get(`taskLists/${list.id}/tasks`)
            const tasks: ITask[] = response.data.map((t: ITask) => {
                return {
                    ...t
                }
            })
            result.push(...tasks)
        }
        return result
    } catch (error) {
        throw error
    }
}

export const deleteTaskAsync = async (listId: string, taskId: string) => {
    try {
        const response = await instance(getToken()).delete(`taskLists/${listId}/tasks/${taskId}`)
        return response.data
    } catch (error) {
        throw error
    }
}

export const postTaskAsync = async (listId: string, taskToCreate: ITaskToCreate): Promise<ITask> => {
    try {
        const response = await instance(getToken()).post(`taskLists/${listId}/tasks`, taskToCreate)
        const createdTask = response.data as ITask
        return createdTask
    } catch (error) {
        throw error
    }
}

export const patchTaskAsync = async (listId: string, taskId: string, jsonPatchDocument: JsonPatch[]): Promise<ITask> => {
    try {
        const data = {
            jsonPatchDocument
        }
        const response = await instance(getToken())
            .patch(`taskLists/${listId}/tasks/${taskId}`, data,)
        const patchedTask = response.data as ITask
        return patchedTask
    } catch (error) {
        throw error
    }
}

export const postTaskListAsync = async (taskListToCreate: ITaskListToCreate): Promise<ITaskList> => {
    try {
        const response = await instance(getToken()).post(`taskLists`, taskListToCreate)
        const createdTaskList = response.data as ITaskList
        return createdTaskList
    } catch (error) {
        throw error
    }
}

export const deleteTaskListAsync = async (listId: string) => {
    try {
        const response = await instance(getToken()).delete(`taskLists/${listId}`)
        return response.data
    } catch (error) {
        throw error
    }
}

export const renameTaskListAsync = async (listId: string, newTitle: string) => {
    try {
        const response = await instance(getToken()).post(`taskLists/${listId}`, { newTitle })
        return response.data as ITaskList
    } catch (error) {
        throw error
    }
}


export const registerUserAsync = async (userToCreate: IUserToRegister): Promise<ICreatedUser> => {
    try {
        const response = await instance().post(`users`, userToCreate)
        const createdUser = response.data as ICreatedUser
        return createdUser
    } catch (error) {
        throw error
    }
}

export const registerWithExternalProviderAsync = async (externalLoginPayload: IExternalLoginPayload): Promise<ICreatedUser> => {
    try {
        const response = await instance().post(`users/external`, externalLoginPayload)
        const createdUser = response.data as ICreatedUser
        return createdUser
    } catch (error) {
        throw error
    }
}