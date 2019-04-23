import React from 'react';
import {Provider} from 'react-redux';
import {BrowserRouter as Router} from 'react-router-dom';
import Routes from './routes';
import './App.css';
import store from './store'

const App = () => (
    <Provider store={store}>
      <div className="App">
        <div>
          <h1>Cup cakes Factory</h1>
          <Router>
            <Routes />
          </Router> 
        </div>
      </div>
      </Provider>
);

export default App;
