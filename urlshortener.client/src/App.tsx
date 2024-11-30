import { useEffect, useState } from 'react';
import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import './App.css';
import LoginScreen from './components/LoginScreen';
import RegisterScreen from './components/RegisterScreen';
import Header from './components/Header';
import UrlList from './components/UrlList';
import UrlDetails from './components/UrlDetails';

function App() {
    const [isLoggedIn, setIsLoggedIn] = useState<boolean>(false);
    const [showRegister, setShowRegister] = useState<boolean>(false);
    const [authToken, setAuthToken] = useState<string | null>(null);

    useEffect(() => {
        const token = localStorage.getItem('authToken');
        if (token) {
            setAuthToken(token);
            setIsLoggedIn(true);
        }
    }, []);

    return (
        <Router>
            <div>
                <Header
                    isLoggedIn={isLoggedIn}
                    authToken={authToken}
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
                    <Routes>
                        <Route path="/" element={<UrlList />} />
                        <Route path="/url-details/:id" element={<UrlDetails />} />
                    </Routes>
                )}
            </div>
        </Router>
    );
}

export default App;
