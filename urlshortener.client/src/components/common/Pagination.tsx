import React from 'react';

interface PaginationProps {
    currentPage: number;
    totalPages: number;
    onNextPage: () => void;
    onPreviousPage: () => void;
}

const Pagination: React.FC<PaginationProps> = ({ currentPage, totalPages, onNextPage, onPreviousPage }) => (
    <div style={{ marginTop: '20px', display: 'flex', justifyContent: 'center' }}>
        <button onClick={onPreviousPage} disabled={currentPage === 1}>
            Previous
        </button>
        <span style={{ margin: '0 10px' }}>
            Page {currentPage} of {totalPages}
        </span>
        <button onClick={onNextPage} disabled={currentPage === totalPages}>
            Next
        </button>
    </div>
);

export default Pagination;
