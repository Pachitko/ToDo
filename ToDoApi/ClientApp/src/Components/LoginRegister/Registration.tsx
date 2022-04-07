import React, { FC } from 'react';
import { NavLink } from 'react-router-dom';
import { registerUser } from 'src/redux/actions/userActions';
import { useDispatch } from 'react-redux';
import { IUserToRegister } from 'src/libs/abstractions';
import { FormInputFieldIds, IValidatableForm } from 'src/libs/loginRegisterValidation';
import { useAppSelector } from 'src/redux/hooks';
import ValidateableForm from '../UI/ValidatableForm';

const Registration: FC = () => {
    const dispatch = useDispatch();
    const registrationErrors = useAppSelector(state => state.user.registrationErrors)

    const formInitialState: IValidatableForm = {
        title: "REGISTRATION",
        submitText: "Register",
        isValid: false,
        validationResults: {},
        additionalInputFieldErrors: registrationErrors,
        inputFields: {
            [`${FormInputFieldIds.Username}`]: {
                id: FormInputFieldIds.Username,
                value: "",
                htmlType: "text",
                label: "Username",
            },
            [`${FormInputFieldIds.Email}`]: {
                id: FormInputFieldIds.Email,
                value: "",
                htmlType: "email",
                label: "E-mail"
            },
            [`${FormInputFieldIds.Password}`]: {
                id: FormInputFieldIds.Password,
                value: "",
                htmlType: "password",
                label: "Password"
            },
            [`${FormInputFieldIds.PasswordConfirmation}`]: {
                id: FormInputFieldIds.PasswordConfirmation,
                value: "",
                htmlType: "password",
                label: "Password confirmation"
            }
        },
    }

    const handleSubmit = (form: IValidatableForm) => {
        const userToRegister: IUserToRegister = {
            username: form.inputFields[FormInputFieldIds.Username].value,
            email: form.inputFields[FormInputFieldIds.Email].value,
            password: form.inputFields[FormInputFieldIds.Password].value,
            passwordConfirmation: form.inputFields[FormInputFieldIds.PasswordConfirmation].value
        }
        dispatch(registerUser(userToRegister))
    }

    return (
        <ValidateableForm
            formInitialState={formInitialState}
            onSubmit={handleSubmit}
            additionalPanelContent={
                <>
                    <NavLink to={"/auth/login"}>Log in</NavLink>
                </>
            }
        />
    );
}

export default Registration;