import React from 'react';
import { NavLink, Routes, Route, Navigate, useNavigate } from 'react-router-dom';
import Dashboard from './pages/Dashboard';
import Accounts from './pages/Accounts';
import Categories from './pages/Categories';
import Transactions from './pages/Transactions';
import { clearTokens, getAccessToken } from './services/auth';
import api from './services/api';

export default function App() {
  const token = getAccessToken();
  const navigate = useNavigate();

  async function logout() {
    try {
      await api.post('/api/auth/logout');
    } catch {
      // Local logout should still happen if the API is unavailable.
    }

    clearTokens();
    navigate('/login', { replace: true });
  }

  if (!token) return <Navigate to="/login" replace />;

  return (
    <div className="layout">
      <aside className="sidebar">
        <h2>FinanceTrackerCM</h2>
        <nav>
          <NavLink to="/">Dashboard</NavLink>
          <NavLink to="/accounts">Contas</NavLink>
          <NavLink to="/categories">Categorias</NavLink>
          <NavLink to="/transactions">Transacoes</NavLink>
        </nav>
        <button className="logout-button" onClick={logout}>Sair</button>
      </aside>
      <main className="content">
        <Routes>
          <Route path="/" element={<Dashboard />} />
          <Route path="/accounts" element={<Accounts />} />
          <Route path="/categories" element={<Categories />} />
          <Route path="/transactions" element={<Transactions />} />
        </Routes>
      </main>
    </div>
  );
}
