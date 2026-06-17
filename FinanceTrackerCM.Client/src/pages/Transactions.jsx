import React, { useEffect, useState } from 'react'
import api from '../services/api'
import '../styles/transactions.css'

export default function Transactions(){
  const [items,setItems] = useState([])
  const [contaId,setContaId] = useState('')
  const [categoriaId,setCategoriaId] = useState('')
  const [valor,setValor] = useState('')
  const [descricao,setDescricao] = useState('')

  useEffect(()=>{
    api.get('/api/transacoes').then(r=>setItems(r.data)).catch(()=>{})
  },[])

  async function create(e){
    e.preventDefault()
    try{
      await api.post('/api/transacoes', { contaId, categoriaId, valor: parseFloat(valor), dataTransacao: new Date().toISOString(), descricao })
      const r = await api.get('/api/transacoes')
      setItems(r.data)
      setValor('')
      setDescricao('')
    }catch(err){alert('erro')}
  }

  async function remove(id){
    try{
      await api.delete('/api/transacoes/' + id)
      setItems(items.filter(i=>i.id!==id))
    }catch(err){alert('erro')}
  }

  return (
    <div className="page-container">
      <h2>Gerenciar Transações</h2>
      
      <form className="card form-grid" onSubmit={create}>
        <div className="input-grid">
          <input placeholder="ID da Conta" value={contaId} onChange={e=>setContaId(e.target.value)} />
          <input placeholder="ID da Categoria" value={categoriaId} onChange={e=>setCategoriaId(e.target.value)} />
          <input type="number" step="0.01" placeholder="Valor (R$)" value={valor} onChange={e=>setValor(e.target.value)} />
          <input placeholder="Descrição" value={descricao} onChange={e=>setDescricao(e.target.value)} />
        </div>
        <button type="submit" className="btn-submit">Registrar Transação</button>
      </form>

      <div className="card">
        <ul className="transaction-list">
          {items.map(i => (
            <li key={i.id} className="transaction-item">
              <div className="transaction-info">
                <span className="transaction-desc">{i.descricao || 'Sem descrição'}</span>
                <span className="transaction-meta">Conta: {i.contaId} | Categoria: {i.categoriaId}</span>
              </div>
              <div className="transaction-actions">
                <span className="transaction-amount">R$ {parseFloat(i.valor).toFixed(2)}</span>
                <button className="btn-delete" onClick={()=>remove(i.id)}>Excluir</button>
              </div>
            </li>
          ))}
        </ul>
      </div>
    </div>
  )
}