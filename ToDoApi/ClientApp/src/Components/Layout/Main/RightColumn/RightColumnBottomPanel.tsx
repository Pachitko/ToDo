import React from 'react';
import styled from 'styled-components';
import { deleteTask, hideTaskDetailsAction } from 'src/redux/actions/taskActions';
import { useAppSelector } from 'src/redux/hooks';
import { SIconButtonFilled } from 'src/Components/UI';
import { useDispatch } from 'react-redux';
import { convertUTCDateToLocalDate } from 'src/libs/utils'

const RightColumnBottomPanel = () => {
    const activeTask = useAppSelector(state => state.tasks.activeTask)
    const dispatch = useDispatch();

    return (
        <SRightColumnBottomPanel>
            <SRightColumnBottomPanelButton onClick={() => dispatch(hideTaskDetailsAction())}>
                <i className="fa-solid fa-arrow-right-long"></i>
            </SRightColumnBottomPanelButton>
            <SCreatedAt>
                {activeTask?.createdAt
                    ? `Created ${convertUTCDateToLocalDate(new Date(activeTask.createdAt)).toLocaleDateString()}`
                    : null}
            </SCreatedAt>
            {activeTask !== null &&
                <SRightColumnBottomPanelButton onClick={() => dispatch(deleteTask(activeTask.toDoListId, activeTask.id))}>
                    <i className="fa-regular fa-trash-can"></i>
                </SRightColumnBottomPanelButton>
            }
        </ SRightColumnBottomPanel >
    );
}

export default RightColumnBottomPanel;

const SCreatedAt = styled.div`
    color: ${p => p.theme.colors.onSurface}
`

const SRightColumnBottomPanel = styled.div`
    display: flex;
    flex-direction: row;
    justify-content: space-between;
    align-items: center;
    height: 36px;
`

const SRightColumnBottomPanelButton = styled(SIconButtonFilled)`
    padding: 4px;
    display: flex;
    align-items: center;
    justify-content: center;
    &>i {
        font-size: 1rem;
    }
`