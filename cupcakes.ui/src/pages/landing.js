
import React from 'react';
import { connect } from "react-redux";
import DisplayOrders from "../pages/listorders";
import { submitUserOrderRequest } from "../actions";
import OrdersToMixer from "../components/mixerprocess";
import OrdersToBake from "../components/bakeprocess";
import OrdersToDecorate from "../components/decorateprocess";
import OrdersToBox from "../components/packageprocess";
import OrderRequest from "../components/orderrequest";
import Grid from '@material-ui/core/Grid';

 export const Landing = (props) => {
    return (
        // <div>
        //     <h1> Orders </h1>
        //     {/* <DisplayOrders list={props.submittedorders} /> */}
        //     {/* <OrderDashboard /> */}
        //     {/* <OrderRequest submitOrder={props.submitOrder} /> */}
        //     <OrderDashboard submitOrder={props.submitOrder}  />
        // </div>
        <div>
            <Grid container spacing={24} style={{padding: 24}}>
                <Grid item xs={12} sm={6} lg={4} xl={3}>
                    <OrderRequest submitOrder={props.submitOrder} />
                </Grid>
                <Grid item xs={12} sm={6} lg={4} xl={3}>
                    <OrdersToMixer />
                </Grid>
                <Grid item xs={12} sm={6} lg={4} xl={3}>
                    <OrdersToBake />
                </Grid>
                <Grid item xs={12} sm={6} lg={4} xl={3}>
                    <OrdersToDecorate />
                </Grid>
                <Grid item xs={12} sm={6} lg={4} xl={3}>
                    <OrdersToBox />
                </Grid>
            </Grid>
        </div>
    );
};

const mapStateToProps = state => ({submittedorders:state.order.submittedorders,recievedorders:state.order.recievedorders});

const mapDispatchToProps = dispatch => ({
    submitOrder: payload => dispatch(submitUserOrderRequest(payload))
});

export default connect(
    mapStateToProps,
    mapDispatchToProps
)(Landing);