export enum FormInputFieldIds {
    Email = "email",
    Password = "password",
    PasswordConfirmation = "passwordConfirmation",
    Username = "username"
}

export interface IValidatableForm {
    title: string,
    inputFields: {
        [key: string]: IFormInputField
    },
    validationResults: {
        [key: string]: IValidationResult,
    }
    isValid: boolean,
}

export interface IFormInputField {
    id: FormInputFieldIds,
    value: string,
    htmlType: string,
    label: string,
}

export interface IValidationResult {
    id: FormInputFieldIds,
    errors: string[],
    isValid: boolean
}

// export interface IValidationRule<T> {
//     validate: (value: T) => IValidationResult
// }

export const validateForm = (form: IValidatableForm) => {
    form.isValid = true
    for (const key of Object.keys(form.inputFields)) {
        const field = form.inputFields[key]
        const validatedField = validateFormInputField(form, field)
        form.isValid = form.isValid && form.validationResults[key].isValid
        form.inputFields[key] = validatedField
    }
}

const validateFormInputField = (form: IValidatableForm, field: IFormInputField): IFormInputField => {
    const result: IValidationResult = {
        id: field.id,
        errors: [],
        isValid: true
    }

    switch (field.id) {
        case FormInputFieldIds.Username:
            if (field.value === "") {
                result.isValid = false
                result.errors.push("Username is empty")
            }
            break;
        case FormInputFieldIds.Email:
            if (!validateEmail(field.value)) {
                result.isValid = false
                result.errors.push("Email is invalid")
            }
            break;
        case FormInputFieldIds.Password:
            if (field.value.length < 8) {
                result.isValid = false
                result.errors.push("Minimum password length - 8 characters")
            }
            break;
        case FormInputFieldIds.PasswordConfirmation:
            if (field.value.length === 0 || field.value !== form.inputFields[FormInputFieldIds.Password].value) {
                result.isValid = false
                result.errors.push("Password missmatch")
            }
            break;
    }

    // if (form.additionalFieldErrors[field.id]) {
    //     result.errors.concat(form.additionalFieldErrors[field.id])
    //     // result.isValid = false
    // }

    form.validationResults[result.id] = result
    return field;
}

const validateEmail = (email: string): boolean | null => {
    const regExp = new RegExp(/^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/);
    return regExp.test(email)
};
