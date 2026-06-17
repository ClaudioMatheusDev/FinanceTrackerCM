import React, { useState } from 'react'
import { useNavigate, Link } from 'react-router-dom' 
import api from '../services/api'
import { setTokens } from '../services/auth'
import '../styles/login.css'

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
    <div className="login-container">
      <div className="login-card">
        <h2>Seja Bem-vindo ao FinanceTrackerCM</h2>
        <p className="login-subtitle">Faça login para acessar sua conta</p>
        
        <form onSubmit={submit}>

          <div className="input-group">
            <input 
              type="email" 
              placeholder="E-mail" 
              value={email} 
              onChange={e=>setEmail(e.target.value)} 
              required 
            />
          </div>

          <div className="input-group">
            <input 
              type="password" 
              placeholder="Senha" 
              value={password} 
              onChange={e=>setPassword(e.target.value)} 
              required 
            />
          </div>
        <div className="auth-switch">
        <span>Não tem uma conta? </span>
        <Link to="/register">Cadastre-se</Link>
      </div>

          <button type="submit" className="btn-login">Entrar</button>
        </form>
      </div>
    </div>
  )
}