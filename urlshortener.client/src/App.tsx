// src/App.tsx
import React, { useState } from 'react';
import './App.css';
import LoginScreen from './components/LoginScreen';
import RegisterScreen from './components/RegisterScreen';
import Header from './components/Header';

function App() {
    const [isLoggedIn, setIsLoggedIn] = useState<boolean>(false);
    const [showRegister, setShowRegister] = useState<boolean>(false);

    return (
        <div>
            <Header
                isLoggedIn={isLoggedIn}
                onLoginClick={() => setShowRegister(false)}
                onRegisterClick={() => setShowRegister(true)}
            />
            {!isLoggedIn ? (
                showRegister ? (
                    <RegisterScreen onRegister={() => setIsLoggedIn(true)} />
                ) : (
                    <LoginScreen onLogin={() => setIsLoggedIn(true)} />
                )
            ) : (
                <div>
                    <h1>Welcome to the Dashboard</h1>
                    {/* You can add content for the authenticated user here */}
                </div>
            )}
        </div>
    );
}

export default App;
