import { useEffect, useState, FormEvent } from 'react'
import { todosApi, TodoItem } from '../api/client'

export default function Todos() {
  const [todos, setTodos] = useState<TodoItem[]>([])
  const [newTitle, setNewTitle] = useState('')
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState<string | null>(null)

  useEffect(() => {
    todosApi
      .getAll()
      .then(({ data }) => setTodos(data))
      .catch(() => setError('Failed to load todos.'))
      .finally(() => setLoading(false))
  }, [])

  const handleCreate = async (e: FormEvent) => {
    e.preventDefault()
    if (!newTitle.trim()) return
    const { data } = await todosApi.create(newTitle.trim())
    setTodos([data, ...todos])
    setNewTitle('')
  }

  const handleToggle = async (todo: TodoItem) => {
    const { data } = await todosApi.update(todo.id, todo.title, !todo.isCompleted)
    setTodos(todos.map((t) => (t.id === data.id ? data : t)))
  }

  const handleDelete = async (id: number) => {
    await todosApi.delete(id)
    setTodos(todos.filter((t) => t.id !== id))
  }

  return (
    <div className="max-w-xl mx-auto">
      <h2 className="text-2xl font-bold text-gray-900 mb-6">My Todos</h2>

      {/* Add new */}
      <form onSubmit={handleCreate} className="flex gap-2 mb-8">
        <input
          type="text"
          placeholder="What needs doing?"
          value={newTitle}
          onChange={(e) => setNewTitle(e.target.value)}
          className="flex-1 rounded-md border border-gray-300 px-3 py-2 text-sm focus:outline-none focus:ring-2 focus:ring-indigo-500"
        />
        <button
          type="submit"
          className="rounded-md bg-indigo-600 px-4 py-2 text-sm font-semibold text-white hover:bg-indigo-700"
        >
          Add
        </button>
      </form>

      {loading && <p className="text-gray-400 text-sm">Loading…</p>}
      {error && <p className="text-red-600 text-sm">{error}</p>}

      <ul className="flex flex-col gap-2">
        {todos.map((todo) => (
          <li
            key={todo.id}
            className="flex items-center gap-3 rounded-lg border border-gray-200 bg-white px-4 py-3 shadow-sm"
          >
            <input
              type="checkbox"
              checked={todo.isCompleted}
              onChange={() => handleToggle(todo)}
              className="h-4 w-4 accent-indigo-600"
            />
            <span
              className={`flex-1 text-sm ${
                todo.isCompleted ? 'line-through text-gray-400' : 'text-gray-800'
              }`}
            >
              {todo.title}
            </span>
            <button
              onClick={() => handleDelete(todo.id)}
              className="text-xs text-red-400 hover:text-red-600"
            >
              Delete
            </button>
          </li>
        ))}
        {!loading && todos.length === 0 && (
          <p className="text-center text-sm text-gray-400 py-8">No todos yet. Add one above!</p>
        )}
      </ul>
    </div>
  )
}
