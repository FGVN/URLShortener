import React from 'react';
import { deleteUrl } from '../../api/apiService';

interface DeleteButtonProps {
    urlId: number;
    onDeleteSuccess: () => void;
    onDeleteError: (message: string) => void;
}

const DeleteButton: React.FC<DeleteButtonProps> = ({ urlId, onDeleteSuccess, onDeleteError }) => {
    const handleDelete = async () => {
        try {
            await deleteUrl(urlId);
            alert('URL deleted successfully.');
            onDeleteSuccess();
        } catch (error) {
            console.error('Failed to delete URL:', error);
            alert('Failed to delete the URL. Please try again later.');
            onDeleteError('Failed to delete the URL. Please try again later.');
        }
    };

    return (
        <button onClick={handleDelete}>Delete URL</button>
    );
};

export default DeleteButton;
