import React, { useState } from 'react';
import { loginRequest } from '../../api/apiService'; // Updated import path
import './LoginScreen.css'; // Add this for the external CSS file
import { useNavigate } from 'react-router-dom';

interface LoginScreenProps {
    onLogin: () => void;
}

const LoginScreen: React.FC<LoginScreenProps> = ({ onLogin }) => {
    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');
    const [error, setError] = useState<string | null>(null);
    const navigate = useNavigate();

    const handleLogin = async () => {
        try {
            const response = await loginRequest(email, password);

            if (response.status === 200) {
                console.log('Login successful:', response.data);

                // Token handling is already done in the `postRequest` function
                onLogin();
                navigate("/");
            } else {
                setError('Login failed. Please check your credentials and try again.');
            }
        } catch (err) {
            console.error('Login error:', err);
            setError('Login failed. Please try again.');
        }
    };

    return (
        <div className="login-screen">
            <h2>Login</h2>
            <input
                type="email"
                placeholder="Login"
                value={email}
                onChange={(e) => setEmail(e.target.value)}
            />
            <input
                type="password"
                placeholder="Password"
                value={password}
                onChange={(e) => setPassword(e.target.value)}
            />
            {error && <div className="error-message">{error}</div>}
            <button onClick={handleLogin}>Login</button>
        </div>
    );
};

export default LoginScreen;
