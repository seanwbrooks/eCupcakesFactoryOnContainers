
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
import Button from '@material-ui/core/Button';
import CardMedia from '@material-ui/core/CardMedia';
import { Link } from 'react-router-dom'

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
        width: 400,
        height:310,
        textAlign: 'center'
    },
    media: {
        height: 160,
    },
});

class OrdersToBake extends Component {
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

        hubConnection.on('InformNewOrderToBake', (receivedMessage) => {
            console.log(receivedMessage);
            var newArray = this.state.messages.slice();
            newArray.push(receivedMessage);
            this.setState({ messages: newArray });
            console.log(this.state.messages[0]);
        });
        
    }
    
    render() {
        const { classes } = this.props;
         const ViewAll = props => <Link  to="/listorders" {...props} />
        //const ViewAll = props => <Link to={{ pathname: '/listorders', state: { list: this.state.messages} }} {...props} />
        return (
        
        <Card className={classes.card}>
            <CardHeader title="Bake Process " component="span" style={{backgroundColor:'lightblue',color:'white'}}>
            </CardHeader>
            <CardMedia
                className={classes.media}
                image="/bakeprocess.png"
                title="Bake"
            />
            <CardContent>
                { !this.state.messages ? "No New Orders" : (<div>New orders: {this.state.messages.length}
                </div>) }
                    {/* this.state.messages.map((orderrequest, index) => (
                        <Order request={orderrequest}/> 
                    )) : "No New Orders" } */}
            </CardContent>
            <CardActions>
                <Button size="small" color="primary" component={ViewAll}>
                    View All
                </Button>
            </CardActions>
        </Card>
        );
    }
}

export default withStyles(styles)(OrdersToBake);