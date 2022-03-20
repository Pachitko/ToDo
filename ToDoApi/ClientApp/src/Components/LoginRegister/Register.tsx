import React, { BaseSyntheticEvent, FC, useState } from 'react';
import { Navigate } from 'react-router-dom';
import { SButton } from 'src/Components/UI'
import { SAuthError, SAuthInput, SAuthInputWrapper, STitle } from './Auth';
import { registerUserAsync } from 'src/redux/actions/userActions';
import { useDispatch } from 'react-redux';
import { IUserToRegister } from 'src/libs/api';
import { FormInputFieldIds, IValidatableForm, validateForm } from 'src/libs/loginRegisterValidation';
import { useAppSelector } from 'src/redux/hooks';

const Register: FC = () => {
    const dispatch = useDispatch();
    const registrationErrors = useAppSelector(state => state.user.registrationErrors)
    const isRegistering = useAppSelector(state => state.user.isRegistering)

    const [form, setForm] = useState<IValidatableForm>({
        title: "REGISTRATION",
        isValid: false,
        validationResults: {},
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
    })

    const handleInputChange = (e: BaseSyntheticEvent) => {
        const field = form.inputFields[e.target.id]
        field.value = e.target.value

        validateForm(form)
        setForm({ ...form })
    }

    if (isRegistering) {
        return <STitle>Registering</STitle>
    }

    // if (Object.keys(registrationErrors).length === 0 && !isRegistering) {
    //     console.log(1);
    //     return <Navigate to={"/login"} />
    // }

    const handleRegistration = (e: BaseSyntheticEvent) => {
        e.preventDefault();
        validateForm(form)
        if (form.isValid) {
            const userToRegister: IUserToRegister = {
                username: form.inputFields[FormInputFieldIds.Username].value,
                email: form.inputFields[FormInputFieldIds.Email].value,
                password: form.inputFields[FormInputFieldIds.Password].value,
                passwordConfirmation: form.inputFields[FormInputFieldIds.PasswordConfirmation].value
            }
            dispatch(registerUserAsync(userToRegister))
        } else {
            setForm({ ...form })
        }
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
                    <SAuthInputWrapper key={fieldName}>
                        <SAuthError>
                            {validationResult &&
                                validationResult.errors.concat(registrationErrors[fieldName])
                                    .map((e, i) => <span key={i}>{e}</span>)}
                        </SAuthError>
                        <SAuthInput isValid={isValid}
                            value={field.value} id={field.id}
                            placeholder={field.label} type={field.htmlType}
                            onChange={handleInputChange}
                        />
                    </SAuthInputWrapper>
                )
            })}
            <SButton type='submit' onClick={handleRegistration}>Register</SButton>
        </>
    );
}

export default Register;