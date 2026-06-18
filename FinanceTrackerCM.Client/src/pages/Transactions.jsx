import React, { useEffect, useState } from 'react'
import api from '../services/api'
import '../styles/transactions.css'

export default function Transactions(){
  const [items, setItems] = useState([])
  
  // Estados para armazenar as opções que vêm do banco
  const [contas, setContas] = useState([])
  const [categorias, setCategorias] = useState([])

  // Estados dos campos do formulário
  const [contaId, setContaId] = useState('')
  const [categoriaId, setCategoriaId] = useState('')
  const [valor, setValor] = useState('')
  const [descricao, setDescricao] = useState('')

  // Carrega transações, contas e categorias ao montar o componente
  useEffect(() => {
    // Busca transações
    api.get('/api/transacoes').then(r => setItems(r.data)).catch(() => {})
    
    // Busca contas para o Select
    api.get('/api/contas').then(r => setContas(r.data)).catch(() => {})
    
    // Busca categorias para o Select
    api.get('/api/categorias').then(r => setCategorias(r.data)).catch(() => {})
  }, [])

  async function create(e){
    e.preventDefault()
    
    if (!contaId || !categoriaId) {
      alert('Por favor, selecione uma conta e uma categoria.')
      return
    }

    try {
      await api.post('/api/transacoes', { 
        contaId, 
        categoriaId, 
        valor: parseFloat(valor), 
        dataTransacao: new Date().toISOString(), 
        descricao 
      })
      
      const r = await api.get('/api/transacoes')
      setItems(r.data)
      setValor('')
      setDescricao('')
      setContaId('')
      setCategoriaId('')
    } catch(err) {
      alert('Erro ao registrar transação')
    }
  }

  async function remove(id){
    try {
      await api.delete('/api/transacoes/' + id)
      setItems(items.filter(i => i.id !== id))
    } catch(err) {
      alert('Erro ao excluir transação')
    }
  }

  // Função auxiliar para encontrar o nome da conta na listagem (melhora a visualização)
    const getNomeConta = (id) => {
      const conta = contas.find(c => (c.id || c.Id) === id)
      return conta ? (conta.nome || conta.Nome) : id
    }

    const getNomeCategoria = (id) => {
      const categoria = categorias.find(c => (c.id || c.Id) === id)
      return categoria ? (categoria.nomeCategoria || categoria.nomeCategoria) : id
    }

  return (
    <div className="page-container">
      <h2>Gerenciar Transações</h2>
      
      <form className="card form-grid" onSubmit={create}>
        <div className="input-grid">
          
          {/* Select de Contas */}
          <select value={contaId} onChange={e => setContaId(e.target.value)} required>
            <option value="">Selecione a Conta</option>
            {contas.map(c => (
              <option key={c.id} value={c.id}>
                {c.nome}
              </option>
            ))}
          </select>

          {/* Select de Categorias */}
          <select value={categoriaId} onChange={e => setCategoriaId(e.target.value)} required>
            <option value="">Selecione a Categoria</option>
            {categorias.map(c => (
              <option key={c.id || c.Id} value={c.id || c.Id}>
                {c.nomeCategoria} {/* <-- Alterado de c.nome para c.nomeCategoria */}
              </option>
            ))}
          </select>

          <input type="number" step="0.01" placeholder="Valor (R$)" value={valor} onChange={e => setValor(e.target.value)} required />
          <input placeholder="Descrição" value={descricao} onChange={e => setDescricao(e.target.value)} />
        </div>
        <button type="submit" className="btn-submit">Registrar Transação</button>
      </form>

      <div className="card">
        <ul className="transaction-list">
          {items.map(i => (
            <li key={i.id} className="transaction-item">
              <div className="transaction-info">
                <span className="transaction-desc">{i.descricao || 'Sem descrição'}</span>
                {}
                <span className="transaction-meta">
                  Conta: {getNomeConta(i.contaId)} | Categoria: {getNomeCategoria(i.categoriaId)}
                </span>
              </div>
              <div className="transaction-actions">
                <span className="transaction-amount">R$ {parseFloat(i.valor).toFixed(2)}</span>
                <button className="btn-delete" onClick={() => remove(i.id)}>Excluir</button>
              </div>
            </li>
          ))}
        </ul>
      </div>
    </div>
  )
}