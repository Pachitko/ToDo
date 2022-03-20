import React, { BaseSyntheticEvent, FC, useState } from 'react';
import { SButton } from 'src/Components/UI'
import { loginAsync } from 'src/redux/actions/userActions';
import { useDispatch } from 'react-redux';
import { SAuthInput, SAuthInputWrapper, SAuthError, STitle } from './Auth';
import { FormInputFieldIds, IValidatableForm, validateForm } from 'src/libs/loginRegisterValidation';
import { useAppSelector } from 'src/redux/hooks';

const Login: FC = () => {
    const dispatch = useDispatch()
    const loginError = useAppSelector(state => state.user.loginError)
    const isLogging = useAppSelector(state => state.user.isLogging)

    const [form, setForm] = useState<IValidatableForm>({
        title: "LOGIN",
        isValid: false,
        validationResults: {},
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
        },
    })

    const handleInputChange = (e: BaseSyntheticEvent) => {
        const field = form.inputFields[e.target.id]
        field.value = e.target.value

        validateForm(form)
        setForm({ ...form })
    }

    const handleLogin = (e: BaseSyntheticEvent) => {
        e.preventDefault();
        validateForm(form)
        if (form.isValid) {
            dispatch(loginAsync(form.inputFields[FormInputFieldIds.Username].value,
                form.inputFields[FormInputFieldIds.Password].value))
        } else {
            setForm({ ...form })
        }
    }

    if (isLogging) {
        return <STitle>Logging</STitle>
    }

    return (
        <>
            <STitle>
                {form.title}
            </STitle>
            {Object.keys(form.inputFields).map(fieldName => {
                const field = form.inputFields[fieldName]
                const validationResult = form.validationResults[fieldName]
                const isValid = validationResult === undefined || validationResult.isValid
                return (
                    <SAuthInputWrapper key={field.id}>
                        <SAuthError>
                            {validationResult &&
                                validationResult.errors.map((e, i) => <span key={i}>{e}</span>)}
                        </SAuthError>
                        <SAuthInput isValid={isValid}
                            value={field.value} id={field.id}
                            placeholder={field.label} type={field.htmlType}
                            onChange={handleInputChange}
                        />
                    </SAuthInputWrapper>
                )
            })}
            {loginError &&
                <SAuthError>
                    <span>Invalid email or password</span>
                </SAuthError>}
            <SButton type='submit' onClick={handleLogin}>Log in</SButton>
        </>
    );
}

export default Login