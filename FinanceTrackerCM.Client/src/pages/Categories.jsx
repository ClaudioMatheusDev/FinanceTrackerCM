import React, { useEffect, useState } from 'react'
import api from '../services/api'
import '../styles/categories.css'

export default function Categories(){
  const [items,setItems] = useState([])
  const [name,setName] = useState('')

  useEffect(()=>{
    api.get('/api/categorias').then(r=>setItems(r.data)).catch(()=>{})
  },[])

  async function create(e){
    e.preventDefault()
    try{
      await api.post('/api/categorias', { nomeCategoria: name, tipo: 0 })
      const r = await api.get('/api/categorias')
      setItems(r.data)
      setName('')
    }catch(err){alert('erro')}
  }

  async function remove(id){
    try{
      await api.delete('/api/categorias/' + id)
      setItems(items.filter(i=>i.id!==id))
    }catch(err){alert('erro')}
  }

  return (
    <div className="page-container">
      <h2>Gerenciar Categorias</h2>
      
      <form className="card form-group" onSubmit={create}>
        <input 
          placeholder="Nome da categoria (ex: Alimentação, Lazer)" 
          value={name} 
          onChange={e=>setName(e.target.value)} 
        />
        <button type="submit">Criar Categoria</button>
      </form>

      <div className="card">
        <ul className="category-list">
          {items.map(i => (
            <li key={i.id} className="category-item">
              <span>{i.nomeCategoria}</span>
              <button className="btn-delete" onClick={()=>remove(i.id)}>Excluir</button>
            </li>
          ))}
        </ul>
      </div>
    </div>
  )
}