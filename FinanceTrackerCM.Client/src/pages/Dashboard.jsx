import React, { useEffect, useMemo, useState } from 'react';
import {
  BarElement,
  CategoryScale,
  Chart as ChartJS,
  Legend,
  LinearScale,
  Tooltip
} from 'chart.js';
import { Bar } from 'react-chartjs-2';
import api from '../services/api';
import '../styles/dashboard.css';

ChartJS.register(CategoryScale, LinearScale, BarElement, Tooltip, Legend);

function currency(value) {
  return Number(value || 0).toLocaleString('pt-BR', { style: 'currency', currency: 'BRL' });
}

export default function Dashboard(){
  const today = new Date();
  const [month, setMonth] = useState(today.getMonth() + 1);
  const [year, setYear] = useState(today.getFullYear());
  const [resumo, setResumo] = useState({ saldoGeral: 0, receitas: 0, despesas: 0 });
  const [loading, setLoading] = useState(false);

  useEffect(() => {
    setLoading(true);
    api.get('/api/summary', { params: { month, year } })
      .then(r => setResumo(r.data))
      .catch(() => setResumo({ saldoGeral: 0, receitas: 0, despesas: 0 }))
      .finally(() => setLoading(false));
  }, [month, year]);

  const chartData = useMemo(() => ({
    labels: ['Receitas', 'Despesas', 'Saldo'],
    datasets: [
      {
        label: 'Resumo do periodo',
        data: [resumo.receitas, resumo.despesas, resumo.saldoGeral],
        backgroundColor: ['#27ae60', '#e74c3c', '#3498db']
      }
    ]
  }), [resumo]);

  async function downloadReport(type) {
    const extension = type === 'pdf' ? 'pdf' : 'xlsx';
    const responseType = 'blob';
    const res = await api.get(`/api/reports/monthly/${type}`, {
      params: { month, year },
      responseType
    });

    const url = window.URL.createObjectURL(new Blob([res.data]));
    const link = document.createElement('a');
    link.href = url;
    link.download = `relatorio_${year}_${String(month).padStart(2, '0')}.${extension}`;
    link.click();
    window.URL.revokeObjectURL(url);
  }

  return (
    <div className="page-container">
      <div className="dashboard-header">
        <div>
          <h2>Visao Geral</h2>
          <p>Acompanhe receitas, despesas e saldo do periodo selecionado.</p>
        </div>

        <div className="dashboard-filters">
          <select value={month} onChange={e => setMonth(Number(e.target.value))}>
            {Array.from({ length: 12 }, (_, index) => (
              <option key={index + 1} value={index + 1}>
                {String(index + 1).padStart(2, '0')}
              </option>
            ))}
          </select>
          <input type="number" value={year} onChange={e => setYear(Number(e.target.value))} />
        </div>
      </div>

      <div className="metrics-grid">
        <div className="card metric-card card-saldo">
          <span className="metric-title">Saldo do Periodo</span>
          <span className="metric-value">{currency(resumo.saldoGeral)}</span>
        </div>

        <div className="card metric-card card-receita">
          <span className="metric-title">Receitas</span>
          <span className="metric-value">+ {currency(resumo.receitas)}</span>
        </div>

        <div className="card metric-card card-despesa">
          <span className="metric-title">Despesas</span>
          <span className="metric-value">- {currency(resumo.despesas)}</span>
        </div>
      </div>

      <div className="card chart-section">
        <div className="section-header">
          <h3>Resumo mensal</h3>
          <div className="report-actions">
            <button onClick={() => downloadReport('pdf')}>PDF</button>
            <button onClick={() => downloadReport('excel')}>Excel</button>
          </div>
        </div>
        {loading ? (
          <p className="muted">Carregando dados...</p>
        ) : (
          <div className="chart-container">
            <Bar data={chartData} options={{ responsive: true, maintainAspectRatio: false }} />
          </div>
        )}
      </div>
    </div>
  );
}
