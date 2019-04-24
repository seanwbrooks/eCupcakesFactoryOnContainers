
import React from 'react';
import { connect } from "react-redux";
import DisplayOrders from "../components/listorders";
import { submitUserOrderRequest } from "../actions";
import OrderDashboard from "../components/orderdashboard";
import OrderRequest from "../components/orderrequest";

 export const Landing = (props) => {
    return (
        <div>
            <h1> Orders </h1>
            {/* <DisplayOrders list={props.submittedorders} /> */}
            {/* <OrderDashboard /> */}
            {/* <OrderRequest submitOrder={props.submitOrder} /> */}
            <OrderDashboard submitOrder={props.submitOrder}  />
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