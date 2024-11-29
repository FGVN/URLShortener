import React from 'react';

interface HeaderProps {
    isLoggedIn: boolean;
    onLoginClick: () => void;
    onRegisterClick: () => void;
}

const Header: React.FC<HeaderProps> = ({ isLoggedIn, onLoginClick, onRegisterClick }) => {
    return (
        <header>
            {isLoggedIn ? (
                <h2>Hello, User!</h2>
            ) : (
                <nav>
                    <button onClick={onLoginClick}>Login</button>
                    <button onClick={onRegisterClick}>Register</button>
                </nav>
            )}
        </header>
    );
};

export default Header;
