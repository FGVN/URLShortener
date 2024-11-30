import { useState, useEffect } from 'react';
import { fetchUrls, Url } from '../api/apiService';

interface FetchUrlsResponse {
    currentPage: number;
    pageSize: number;
    totalRecords: number;
    data: Url[];
}

const useUrls = () => {
    const [urls, setUrls] = useState<Url[]>([]);
    const [error, setError] = useState<string | null>(null);
    const [currentPage, setCurrentPage] = useState<number>(1);
    const [totalPages, setTotalPages] = useState<number>(1);
    const [isModalOpen, setIsModalOpen] = useState<boolean>(false);

    const isLoggedIn: boolean = !!localStorage.getItem('authToken');

    const loadUrls = async () => {
        try {
            const data: FetchUrlsResponse = await fetchUrls(currentPage, 5); 
            console.log('Fetched data:', data); 
            setUrls(data.data || []);
            setTotalPages(Math.ceil(data.totalRecords / data.pageSize)); 
        } catch (err) {
            console.error('Error fetching URLs:', err); 
            setError('Failed to load URLs. Please try again later.');
        }
    };

    const handleNextPage = () => {
        if (currentPage < totalPages) setCurrentPage((prev) => prev + 1);
    };

    const handlePreviousPage = () => {
        if (currentPage > 1) setCurrentPage((prev) => prev - 1);
    };

    const handleOpenModal = () => setIsModalOpen(true);
    const handleCloseModal = () => setIsModalOpen(false);

    useEffect(() => {
        loadUrls();
    }, [currentPage]);

    return {
        urls,
        error,
        currentPage,
        totalPages,
        isModalOpen,
        isLoggedIn,
        loadUrls,
        handleNextPage,
        handlePreviousPage,
        handleOpenModal,
        handleCloseModal,
    };
};

export default useUrls;
