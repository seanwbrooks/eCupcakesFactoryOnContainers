
import React, { Component } from 'react';
import * as SignalR from "@aspnet/signalr";
import Card from '@material-ui/core/Card';
import CardActions from '@material-ui/core/CardActions';
import CardContent from '@material-ui/core/CardContent';
import CardHeader from '@material-ui/core/CardHeader';
import { withStyles } from '@material-ui/core/styles';
import Typography from '@material-ui/core/Typography';
import InputLabel from '@material-ui/core/InputLabel';
import Order from './order';

const styles = (theme) =>({
    root: {
        display: 'flex',
        flexWrap: 'wrap',
      },
      formControl: {
        margin: theme.spacing.unit,
        minWidth: 120,
      },
    card: {
        width: 500,
        margin: 'auto',
        textAlign: 'center'
    },
    media: {
        height: 140,
    },
});

class OrdersToMixer extends Component {
    constructor(props) {
        super(props);

        this.state = {
            messages: [],
            hubConnection: null,
        };
    }

    componentDidMount = () => {
        const hubConnection = new SignalR.HubConnectionBuilder()
            .withUrl("http://localhost:5000/ordermonitorhub")
            .configureLogging(SignalR.LogLevel.Information)
            .build();
        this.setState({ hubConnection }, () => {
            hubConnection.start().then(function () {
                console.log("connected");
                
            }).catch(err => console.log('Error while establishing connection :('));
        });

        hubConnection.on('InformNewOrder', (receivedMessage) => {
            console.log(receivedMessage);
            var newArray = this.state.messages.slice();
            newArray.push(receivedMessage);
            this.setState({ messages: newArray });
            console.log(this.state.messages[0]);
        });
        
    }
    
    render() {
        const { classes } = this.props;
        return (
        
        <Card className={classes.card}>
            <CardHeader title="Mixer Process Here ">
            </CardHeader>
            <CardContent>
                    {this.state.messages.map((orderrequest, index) => (
                        <Order request={orderrequest}/> 
                    ))}
            </CardContent>
            <CardActions>
                <div>New orders: {this.state.messages.length}
                </div>
            </CardActions>
        </Card>
        );
    }
}

export default withStyles(styles)(OrdersToMixer);