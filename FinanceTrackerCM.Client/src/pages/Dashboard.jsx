import React, { useEffect, useState } from 'react'
import api from '../services/api'
import '../styles/dashboard.css'

export default function Dashboard(){
  const [resumo, setResumo] = useState({ saldoGeral: 0, receitas: 0.00, despesas: 0.00 })

  useEffect(()=>{
    api.get('/api/summary')
      .then(r => setResumo(r.data))
      .catch(()=>{})
  },[])

  return (
    <div className="page-container">
      <div className="dashboard-header">
        <h2>Visão Geral</h2>
        <p>Acompanhe a saúde financeira do seu negócio em tempo real.</p>
      </div>

      {/* Grid de Cards de Indicadores */}
      <div className="metrics-grid">
        <div className="card metric-card card-saldo">
          <span className="metric-title">Saldo Geral</span>
          <span className="metric-value">R$ {resumo.saldoGeral.toFixed(2)}</span>
        </div>

        <div className="card metric-card card-receita">
          <span className="metric-title">Receitas do Mês</span>
          <span className="metric-value">+ R$ {resumo.receitas.toFixed(2)}</span>
        </div>

        <div className="card metric-card card-despesa">
          <span className="metric-title">Despesas do Mês</span>
          <span className="metric-value">- R$ {resumo.despesas.toFixed(2)}</span>
        </div>
      </div>

      {/* Área do Gráfico */}
      <div className="card chart-section">
        <h3>Gráficos Mensais e Resumo</h3>
        <div className="chart-placeholder">
          <p>Gráficos de desempenho e relatórios detalhados aparecerão aqui.</p>
          <small></small>
        </div>
      </div>
    </div>
  )
}