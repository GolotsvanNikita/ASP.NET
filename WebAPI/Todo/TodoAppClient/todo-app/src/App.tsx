import { useState, useEffect } from 'react';
import axios from 'axios';
import './App.css';

interface TodoItem
{
    id: number,
    name: string,
    isComplete: boolean
}

export function App()
{
    const [todos, setTodos] = useState<TodoItem[]>([]);

    useEffect(() =>
    {
        axios.get<TodoItem[]>('http://localhost:5277/api/Todo')
            .then(response =>
            {
                setTodos(response.data);
            })
            .catch(error =>
            {
                console.error('Error: ', error);
            })
    }, [])

    return(
        <div className='App'>
            <h1>Todo List</h1>
            <ul>
                {todos.map(todo =>
                    <li key={todo.id}>
                        {todo.name} - {todo.isComplete ? 'Complete' : 'Not Complete'}
                    </li>
                )}
            </ul>
        </div>
    )
}