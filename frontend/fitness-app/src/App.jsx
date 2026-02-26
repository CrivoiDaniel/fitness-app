import { BrowserRouter as Router, Routes, Route, useLocation } from 'react-router-dom'
import Dashboard from './pages/admin/Dashboard'
import Navbar from './components/Navbar'

const App = () => {
  return (
    <Router>
      <Navbar/>
      <Routes>
        <Route  path="/" element={<Dashboard/>}/>
      </Routes>
    </Router>
  )
}

export default App
