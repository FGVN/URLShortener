import React, { useState } from 'react';
import { postRequest } from '../utils/axiosInstance';
import { useCookies } from 'react-cookie';

interface RegisterScreenProps {
    onRegister: () => void;
}

const RegisterScreen: React.FC<RegisterScreenProps> = ({ onRegister }) => {
    const [email, setEmail] = useState(''); // Set initial state from props or leave empty
    const [username, setUsername] = useState(''); // Add state for username
    const [passwordState, setPassword] = useState('');
    const [confirmPassword, setConfirmPassword] = useState('');
    const [error, setError] = useState<string | null>(null);

    // Use the `useCookies` hook
    const [cookies, setCookie] = useCookies(['authToken']);

    const handleRegister = async () => {
        if (passwordState !== confirmPassword) {
            setError('Passwords do not match.');
            return;
        }

        try {
            const response = await postRequest('/auth/register', {
                login: email,
                username,
                password: passwordState,
            });

            if (response.status === 201) {
                console.log('Registration successful:', response.data);

                // Store the token in a cookie using setCookie
                const token = response.data.token;
                if (token) {
                    setCookie('authToken', token, { path: '/', maxAge: 60 * 60 * 24 * 7 }); // Expires in 7 days
                }

                onRegister(); // Call the onRegister callback
            } else {
                setError('Registration failed. Please try again.');
            }
        } catch (err) {
            console.error('Registration error:', err);
            setError('Registration failed. Please try again.');
        }
    };

    return (
        <div>
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
            {error && <div style={{ color: 'red' }}>{error}</div>}
            <button onClick={handleRegister}>Register</button>
        </div>
    );
};

export default RegisterScreen;
