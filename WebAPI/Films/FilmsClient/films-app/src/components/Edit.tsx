import '../App.css';
import { useState, ChangeEvent, FormEvent, useEffect } from 'react';
import axios from 'axios';

interface Film
{
    id: number;
    name: string;
    director: string;
    genre: string;
    year: number;
    description: string;
    posterPath: string;
}

interface EditProps
{
    film: Film;
    onBack: () => void;
}

export function Edit({ film, onBack }: EditProps)
{
    const [formData, setFormData] = useState<Film>({ ...film });
    const [uploadedFile, setUploadedFile] = useState<File | null>(null);
    const [url, setUrl] = useState<string | null>(null);

    const handleChange = (e: ChangeEvent<HTMLInputElement>) =>
    {
        const { name, value } = e.target;
        setFormData(prev =>
        ({
            ...prev,
            [name]: name === 'year' ? Number(value) : value,
        }));
    };

    const handleFileChange = (e: ChangeEvent<HTMLInputElement>) =>
    {
        if (e.target.files && e.target.files[0])
        {
            const file = e.target.files[0];
            setUploadedFile(file);

            const objectUrl = URL.createObjectURL(file);
            setUrl(objectUrl);
        }
    };

    const handleSubmit = async (e: FormEvent) =>
    {
        e.preventDefault();
        const data = new FormData();
        data.append('Id', formData.id.toString());
        data.append('Name', formData.name);
        data.append('Director', formData.director);
        data.append('Genre', formData.genre);
        data.append('Year', formData.year.toString());
        data.append('Description', formData.description);
        if (uploadedFile)
        {
            data.append('uploaded', uploadedFile);
        }

        try
        {
            await axios.put(`http://localhost:5075/api/Film/${formData.id}`, data);
            onBack();
        }
        catch (error)
        {
            console.error('Error:', error);
        }
    };

    useEffect(() =>
    {
        return () =>
        {
            if (url)
            {
                URL.revokeObjectURL(url);
            }
        };
    }, [url]);

    return (
        <div className="block">
            <div className="formValue">
                <form onSubmit={handleSubmit}>
                    <label htmlFor="name">
                        Name
                        <input
                            id="name"
                            type="text"
                            name="name"
                            value={formData.name}
                            onChange={handleChange}
                            placeholder="Film Name"
                            required
                        />
                    </label>

                    <label htmlFor="director">
                        Director
                        <input
                            id="director"
                            type="text"
                            name="director"
                            value={formData.director}
                            onChange={handleChange}
                            placeholder="Director"
                            required
                        />
                    </label>

                    <label htmlFor="genre">
                        Genre
                        <input
                            id="genre"
                            type="text"
                            name="genre"
                            value={formData.genre}
                            onChange={handleChange}
                            placeholder="Genre"
                            required
                        />
                    </label>

                    <label htmlFor="year">
                        Year
                        <input
                            id="year"
                            type="number"
                            name="year"
                            value={formData.year}
                            onChange={handleChange}
                            placeholder="Year"
                            required
                        />
                    </label>

                    <label htmlFor="description">
                        Description
                        <textarea
                            id="description"
                            name="description"
                            value={formData.description}
                            onChange={handleChange}
                            placeholder="Description"
                            rows='10'
                            cols='25'
                            required
                        />
                    </label>

                    <label htmlFor="poster">
                        Poster
                        <input
                            id="poster"
                            type="file"
                            name="uploaded"
                            onChange={handleFileChange}
                            accept="image/*"
                        />
                    </label>

                    <div className="buttons">
                        <button type="submit">Save</button>
                        <button type="button" onClick={onBack}>Cancel</button>
                    </div>
                </form>
            </div>

            <div className="poster">
                <img
                    src={url || `http://localhost:5075/${formData.posterPath}`}
                    alt="poster"
                />
            </div>
        </div>
    );
}
