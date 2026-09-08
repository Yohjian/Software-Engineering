import { Link } from 'react-router-dom'

export default function NotFound() {
  return (
    <div className="flex flex-col items-center justify-center py-24 text-center">
      <p className="text-6xl font-bold text-indigo-100">404</p>
      <h2 className="mt-2 text-xl font-semibold text-gray-900">Page not found</h2>
      <p className="mt-2 text-sm text-gray-500">The page you're looking for doesn't exist.</p>
      <Link
        to="/"
        className="mt-6 rounded-md bg-indigo-600 px-4 py-2 text-sm font-semibold text-white hover:bg-indigo-700"
      >
        Go home
      </Link>
    </div>
  )
}
