import React, { FC } from 'react';
import { NavLink } from 'react-router-dom';
import { useDispatch } from 'react-redux';
import { FormInputFieldIds, IValidatableForm } from 'src/libs/loginRegisterValidation';
import { useAppSelector } from 'src/redux/hooks';
import { registerWithExternalProvider } from 'src/redux/actions/userActions';
import ValidateableForm from '../UI/ValidatableForm';

const ExternalProviderRegistration: FC = () => {
    const dispatch = useDispatch();
    const registrationErrors = useAppSelector(state => state.user.registrationErrors)
    const externalLoginPayload = useAppSelector(state => state.user.externalLoginPayload)

    const formInitialState: IValidatableForm = {
        title: `${externalLoginPayload?.providerName.toUpperCase()} REGISTRATION`,
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
            }
        },
    }

    const handleSubmit = (form: IValidatableForm) => {
        dispatch(registerWithExternalProvider(
            {
                providerName: externalLoginPayload ? externalLoginPayload.providerName : "",
                username: form.inputFields[FormInputFieldIds.Username].value,
                tokenId: externalLoginPayload ? externalLoginPayload.tokenId : "",
            }
        ))
    }

    return (
        <ValidateableForm
            formInitialState={formInitialState}
            onSubmit={handleSubmit}
            additionalPanelContent={
                <>
                    <NavLink to={"/auth/registration"}>Registration</NavLink>
                    <NavLink to={"/auth/login"}>Log in</NavLink>
                </>
            }
        />
    );
}

export default ExternalProviderRegistration;