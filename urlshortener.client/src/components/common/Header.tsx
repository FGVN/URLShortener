import React, { useState, useEffect } from 'react';
import { jwtDecode } from 'jwt-decode';
import { useNavigate } from 'react-router-dom';
import './Header.css';

interface HeaderProps {
    isLoggedIn: boolean;
    authToken: string | null;
    onLoginClick: () => void;
    onRegisterClick: () => void;
}

interface JwtPayload {
    unique_name?: string; 
}

const Header: React.FC<HeaderProps> = ({ isLoggedIn, onLoginClick, onRegisterClick }) => {
    const [username, setUsername] = useState<string | null>(null);
    const authToken = localStorage.getItem('authToken');
    const navigate = useNavigate();

    const handleLoginClick = () => {
        navigate('/login');
        onLoginClick();
    };

    const handleRegisterClick = () => {
        navigate('/register');
        onRegisterClick();
    };

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

    return (
        <header className="header">
            <h2 className="header-title">Hello, {isLoggedIn ? username : "Guest!"}</h2>

            <p><a href="/about-us">About us</a></p>
            {!isLoggedIn && (
                <nav className="header-buttons">
                    <button onClick={handleLoginClick}>Login</button>
                    <button onClick={handleRegisterClick}>Register</button>
                </nav>
            )}
        </header>
    );
};

export default Header;
