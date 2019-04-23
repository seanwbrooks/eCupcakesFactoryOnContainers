
import React from 'react';
import { connect } from "react-redux";
import DisplayOrders from "../components/order";
import { submitUserOrderRequest } from "../actions";

 export const Landing = (props) => {
    return (
        <div>
            <h1> Landing Page-{props.recievedorder} </h1>
            <DisplayOrders list={props.submittedorders} />
        </div>
    );
};

const mapStateToProps = state => ({submittedorders:state.order.submittedorders,recievedorder:state.order.recievedorder});

const mapDispatchToProps = dispatch => ({
    submitOrder: payload => dispatch(submitUserOrderRequest(payload))
});

export default connect(
    mapStateToProps,
    mapDispatchToProps
)(Landing);