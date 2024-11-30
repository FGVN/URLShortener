import axiosInstance from './axiosInstance';

export interface Url {
    id: number;
    url: string;
    createdAt: string;
    authorId: string;
}

export interface FetchUrlsResponse {
    currentPage: number;
    pageSize: number;
    totalRecords: number;
    data: Url[];
}

export interface UrlMetadata {
    title?: string;
    description?: string;
    image?: string;
}

export interface UrlDetailsDto {
    id: number;
    url: string;
    originUrl: string;
    createdAt: string;
    metadata?: UrlMetadata;
    username?: string;
    authorId: number;
}

export const postRequest = async (url: string, data: object) => {
    try {
        const response = await axiosInstance.post(url, data);
        if (response.data && response.data.token) {
            localStorage.setItem('authToken', response.data.token);
            console.log('Token saved to localStorage successfully.');
        }
        return response;
    } catch (error) {
        console.error(`Failed to post data to ${url}:`, error);
        throw error;
    }
};

export const getRequest = async (url: string, params?: Record<string, any>) => {
    try {
        const response = await axiosInstance.get(url, { params });
        return response.data;
    } catch (error) {
        console.error(`Failed to fetch data from ${url}:`, error);
        throw error;
    }
};

export const deleteRequest = async (url: string) => {
    try {
        const response = await axiosInstance.delete(url);
        return response.data;
    } catch (error) {
        console.error(`Failed to delete data from ${url}:`, error);
        throw error;
    }
};

export const loginRequest = async (email: string, password: string) => {
    try {
        const response = await postRequest('/auth/login', { login: email, password });
        return response;
    } catch (error) {
        throw error;
    }
};

export const addUrlRequest = async (originUrl: string, url: string) => {
    try {
        const response = await postRequest('/UrlShortener', { OriginUrl: originUrl, Url: url });
        return response;
    } catch (error) {
        throw error;
    }
};

export const fetchUrlDetails = async (id: string): Promise<UrlDetailsDto> => {
    try {
        const response = await axiosInstance.get(`/UrlShortener/${id}`);
        response.data.url = import.meta.env.VITE_APP_REDIRECT_BASE_URL + response.data.url
        return response.data;
    } catch (error) {
        throw new Error('Failed to fetch URL details. Please try again later.');
    }
};

export const fetchUrls = async (page: number, pageSize: number): Promise<FetchUrlsResponse> => {
    try {
        const response = await getRequest('/UrlShortener', { page, pageSize });

        if (response.data && Array.isArray(response.data)) {
            response.data.forEach((urlObj: Url) => {
                urlObj.url = import.meta.env.VITE_APP_REDIRECT_BASE_URL + urlObj.url;
            });
        }

        return response;
    } catch (error) {
        console.error('Error fetching URLs:', error);
        throw error;
    }
};


export const deleteUrl = async (id: number) => {
    try {
        const response = await deleteRequest(`/UrlShortener/${id}`);
        return response.data;
    } catch (error) {
        console.error('Failed to delete URL:', error);
        throw error;
    }
};

export const getAboutUsContent = async (): Promise<string> => {
    try {
        const response = await axiosInstance.get('/aboutus');
        if (response.status === 200) {
            return response.data.content; 
        }
        throw new Error('Failed to fetch About Us content');
    } catch (error) {
        console.error('Error fetching About Us content:', error);
        throw error;
    }
};

export const updateAboutUsContent = async (content: string) => {
    try {
        const response = await postRequest('/aboutus', { content });
        return response.data;
    } catch (error) {
        console.error('Failed to update About Us content:', error);
        throw error;
    }
};
