import { useEffect, useState } from 'react';
import { isAdmin } from '../../utils/auth';
import { getAboutUsContent, updateAboutUsContent } from '../../api/apiService';

const AboutUs: React.FC = () => {
    const [content, setContent] = useState('');
    const [isEditing, setIsEditing] = useState(false);
    const [newContent, setNewContent] = useState('');

    useEffect(() => {
        const fetchContent = async () => {
            try {
                const aboutUsContent = await getAboutUsContent();
                setContent(aboutUsContent);
                setNewContent(aboutUsContent);
            } catch (error) {
                alert('Failed to fetch About Us content');
            }
        };

        fetchContent();
    }, []);

    const handleEditClick = () => {
        setIsEditing(true);
    };

    const handleSaveClick = async () => {
        try {
            await updateAboutUsContent(newContent);
            alert('Content updated successfully!');
            setContent(newContent);
            setIsEditing(false);
        } catch (error) {
            alert('An error occurred while updating the content');
        }
    };

    return (
        <div className="about-us">
            {isEditing ? (
                <div className="edit-form">
                    <textarea
                        value={newContent}
                        onChange={(e) => setNewContent(e.target.value)}
                    />
                    <button onClick={handleSaveClick}>Save</button>
                    <button onClick={() => setIsEditing(false)}>Cancel</button>
                </div>
            ) : (
                <div>
                    <p>{content}</p>
                    {isAdmin() && (
                        <button onClick={handleEditClick}>Edit</button>
                    )}
                </div>
            )}
        </div>
    );
};

export default AboutUs;
