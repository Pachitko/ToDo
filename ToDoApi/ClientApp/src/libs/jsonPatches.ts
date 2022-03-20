import { Recurrence } from "src/redux/reducers/tasks"

export interface JsonPatch {
    op: string
    from: string | null
    path: string
    value: any | null
}

export const replaceTaskIsCompletedPatch = (newValue: boolean): JsonPatch => {
    return {
        op: 'replace',
        from: null,
        path: 'isCompleted',
        value: newValue
    }
}

export const replaceTaskIsImportantPatch = (newValue: boolean): JsonPatch => {
    return {
        op: 'replace',
        from: null,
        path: 'isImportant',
        value: newValue
    }
}

export const replaceTaskTitlePatch = (newValue: string): JsonPatch => {
    return {
        op: 'replace',
        from: null,
        path: 'title',
        value: newValue
    }
}

export const replaceTaskDueDatePatch = (newValue: Date): JsonPatch => {
    return {
        op: 'replace',
        from: null,
        path: 'dueDate',
        value: newValue
    }
}

export const replaceTaskRecurrencePatch = (newValue: Recurrence): JsonPatch => {
    return {
        op: 'replace',
        from: null,
        path: 'recurrence',
        value: newValue
    }
}

export const removeTaskRecurrencePatch = (): JsonPatch => {
    return {
        op: 'remove',
        from: null,
        path: 'recurrence',
        value: null
    }
}


export const removeTaskDueDatePatch = (): JsonPatch => {
    return {
        op: 'remove',
        from: null,
        path: 'dueDate',
        value: null
    }
}