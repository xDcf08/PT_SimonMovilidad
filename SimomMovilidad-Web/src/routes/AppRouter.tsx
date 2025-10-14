import { BrowserRouter, Navigate, Route, Routes } from 'react-router-dom'
import { PrivateRoutes } from './PrivateRoutes'
import { PublicRoutes } from './PublicRoutes'
import { DashboardPage, LoginPage } from '../pages'

const AppRouter = () => {

  return (
    <BrowserRouter>
      <Routes>
        <Route element={<PublicRoutes/>}>
          <Route path='/login' element={<LoginPage />}/>
        </Route>

        <Route element={<PrivateRoutes />}>
          <Route path='/dashboard' element={<DashboardPage />}/>
        </Route> 

        <Route path='*' element={<Navigate to='/login' replace/>}/>
      </Routes>
    </BrowserRouter>
  )
}

export default AppRouter