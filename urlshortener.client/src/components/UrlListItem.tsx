import React from 'react';
import { useNavigate } from 'react-router-dom';
import './UrlListItem.css'; 

interface UrlListItemProps {
    id: number;
    url: string;
    createdAt: string;
}

const UrlListItem: React.FC<UrlListItemProps> = ({ id, url, createdAt }) => {
    const navigate = useNavigate();

    const handleClick = () => {
        navigate(`/url-details/${id}`);
    };

    return (
        <div className="url-list-item" onClick={handleClick} style={{ cursor: 'pointer' }}>
            <div className="url-item-content">
                <p><strong>ID:</strong> {id}</p>
                <p><strong>URL:</strong> {url}</p>
                <p><strong>Created At:</strong> {new Date(createdAt).toLocaleString()}</p>
            </div>
        </div>
    );
};

export default UrlListItem;
