import React, { useState, useEffect } from 'react';
import { jwtDecode } from 'jwt-decode';
import './Header.css';
import AddUrlModal from '../components/AddUrlModal'; // Adjust the path as needed

interface HeaderProps {
    isLoggedIn: boolean;
    authToken: string | null;
    onLoginClick: () => void;
    onRegisterClick: () => void;
}

interface JwtPayload {
    unique_name?: string; // Add any other claims you expect to access
}

const Header: React.FC<HeaderProps> = ({ isLoggedIn, authToken, onLoginClick, onRegisterClick }) => {
    const [isModalOpen, setIsModalOpen] = useState(false);
    const [username, setUsername] = useState<string | null>(null);

    useEffect(() => {
        if (isLoggedIn && authToken) {
            try {
                const decodedToken = jwtDecode<JwtPayload>(authToken);
                setUsername(decodedToken.unique_name || 'User');
            } catch (error) {
                console.error('Error decoding the token:', error);
                setUsername('User');
            }
        }
    }, [isLoggedIn, authToken]);

    const openModal = () => setIsModalOpen(true);
    const closeModal = () => setIsModalOpen(false);

    return (
        <header className="header">
            <h2 className="header-title">Hello, {isLoggedIn ? username : "Welcome!"}</h2>
            {isLoggedIn && (
                <div className="header-buttons">
                    <button onClick={openModal}>Add New URL</button>
                    <AddUrlModal isOpen={isModalOpen} onClose={closeModal} onAdd={(newUrl) => console.log(newUrl)} />
                </div>
            )}
            {!isLoggedIn && (
                <nav className="header-buttons">
                    <button onClick={onLoginClick}>Login</button>
                    <button onClick={onRegisterClick}>Register</button>
                </nav>
            )}
        </header>
    );
};

export default Header;
