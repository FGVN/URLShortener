// src/components/RegisterScreen.tsx
import React, { useState } from 'react';
import { registerRequest } from '../../api/apiService'; // Import postRequest from apiService
import './RegisterScreen.css';
import { useNavigate } from 'react-router-dom';

interface RegisterScreenProps {
    onRegister: () => void;
}

const RegisterScreen: React.FC<RegisterScreenProps> = ({ onRegister }) => {
    const [email, setEmail] = useState('');
    const [username, setUsername] = useState('');
    const [passwordState, setPassword] = useState('');
    const [confirmPassword, setConfirmPassword] = useState('');
    const [error, setError] = useState<string | null>(null);
    const navigate = useNavigate();

    const handleRegister = async () => {
        if (passwordState !== confirmPassword) {
            setError('Passwords do not match.');
            return;
        }

        try {
            const response = await registerRequest({
                login: email,
                username,
                password: passwordState,
            });

            if (response.status === 201) {
                console.log('Registration successful:', response.data);
                const token = response.data.token;
                if (token) {
                    localStorage.setItem('authToken', token);
                }

                onRegister();
                navigate("/");
            } else {
                setError('Registration failed. Please try again.');
            }
        } catch (err) {
            console.error('Registration error:', err);
            setError('Registration failed. Please try again.');
        }
    };

    return (
        <div className="register-screen">
            <h2>Register</h2>
            <input
                type="text"
                placeholder="Username"
                value={username}
                onChange={(e) => setUsername(e.target.value)}
            />
            <input
                type="email"
                placeholder="Email"
                value={email}
                onChange={(e) => setEmail(e.target.value)}
            />
            <input
                type="password"
                placeholder="Password"
                value={passwordState}
                onChange={(e) => setPassword(e.target.value)}
            />
            <input
                type="password"
                placeholder="Confirm Password"
                value={confirmPassword}
                onChange={(e) => setConfirmPassword(e.target.value)}
            />
            {error && <div className="error-message">{error}</div>}
            <button onClick={handleRegister}>Register</button>
        </div>
    );
};

export default RegisterScreen;
