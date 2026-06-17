import React, { useState } from 'react'
import { useNavigate } from 'react-router-dom'
import api from '../services/api'
import { setTokens } from '../services/auth'

export default function Login(){
  const [email,setEmail] = useState('')
  const [password,setPassword] = useState('')
  const [tenantId,setTenantId] = useState('')
  const navigate = useNavigate()

  async function submit(e){
    e.preventDefault()
    try{
      const res = await api.post('/api/auth/login', { email, password, tenantId })
      setTokens(res.data.accessToken)
      navigate('/')
    }catch(err){
      alert('Falha no login')
    }
  }

  return (
    <div className="login">
      <h2>Login</h2>
      <form onSubmit={submit}>
        <input placeholder="Email" value={email} onChange={e=>setEmail(e.target.value)} />
        <input placeholder="Senha" type="password" value={password} onChange={e=>setPassword(e.target.value)} />
        <input placeholder="TenantId" value={tenantId} onChange={e=>setTenantId(e.target.value)} />
        <button>Entrar</button>
      </form>
    </div>
  )
}
