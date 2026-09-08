import axios from 'axios'

// Base URL: in dev the Vite proxy forwards /api → https://localhost:5001
// In production set VITE_API_BASE_URL to your deployed API origin.
const baseURL = import.meta.env.VITE_API_BASE_URL ?? '/api'

const client = axios.create({
  baseURL,
  headers: { 'Content-Type': 'application/json' },
})

// Attach JWT from localStorage to every request
client.interceptors.request.use((config) => {
  const token = localStorage.getItem('token')
  if (token) {
    config.headers.Authorization = `Bearer ${token}`
  }
  return config
})

// On 401, clear token and redirect to login
client.interceptors.response.use(
  (response) => response,
  (error) => {
    if (error.response?.status === 401) {
      localStorage.removeItem('token')
      window.location.href = '/login'
    }
    return Promise.reject(error)
  },
)

export default client

// ── Typed API helpers ─────────────────────────────────────────────────────────

export interface TodoItem {
  id: number
  title: string
  isCompleted: boolean
  createdAt: string
}

export interface AuthResponse {
  token: string
  email: string
}

export const authApi = {
  register: (email: string, password: string) =>
    client.post<{ message: string }>('/auth/register', { email, password }),

  login: (email: string, password: string) =>
    client.post<AuthResponse>('/auth/login', { email, password }),
}

export const todosApi = {
  getAll: () => client.get<TodoItem[]>('/todos'),
  getById: (id: number) => client.get<TodoItem>(`/todos/${id}`),
  create: (title: string) => client.post<TodoItem>('/todos', { title }),
  update: (id: number, title: string, isCompleted: boolean) =>
    client.put<TodoItem>(`/todos/${id}`, { title, isCompleted }),
  delete: (id: number) => client.delete(`/todos/${id}`),
}
