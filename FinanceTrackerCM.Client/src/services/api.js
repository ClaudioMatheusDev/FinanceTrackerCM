import axios from 'axios'
import { getAccessToken, setTokens, clearTokens } from './auth'

const api = axios.create({ baseURL: import.meta.env.VITE_API_URL || 'http://localhost:5062', withCredentials: true })

let isRefreshing = false
let failedQueue = []

const processQueue = (error, token=null) => {
  failedQueue.forEach(p => {
    if(error) p.reject(error)
    else p.resolve(token)
  })
  failedQueue = []
}

api.interceptors.request.use(config => {
  const token = getAccessToken()
  if(token) config.headers.Authorization = `Bearer ${token}`
  return config
})

api.interceptors.response.use(response => response, async error => {
  const originalRequest = error.config
  if(error.response?.status === 401 && !originalRequest._retry){
    if(isRefreshing){
      return new Promise(function(resolve, reject){
        failedQueue.push({resolve, reject})
      }).then(token => {
        originalRequest.headers.Authorization = 'Bearer ' + token
        return api(originalRequest)
      })
    }

    originalRequest._retry = true
    isRefreshing = true
    try{
      // call refresh endpoint; refresh token is sent via HttpOnly cookie
      const res = await api.post('/api/auth/refresh')
      setTokens(res.data.accessToken)
      processQueue(null, res.data.accessToken)
      originalRequest.headers.Authorization = 'Bearer ' + res.data.accessToken
      return api(originalRequest)
    }catch(err){
      processQueue(err, null)
      clearTokens()
      window.location.href = '/login'
      return Promise.reject(err)
    }finally{
      isRefreshing = false
    }
  }
  return Promise.reject(error)
})

export default api
