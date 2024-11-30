import React, { useEffect, useState } from 'react';
import { useParams } from 'react-router-dom';
import { fetchUrlDetails, UrlDetailsDto } from '../../api/apiService';
import './UrlDetails.css';

const UrlDetails: React.FC = () => {
    const { id } = useParams<{ id: string }>();
    const [urlDetails, setUrlDetails] = useState<UrlDetailsDto | null>(null);
    const [error, setError] = useState<string | null>(null);


    useEffect(() => {
        const loadUrlDetails = async () => {
            try {
                if (id) {
                    const details = await fetchUrlDetails(id);
                    setUrlDetails(details);
                }
            } catch (err: any) {
                setError(err.message);
            }
        };

        loadUrlDetails();
    }, [id]);

    if (error) {
        return <div style={{ color: 'red' }}>{error}</div>;
    }

    if (!urlDetails) {
        return <div>Loading...</div>;
    }

    return (
        <div className="url-details-container">
            <h2>URL Details</h2>
            <div className="url-details-top">
                <p><strong>Short URL:</strong> {urlDetails.url}</p>
                <p><strong>Original URL:</strong> {urlDetails.originUrl}</p>

                {urlDetails.username && <p><strong>Author:</strong> {urlDetails.username}</p>}
                <p><strong>Created At:</strong> {new Date(urlDetails.createdAt).toLocaleString()}</p>
            </div>

            {urlDetails.metadata && (
                <div>
                    <h3>Metadata</h3>
                    <div className="url-details-metadata">
                        <div className="metadata-info">
                            {urlDetails.metadata.title && <p><strong>Title:</strong> {urlDetails.metadata.title}</p>}
                            {urlDetails.metadata.description && <p><strong>Description:</strong> {urlDetails.metadata.description}</p>}
                        </div>
                        {urlDetails.metadata.image && (
                            <div className="metadata-image">
                                <img
                                    src={urlDetails.metadata.image}
                                    alt="URL Metadata"
                                    style={{ maxWidth: '300px', maxHeight: '200px' }}
                                />
                            </div>
                        )}
                    </div>
                </div>
            )}
        </div>
    );
};

export default UrlDetails;
