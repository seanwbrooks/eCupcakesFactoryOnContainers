import React from 'react';
import {BrowserRouter as Router} from 'react-router-dom';
import Routes from './routes';
import './App.css';

const App = () => (
      <div className="App">
        <div>
          <h1>Cup cakes Factory</h1>
          <Router>
            <Routes />
          </Router> 
        </div>
      </div>
);

export default App;
