import { Link } from 'react-router-dom'

export default function Home() {
  const isLoggedIn = Boolean(localStorage.getItem('token'))

  return (
    <div className="flex flex-col items-center justify-center py-20 text-center">
      <h1 className="text-4xl font-bold text-gray-900 mb-4">Welcome to MyApp</h1>
      <p className="text-gray-500 text-lg mb-8 max-w-md">
        An ASP.NET + React starter template with JWT auth, EF Core, and Tailwind CSS.
      </p>
      {isLoggedIn ? (
        <Link
          to="/todos"
          className="bg-indigo-600 text-white px-6 py-3 rounded-lg hover:bg-indigo-700 transition"
        >
          Go to Todos →
        </Link>
      ) : (
        <div className="flex gap-3">
          <Link
            to="/login"
            className="border border-indigo-600 text-indigo-600 px-6 py-3 rounded-lg hover:bg-indigo-50 transition"
          >
            Log in
          </Link>
          <Link
            to="/register"
            className="bg-indigo-600 text-white px-6 py-3 rounded-lg hover:bg-indigo-700 transition"
          >
            Register
          </Link>
        </div>
      )}
    </div>
  )
}
