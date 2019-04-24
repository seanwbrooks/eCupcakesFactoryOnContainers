import React from 'react';
import { Route, Switch } from 'react-router-dom';

import Landing from './pages/landing';
import Mixer from './pages/mixer';
import Bake from "./pages/bake";
import OrderDashboard from './components/orderdashboard';

const Routes = () => (
  <Switch>
    <Route exact path="/" component={Landing} />
    <Route path="/mixer" component={Mixer} />
    <Route path="/bake" component={Bake} />
  </Switch>
);

export default Routes;
