import React from 'react';
import { useNavigate } from 'react-router-dom';
import './UrlListItem.css';
import DeleteButton from '../common/DeleteButton';
import { getUserInfoFromToken, isAdmin } from '../../utils/auth';

interface UrlListItemProps {
    id: number;
    url: string;
    createdAt: string;
    authorId: string;
}

const UrlListItem: React.FC<UrlListItemProps> = ({ id, url, createdAt, authorId }) => {
    const navigate = useNavigate();
    const userInfo = getUserInfoFromToken();
    const currentUserSub = userInfo?.sub;
    const isAuthorized = authorId == currentUserSub?.toString();
    console.log(currentUserSub);
    console.log(authorId);

    return (
        <div className="url-list-item" onClick={() => navigate(`/url-details/${id}`)} style={{ cursor: 'pointer' }}>
            <div className="url-item-content">
                <p><strong>ID:</strong> {id}</p>
                <p><strong>URL:</strong> {url}</p>
                <p><strong>Created At:</strong> {new Date(createdAt).toLocaleString()}</p>
                {(isAuthorized || isAdmin()) && (
                    <DeleteButton
                        urlId={id}
                        onDeleteSuccess={() => navigate("/")}
                        onDeleteError={(message: string) => console.log("Error occurred: " + message)}
                    />
                )}
            </div>
        </div>
    );
};

export default UrlListItem;
