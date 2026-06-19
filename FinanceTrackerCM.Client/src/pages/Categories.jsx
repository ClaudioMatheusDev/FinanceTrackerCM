import React, { useEffect, useState } from 'react';
import api from '../services/api';
import '../styles/categories.css';

const tipos = {
  0: 'Receita',
  1: 'Despesa'
};

export default function Categories(){
  const [items,setItems] = useState([]);
  const [name,setName] = useState('');
  const [tipo,setTipo] = useState(1);

  useEffect(()=>{
    api.get('/api/categorias').then(r=>setItems(r.data)).catch(()=>{});
  },[]);

  async function create(e){
    e.preventDefault();
    try{
      await api.post('/api/categorias', { nomeCategoria: name, tipo });
      const r = await api.get('/api/categorias');
      setItems(r.data);
      setName('');
      setTipo(1);
    }catch(err){alert('Erro ao criar categoria');}
  }

  async function remove(id){
    try{
      await api.delete('/api/categorias/' + id);
      setItems(items.filter(i=>i.id!==id));
    }catch(err){alert('Erro ao excluir categoria');}
  }

  return (
    <div className="page-container">
      <h2>Gerenciar Categorias</h2>
      
      <form className="card form-group" onSubmit={create}>
        <input 
          placeholder="Nome da categoria (ex: Alimentacao, Lazer)" 
          value={name} 
          onChange={e=>setName(e.target.value)} 
          required
        />
        <select value={tipo} onChange={e => setTipo(Number(e.target.value))}>
          <option value={1}>Despesa</option>
          <option value={0}>Receita</option>
        </select>
        <button type="submit">Criar Categoria</button>
      </form>

      <div className="card">
        <ul className="category-list">
          {items.map(i => (
            <li key={i.id} className="category-item">
              <span>{i.nomeCategoria} <small>({tipos[i.tipo] || 'Tipo desconhecido'})</small></span>
              <button className="btn-delete" onClick={()=>remove(i.id)}>Excluir</button>
            </li>
          ))}
        </ul>
      </div>
    </div>
  );
}
