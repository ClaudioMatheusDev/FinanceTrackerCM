import React from 'react';
import { NavLink, Routes, Route, Navigate } from 'react-router-dom';
import Dashboard from './pages/Dashboard';
import Accounts from './pages/Accounts';
import Categories from './pages/Categories';
import Transactions from './pages/Transactions';
import { getAccessToken } from './services/auth';

export default function App() {
  const token = getAccessToken();
  if (!token) return <Navigate to="/login" replace />;

  return (
    <div className="layout">
      <aside className="sidebar">
        <h2>FinanceTrackerCM</h2>
        <nav>
          <NavLink to="/">Dashboard</NavLink>
          <NavLink to="/accounts">Contas</NavLink>
          <NavLink to="/categories">Categorias</NavLink>
          <NavLink to="/transactions">Transações</NavLink>
        </nav>
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