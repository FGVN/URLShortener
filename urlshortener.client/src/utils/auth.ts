export const getUserIdFromToken = (): number | null => {
    const token = localStorage.getItem('authToken');
    if (token) {
        try {
            const payload = JSON.parse(atob(token.split('.')[1]));
            return Number(payload.sub);
        } catch (error) {
            console.error('Error decoding token:', error);
        }
    }
    return null;
};

export interface UserTokenPayload {
    sub: string;
    role: string;
    [key: string]: any;
}

export const getUserInfoFromToken = (): UserTokenPayload | null => {
    const token = localStorage.getItem('authToken');
    if (token) {
        try {
            const payload = JSON.parse(atob(token.split('.')[1]));
            return {
                sub: payload.sub,
                role: payload["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"]
            };
        } catch (error) {
            console.error('Error decoding token:', error);
        }
    }
    return null;
};

export const isAdmin = (): boolean => {
    const userInfo = getUserInfoFromToken();
    return userInfo ? userInfo.role === 'Admins' : false;
};

