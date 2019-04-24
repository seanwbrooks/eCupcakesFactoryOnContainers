
import React, { Component } from 'react';
import * as SignalR from "@aspnet/signalr";

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
            var newArray = this.state.messages.slice();
            newArray.push(receivedMessage);
            this.setState({ messages: newArray });
        });

        
    }

    render() {
        return (<div>New orders: {this.state.messages.length}</div>);
    }
}

export default OrdersToMixer;