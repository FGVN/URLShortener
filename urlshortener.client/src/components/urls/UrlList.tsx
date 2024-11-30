import UrlListItem from './UrlListItem';
import AddUrlModal from '../common/AddUrlModal';
import useUrls from '../../hooks/useUrls';
import './UrlList.css';

const UrlList = () => {
    const {
        urls,
        error,
        currentPage,
        totalPages,
        isModalOpen,
        isLoggedIn,
        handleNextPage,
        handlePreviousPage,
        handleOpenModal,
        handleCloseModal,
        loadUrls,
    } = useUrls();

    return (
        <div className="url-list-container">
            {isLoggedIn && (
                <AddUrlModal isOpen={isModalOpen} onClose={handleCloseModal} onAdd={loadUrls} />
            )}
            <div className="url-list-header">
                <h2>Browse the list of URLs{isLoggedIn && ' or '}</h2>
                {isLoggedIn && (
                    <button onClick={handleOpenModal}>
                        Add URL
                    </button>
                )}
            </div>
            {error ? (
                <div style={{ color: 'red' }}>{error}</div>
            ) : urls.length === 0 ? (
                <p>No URLs found.</p>
            ) : (
                <div className="url-list-content">
                    {urls.map((url) => (
                        <UrlListItem key={url.id} {...url} />
                    ))}
                    <div className="pagination-controls">
                        <button onClick={handlePreviousPage} disabled={currentPage === 1}>
                            Previous
                        </button>
                        <span>
                            Page {currentPage} of {totalPages}
                        </span>
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
