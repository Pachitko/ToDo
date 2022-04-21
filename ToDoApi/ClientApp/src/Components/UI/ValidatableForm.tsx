import React, { BaseSyntheticEvent, FC, useState } from 'react';
import styled from 'styled-components'
import { SButton, PanelCss } from 'src/Components/UI'
import { SSummary, SAuthInputWrapper, STitle, SAuthInput, SInputFieldErrors } from '../LoginRegister/Auth';
import { IValidatableForm, validateForm } from 'src/libs/loginRegisterValidation';

interface Props {
    formInitialState: IValidatableForm
    onSubmit: (form: IValidatableForm) => void
}

const ValidateableForm: FC<Props> = ({ formInitialState, onSubmit, children }) => {
    const [form, setForm] = useState<IValidatableForm>(formInitialState)

    const handleInputChange = (e: BaseSyntheticEvent) => {
        const field = form.inputFields[e.target.id]
        field.value = e.target.value

        validateForm(form)
        setForm({ ...form })
    }

    const handleSubmit = (e: BaseSyntheticEvent) => {
        e.preventDefault();
        validateForm(form)

        if (form.isValid) {
            onSubmit(form)
        } else {
            setForm({ ...form })
        }
    }

    return (
        <SValidatableForm>
            <STitle>
                {form.title}
            </STitle>
            {Object.keys(form.inputFields).map(fieldName => {
                const field = form.inputFields[fieldName]
                const validationResult = form.validationResults[fieldName]
                const isValid = validationResult === undefined || validationResult.isValid
                let fieldErrors = validationResult ? validationResult.errors : []
                fieldErrors = fieldErrors.concat(form.additionalInputFieldErrors[fieldName])
                return (
                    <SAuthInputWrapper key={fieldName}>
                        <SInputFieldErrors>
                            {fieldErrors.map((e, i) => <div key={i}>{e}</div>)}
                        </SInputFieldErrors>
                        <SAuthInput isValid={isValid}
                            value={field.value}
                            id={field.id}
                            placeholder={field.label} type={field.htmlType}
                            onChange={handleInputChange}
                        />
                    </SAuthInputWrapper>
                )
            })}
            {form.summary &&
                <SSummary>
                    <span>{form.summary}</span>
                </SSummary>
            }
            <SButton type='submit' onClick={handleSubmit}>{formInitialState.submitText}</SButton>
            {children}
        </SValidatableForm>
    );
}

export default ValidateableForm;

const SValidatableForm = styled.form`
    ${PanelCss};
    width: 260px;
    display: flex;
    flex-direction: column;
    padding: ${props => props.theme.padding.small}px;
`