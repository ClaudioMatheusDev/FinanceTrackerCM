import React, { useEffect, useState } from 'react'
import api from '../services/api'
import '../styles/accounts.css'

export default function Accounts(){
  const [accounts,setAccounts] = useState([])
  const [nome,setNome] = useState('')
  const [saldo,setSaldo] = useState('')

  useEffect(()=>{
    api.get('/api/contas').then(r=>setAccounts(r.data)).catch(()=>{})
  },[])

  async function create(e){
    e.preventDefault()
    try{
      await api.post('/api/contas', { nome: nome, saldoInicial: parseFloat(saldo) || 0 })
      const r = await api.get('/api/contas')
      setAccounts(r.data)
      setNome('')
      setSaldo('')
    }catch(err){alert('erro')}
  }

  async function remove(id){
    try{
      await api.delete('/api/contas/' + id)
      setAccounts(accounts.filter(a=>a.id!==id))
    }catch(err){alert('erro')}
  }

return (
    <div className="page-container">
      <h2>Gerenciar Contas</h2>
      
      <form className="card form-group" onSubmit={create}>
        <input placeholder="Nome da conta" value={nome} onChange={e=>setNome(e.target.value)} />
        <input type="number" placeholder="Saldo inicial" value={saldo} onChange={e=>setSaldo(e.target.value)} />
        <button type="submit">Criar Conta</button>
      </form>

      <div className="card">
        <ul className="account-list">
          {accounts.map(a => (
            <li key={a.id} className="account-item">
              <span><strong>{a.nome}</strong>: R$ {parseFloat(a.saldo).toFixed(2)}</span>
              <button className="btn-delete" onClick={() => remove(a.id)}>Excluir</button>
            </li>
          ))}
        </ul>
      </div>
    </div>
  );
}