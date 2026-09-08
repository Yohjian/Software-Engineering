import { Link, NavLink, useNavigate } from 'react-router-dom'

export default function Layout({ children }: { children: React.ReactNode }) {
  const navigate = useNavigate()
  const isLoggedIn = Boolean(localStorage.getItem('token'))

  const handleLogout = () => {
    localStorage.removeItem('token')
    navigate('/login')
  }

  return (
    <div className="min-h-screen flex flex-col bg-gray-50">
      {/* ── Nav ── */}
      <header className="bg-white border-b border-gray-200 shadow-sm">
        <nav className="max-w-5xl mx-auto px-4 h-14 flex items-center justify-between">
          <Link to="/" className="text-lg font-semibold text-indigo-600 hover:text-indigo-700">
            MyApp
          </Link>
          <div className="flex items-center gap-4 text-sm">
            <NavLink
              to="/"
              className={({ isActive }) =>
                isActive ? 'font-medium text-indigo-600' : 'text-gray-600 hover:text-gray-900'
              }
            >
              Home
            </NavLink>
            {isLoggedIn ? (
              <>
                <NavLink
                  to="/todos"
                  className={({ isActive }) =>
                    isActive ? 'font-medium text-indigo-600' : 'text-gray-600 hover:text-gray-900'
                  }
                >
                  Todos
                </NavLink>
                <button
                  onClick={handleLogout}
                  className="text-gray-600 hover:text-gray-900"
                >
                  Log out
                </button>
              </>
            ) : (
              <>
                <NavLink
                  to="/login"
                  className={({ isActive }) =>
                    isActive ? 'font-medium text-indigo-600' : 'text-gray-600 hover:text-gray-900'
                  }
                >
                  Log in
                </NavLink>
                <NavLink
                  to="/register"
                  className="bg-indigo-600 text-white px-3 py-1.5 rounded-md hover:bg-indigo-700"
                >
                  Register
                </NavLink>
              </>
            )}
          </div>
        </nav>
      </header>

      {/* ── Main ── */}
      <main className="flex-1 max-w-5xl w-full mx-auto px-4 py-8">
        {children}
      </main>

      {/* ── Footer ── */}
      <footer className="border-t border-gray-200 py-4 text-center text-xs text-gray-400">
        © {new Date().getFullYear()} MyApp
      </footer>
    </div>
  )
}
