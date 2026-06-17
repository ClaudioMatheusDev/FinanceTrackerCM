import React, { useState } from 'react'
import { useNavigate, Link } from 'react-router-dom'
import api from '../services/api'
import '../styles/login.css' // Reaproveita o container estrutural do login

export default function Register(){
  const [email, setEmail] = useState('')
  const [password, setPassword] = useState('')
  // TenantId removido do fluxo de registro — administradores atribuirão o tenant.
  const navigate = useNavigate()

  async function submit(e){
    e.preventDefault()
    try {
      await api.post('/api/auth/register', { email, password })
      alert('Cadastro realizado com sucesso!')
      navigate('/login')
    } catch(err) {
      alert('Falha ao registrar conta. Verifique os dados ou os requisitos da senha.')
    }
  }

  return (
    <div className="login-container">
      <div className="login-card">
        <h2>Criar Conta</h2>
        <p className="login-subtitle">Cadastre seus dados para começar</p>
        
        <form onSubmit={submit}>
          {/* TenantId removido do formulário de registro; admins atribuirão o tenant posteriormente */}

          <div className="input-group">
            <input 
              type="email" 
              placeholder="E-mail corporativo" 
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

          <button type="submit" className="btn-login btn-register">Registrar</button>
        </form>

        <div className="auth-switch">
          <span>Já tem uma conta? </span>
          <Link to="/login">Faça Login</Link>
        </div>
      </div>
    </div>
  )
}