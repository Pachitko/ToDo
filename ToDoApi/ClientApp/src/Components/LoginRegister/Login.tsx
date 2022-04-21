import React, { FC } from 'react';
import { login } from 'src/redux/actions/authActions';
import { useDispatch } from 'react-redux';
import { FormInputFieldIds, IValidatableForm } from 'src/libs/loginRegisterValidation';
import { useAppSelector } from 'src/redux/hooks';
import GoogleLoginButton from '../UI/GoogleLoginButton';
import ValidateableForm from '../UI/ValidatableForm';
import { NavLink } from 'react-router-dom';
import { SAdditionalPanel, SSeparator } from './Auth';

const Login: FC = () => {
    const dispatch = useDispatch()
    const loginError = useAppSelector(state => state.auth.loginError)

    const formInitialState: IValidatableForm = {
        title: "LOGIN",
        submitText: "Log in",
        isValid: false,
        validationResults: {},
        additionalInputFieldErrors: {},
        summary: loginError?.message,
        inputFields: {
            [`${FormInputFieldIds.Username}`]: {
                id: FormInputFieldIds.Username,
                value: "",
                htmlType: "text",
                label: "Username"
            },
            [`${FormInputFieldIds.Password}`]: {
                id: FormInputFieldIds.Password,
                value: "",
                htmlType: "password",
                label: "Password"
            }
        }
    }

    const handleSubmit = (form: IValidatableForm) => {
        dispatch(login(form.inputFields[FormInputFieldIds.Username].value,
            form.inputFields[FormInputFieldIds.Password].value))
    }

    return (
        <>
            <ValidateableForm
                formInitialState={formInitialState}
                onSubmit={handleSubmit}
                children={
                    <GoogleLoginButton />
                }
            />
            <SSeparator />
            <SAdditionalPanel>
                <NavLink to={"/auth/registration"}>Registration</NavLink>
            </SAdditionalPanel>
        </>
    );
}

export default Login