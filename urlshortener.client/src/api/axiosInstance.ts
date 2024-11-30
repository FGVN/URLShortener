import axios from 'axios';

const axiosInstance = axios.create({
    baseURL: import.meta.env.VITE_APP_API_URL || 'http://localhost:3000',
    headers: {
        'Content-Type': 'application/json',
    },
});

axiosInstance.interceptors.request.use(
    (config) => {
        // Retrieve the token from localStorage
        const token = localStorage.getItem('authToken');
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
            // Remove the token from localStorage on 401 response
            localStorage.removeItem('authToken');
            window.location.href = '/login';
        } else {
            console.error('An error occurred:', error);
        }
        return Promise.reject(error);
    }
);

export const postRequest = async (url: string, data: object) => {
    try {
        const response = await axiosInstance.post(url, data);
        if (response.data && response.data.token) {
            localStorage.setItem('authToken', response.data.token);
            console.log('Token saved to localStorage successfully.');
        }

        return response;
    } catch (error) {
        throw error;
    }
};

export const axiosWithAuth = axiosInstance;

export default axiosInstance;
