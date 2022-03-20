import React from 'react';
import { SIconButton } from 'src/Components/UI';
import { ITask } from 'src/redux/reducers/tasks';
import styled from 'styled-components'
import { SSectionBody, SSectionIcon } from './TaskDetails';

const RecurenceEditor = ({ activeTask }: { activeTask: ITask }) => {
    const handleClick = () => {
        console.log(1);
    }
    console.log(2);

    return (
        <SRecurenceEditor onClick={handleClick}>
            <SSectionIcon>
                <i className="fa-regular fa-calendar"></i>
            </SSectionIcon>
            <SSectionBody>
                <RecurenceEditor activeTask={activeTask} />
            </SSectionBody>
            <SIconButton>
                <i className="fa-solid fa-times"></i>
            </SIconButton>
        </SRecurenceEditor >
    );
}

export default RecurenceEditor;

const SRecurenceEditor = styled.div`
    padding: 8px;
    display: flex;
`