import { Recurrence } from "src/redux/reducers/tasks";

export type IUserToken = string | null

export interface ILoginResult {
    succeeded: boolean,
    isLockedOut: boolean,
    isNotAllowed: boolean,
    requiresTwoFactor: boolean,
    token: string
}

export interface ITaskToCreate {
    title: string,
    isCompleted: boolean | null,
    isImportant: boolean | null,
    dueDate: Date | null,
    recurrence: Recurrence | null
}

export interface ITaskListToCreate {
    title: string
}

export interface ICreatedUser {
    name: string,
    email: string
}

export interface IUserToRegister {
    username: string,
    email: string,
    password: string,
    passwordConfirmation: string,
}

export interface IExternalLoginPayload {
    providerName: string,
    username: string,
    tokenId: string,
}