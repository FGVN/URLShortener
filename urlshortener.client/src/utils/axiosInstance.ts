import axios from 'axios';
import { Cookies } from 'react-cookie'; // Import Cookies from react-cookie

// Create a Cookies instance
const cookies = new Cookies();

const axiosInstance = axios.create({
    baseURL: import.meta.env.VITE_APP_API_URL || 'http://localhost:3000',
    headers: {
        'Content-Type': 'application/json',
    },
});

axiosInstance.interceptors.request.use(
    async (config) => {
        // Access the cookie directly using the cookies instance
        const token = cookies.get('authToken');
        if (token) {
            config.headers['Authorization'] = `Bearer ${token}`;
        }
        return config;
    },
    (error) => {
        return Promise.reject(error);
    }
);

axiosInstance.interceptors.response.use(
    (response) => response,
    (error) => {
        if (error.response && error.response.status === 401) {
            console.error('Unauthorized access. Please log in.');
            // Remove the cookie on 401 response
            cookies.remove('authToken');
            window.location.href = '/login';
        } else {
            console.error('An error occurred:', error);
        }
        return Promise.reject(error);
    }
);

interface RegisterData {
    login: string;
    username: string;
    password: string;
}

export const postRequest = async (url: string, data: RegisterData) => {
    try {
        const response = await axiosInstance.post(url, data);
        if (response.data && response.data.token) {
            cookies.set('authToken', response.data.token, {
                path: '/',
                sameSite: 'lax',
                secure: process.env.NODE_ENV === 'production',
                httpOnly: false, 
            });
            console.log('Token cookie set successfully.');
        }

        return response;
    } catch (error) {
        throw error;
    }
};

export const axiosWithAuth = axiosInstance;

export default axiosInstance;
