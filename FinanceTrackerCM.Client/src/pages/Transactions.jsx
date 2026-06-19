import React, { useEffect, useState } from 'react';
import api from '../services/api';
import '../styles/transactions.css';

const tipoLabel = {
  0: 'Receita',
  1: 'Despesa'
};

function todayInputValue() {
  return new Date().toISOString().slice(0, 10);
}

export default function Transactions(){
  const [items, setItems] = useState([]);
  const [contas, setContas] = useState([]);
  const [categorias, setCategorias] = useState([]);
  const [contaId, setContaId] = useState('');
  const [categoriaId, setCategoriaId] = useState('');
  const [valor, setValor] = useState('');
  const [dataTransacao, setDataTransacao] = useState(todayInputValue());
  const [descricao, setDescricao] = useState('');

  useEffect(() => {
    loadData();
  }, []);

  async function loadData() {
    const [transacoesRes, contasRes, categoriasRes] = await Promise.all([
      api.get('/api/transacoes'),
      api.get('/api/contas'),
      api.get('/api/categorias')
    ]);

    setItems(transacoesRes.data);
    setContas(contasRes.data);
    setCategorias(categoriasRes.data);
  }

  async function create(e){
    e.preventDefault();
    
    if (!contaId || !categoriaId) {
      alert('Selecione uma conta e uma categoria.');
      return;
    }

    try {
      await api.post('/api/transacoes', { 
        contaId, 
        categoriaId, 
        valor: parseFloat(valor), 
        dataTransacao: new Date(`${dataTransacao}T12:00:00`).toISOString(), 
        descricao 
      });
      
      await loadData();
      setValor('');
      setDescricao('');
      setContaId('');
      setCategoriaId('');
      setDataTransacao(todayInputValue());
    } catch(err) {
      alert(err.response?.data?.message || 'Erro ao registrar transacao');
    }
  }

  async function remove(id){
    try {
      await api.delete('/api/transacoes/' + id);
      await loadData();
    } catch(err) {
      alert(err.response?.data?.message || 'Erro ao excluir transacao');
    }
  }

  const getNomeConta = (id) => {
    const conta = contas.find(c => c.id === id);
    return conta ? conta.nome : id;
  };

  const getCategoria = (id) => categorias.find(c => c.id === id);

  return (
    <div className="page-container">
      <h2>Gerenciar Transacoes</h2>
      
      <form className="card form-grid" onSubmit={create}>
        <div className="input-grid">
          <select value={contaId} onChange={e => setContaId(e.target.value)} required>
            <option value="">Selecione a Conta</option>
            {contas.map(c => (
              <option key={c.id} value={c.id}>
                {c.nome}
              </option>
            ))}
          </select>

          <select value={categoriaId} onChange={e => setCategoriaId(e.target.value)} required>
            <option value="">Selecione a Categoria</option>
            {categorias.map(c => (
              <option key={c.id} value={c.id}>
                {c.nomeCategoria} ({tipoLabel[c.tipo]})
              </option>
            ))}
          </select>

          <input type="number" step="0.01" placeholder="Valor (R$)" value={valor} onChange={e => setValor(e.target.value)} required />
          <input type="date" value={dataTransacao} onChange={e => setDataTransacao(e.target.value)} required />
          <input placeholder="Descricao" value={descricao} onChange={e => setDescricao(e.target.value)} required />
        </div>
        <button type="submit" className="btn-submit">Registrar Transacao</button>
      </form>

      <div className="card">
        <ul className="transaction-list">
          {items.map(i => {
            const categoria = getCategoria(i.categoriaId);
            const isReceita = i.tipo === 0;

            return (
              <li key={i.id} className="transaction-item">
                <div className="transaction-info">
                  <span className="transaction-desc">{i.descricao || 'Sem descricao'}</span>
                  <span className="transaction-meta">
                    {new Date(i.dataTransacao).toLocaleDateString('pt-BR')} | Conta: {getNomeConta(i.contaId)} | Categoria: {categoria?.nomeCategoria || i.categoriaId}
                  </span>
                </div>
                <div className="transaction-actions">
                  <span className={isReceita ? 'transaction-amount income' : 'transaction-amount expense'}>
                    {isReceita ? '+' : '-'} R$ {parseFloat(i.valor).toFixed(2)}
                  </span>
                  <button className="btn-delete" onClick={() => remove(i.id)}>Excluir</button>
                </div>
              </li>
            );
          })}
        </ul>
      </div>
    </div>
  );
}
