
import React from 'react';
import { connect } from "react-redux";
import { submitUserOrderRequest,submitMixedOrder,submitBakedOrder,submitDecoratedOrder,submitBoxedOrder } from "../actions";
import OrdersToMixer from "../components/dashboard/mixerprocess";
import OrdersToBake from "../components/dashboard/bakeprocess";
import OrdersToDecorate from "../components/dashboard/decorateprocess";
import OrdersToBox from "../components/dashboard/packageprocess";
import OrderRequest from "../components/orderrequest";
import Grid from '@material-ui/core/Grid';
// import IconLabelTabs from '../components/IconLabelTabs';
 export const Landing = (props) => {
    return (
        // <div>
        //     <IconLabelTabs />
        // </div>
        <div>
            <Grid container spacing={24} style={{padding: 24}}>
                <Grid item xs={12} sm={6} lg={4} xl={3}>
                    <OrderRequest submitOrder={props.submitOrder} />
                </Grid>
                <Grid item xs={12} sm={6} lg={4} xl={3}>
                    <OrdersToMixer submitMixedOrder={props.submitMixedOrder} />
                </Grid>
                <Grid item xs={12} sm={6} lg={4} xl={3}>
                    <OrdersToBake submitBakedOrder={props.submitBakedOrder} />
                </Grid>
                <Grid item xs={12} sm={6} lg={4} xl={3}>
                    <OrdersToDecorate submitDecoratedOrder={props.submitDecoratedOrder}/>
                </Grid>
                <Grid item xs={12} sm={6} lg={4} xl={3}>
                    <OrdersToBox submitBoxedOrder={props.submitBoxedOrder} />
                </Grid>
            </Grid>
        </div>
    );
};

const mapStateToProps = state => ({submittedorders:state.order.submittedorders,recievedorders:state.order.recievedorders});

const mapDispatchToProps = dispatch => ({
    submitOrder: payload => dispatch(submitUserOrderRequest(payload)),
    submitMixedOrder : payload => dispatch(submitMixedOrder(payload)),
    submitBakedOrder : payload => dispatch(submitBakedOrder(payload)),
    submitDecoratedOrder : payload => dispatch(submitDecoratedOrder(payload)),
    submitBoxedOrder : payload => dispatch(submitBoxedOrder(payload))
});

export default connect(
    mapStateToProps,
    mapDispatchToProps
)(Landing);