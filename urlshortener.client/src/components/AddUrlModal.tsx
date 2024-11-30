import React, { FC, useState } from 'react';
import axiosInstance from '../utils/axiosInstance';
import './UrlDetails.css';

interface AddUrlModalProps {
    isOpen: boolean;
    onClose: () => void;
    onAdd: (newUrl: { OriginUrl: string; Url: string }) => void;
}

const AddUrlModal: FC<AddUrlModalProps> = ({ isOpen, onClose, onAdd }) => {
    const [originUrl, setOriginUrl] = useState('');
    const [url, setUrl] = useState('');

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        try {
            const response = await axiosInstance.post('/UrlShortener', {
                OriginUrl: originUrl,
                Url: url,
            });
            alert('URL added successfully!');
            onAdd(response.data); // Optional callback for updates
            onClose();
        } catch (error) {
            alert('Error adding URL. Please try again.');
        }
    };

    if (!isOpen) return null;

    return (
        <div className="modal-overlay">
            <div className="modal-content">
                <h2>Add New URL</h2>
                <form onSubmit={handleSubmit}>
                    <div className="form-group">
                        <label>Origin URL:</label>
                        <input
                            type="url"
                            value={originUrl}
                            onChange={(e) => setOriginUrl(e.target.value)}
                            required
                        />
                    </div>
                    <div className="form-group">
                        <label>Short URL:</label>
                        <input
                            type="text"
                            value={url}
                            onChange={(e) => setUrl(e.target.value)}
                            required
                        />
                    </div>
                    <button type="submit">Add URL</button>
                    <button type="button" onClick={onClose}>Close</button>
                </form>
            </div>
        </div>
    );
};

export default AddUrlModal;
