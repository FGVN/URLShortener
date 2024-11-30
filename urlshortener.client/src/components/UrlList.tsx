import React, { useEffect, useState } from 'react';
import UrlListItem from './UrlListItem';
import { fetchUrls } from '../utils/axiosInstance';

interface UrlGlobalDto {
    id: number;
    url: string;
    createdAt: string;
}

const UrlList: React.FC = () => {
    const [urls, setUrls] = useState<UrlGlobalDto[]>([]);
    const [error, setError] = useState<string | null>(null);
    const [currentPage, setCurrentPage] = useState(1);
    const [pageSize] = useState(10); 
    const [totalPages, setTotalPages] = useState(1);

    useEffect(() => {
        const loadUrls = async () => {
            try {
                const data = await fetchUrls(currentPage, pageSize); 
                setUrls(data.data); 
                setTotalPages(data.totalPages); 
            } catch (err) {
                setError('Failed to load URLs. Please try again later.');
            }
        };

        loadUrls();
    }, [currentPage, pageSize]); 

    const handleNextPage = () => {
        if (currentPage < totalPages) {
            setCurrentPage((prevPage) => prevPage + 1);
        }
    };

    const handlePreviousPage = () => {
        if (currentPage > 1) {
            setCurrentPage((prevPage) => prevPage - 1);
        }
    };

    return (
        <div>
            <h2>Your URLs</h2>
            {error ? (
                <div style={{ color: 'red' }}>{error}</div>
            ) : urls.length === 0 ? (
                <p>No URLs found.</p>
            ) : (
                <div>
                    {urls.map((url) => (
                        <UrlListItem
                            key={url.id}
                            id={url.id}
                            url={url.url}
                            createdAt={url.createdAt}
                        />
                    ))}
                    <div style={{ marginTop: '20px' }}>
                        <button onClick={handlePreviousPage} disabled={currentPage === 1}>
                            Previous
                        </button>
                        <span style={{ margin: '0 10px' }}>Page {currentPage} of {totalPages}</span>
                        <button onClick={handleNextPage} disabled={currentPage === totalPages}>
                            Next
                        </button>
                    </div>
                </div>
            )}
        </div>
    );
};

export default UrlList;
