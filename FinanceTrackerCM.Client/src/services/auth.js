const ACCESS_KEY = 'ft_access'

export function setTokens(access){
  localStorage.setItem(ACCESS_KEY, access)
}
export function getAccessToken(){
  return localStorage.getItem(ACCESS_KEY)
}
export function clearTokens(){
  localStorage.removeItem(ACCESS_KEY)
}
