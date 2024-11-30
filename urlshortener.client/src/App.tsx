import { useEffect, useState } from 'react';
import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import './App.css';
import LoginScreen from './components/auth/LoginScreen';
import RegisterScreen from './components/auth/RegisterScreen';
import Header from './components/common/Header';
import UrlList from './components/urls/UrlList';
import UrlDetails from './components/urls/UrlDetails';
import AboutUs from './components/common/AboutUs';

function App() {
    const [isLoggedIn, setIsLoggedIn] = useState<boolean>(false);
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
                    onLoginClick={() => setIsLoggedIn(false)}
                    onRegisterClick={() => setIsLoggedIn(false)}
                />
                <Routes>
                    {!isLoggedIn && (
                        <>
                            <Route path="/login" element={<LoginScreen onLogin={() => setIsLoggedIn(true)} />} />
                            <Route path="/register" element={<RegisterScreen onRegister={() => setIsLoggedIn(true)} />} />
                        </>
                    ) 
                    }
                    <Route path="*" element={<UrlList />} />
                    <Route path="/url-details/:id" element={<UrlDetails />} />
                    <Route path="/about-us" element={<AboutUs/>} />
                </Routes>
            </div>
        </Router>
    );
}

export default App;
