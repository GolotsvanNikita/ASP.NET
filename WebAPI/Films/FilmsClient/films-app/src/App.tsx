import {type FormEvent, useEffect, useState} from 'react'
import './App.css'
import axios from "axios";
import {Edit} from "./components/Edit.tsx";
import {Create} from "./components/Create.tsx";

interface Film
{
    id: number,
    name: string,
    director: string,
    genre: string,
    year: number,
    description: string,
    posterPath: string
}

function App()
{
    const [films, setFilms] = useState<Film[]>([]);
    const [edit, setEdit] = useState<Film | null>(null);
    const [create, setCreate] = useState<boolean | null>(null);

    useEffect(() =>
    {
        axios.get<Film[]>('http://localhost:5075/app/Film')
            .then(response =>
            {
                setFilms(response.data);
            })
            .catch(error =>
            {
                console.error('Error: ', error);
            })
    }, films);

    const handleBack = () =>
    {
        setEdit(null);
        setCreate(null);
    }

    if (edit)
    {
        return <Edit film={edit} onBack={handleBack} />;
    }

    if (create)
    {
        return <Create onBack={handleBack}/>;
    }

    const handleDelete = (e : FormEvent, id : number) =>
    {
        e.preventDefault();
        let isConfirm : boolean = confirm('Delete?');
        if (isConfirm)
        {
            axios.delete(`http://localhost:5075/app/Film/${id}`)
                .catch(error =>
                {
                    console.error('Error: ', error);
                });
        }
    }

    films.forEach(film => console.log(film.posterPath));

    return(
        <div className='App'>
            <button onClick={() => setCreate(true)}>Create film</button>
            <div className='container'>
                {films.map((film) =>
                (
                    <div className='card' key={film.id}>
                        <img src={`http://localhost:5075/${film.posterPath}`} alt="poster"/>
                        <div className='elements'>
                            <p>{film.name}({film.year})</p>
                            <p>{film.genre}</p>
                            <p>{film.director}</p>
                            <p className='last'>{film.description}</p>
                        </div>
                        <div className='controls'>
                            <button onClick={() => setEdit(film)}>Edit</button>
                            <button onClick={(event) => handleDelete(event, film.id)}>Delete</button>
                        </div>
                    </div>
                ))}
            </div>
        </div>
    )
}

export default App
