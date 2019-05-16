import React, { Component } from "react";
import { withStyles } from "@material-ui/core/styles";
import Card from "@material-ui/core/Card";
import CardActionArea from "@material-ui/core/CardActionArea";
import CardActions from "@material-ui/core/CardActions";
import CardContent from "@material-ui/core/CardContent";
import CardHeader from "@material-ui/core/CardHeader";
import CardMedia from "@material-ui/core/CardMedia";
import Button from "@material-ui/core/Button";
import Typography from "@material-ui/core/Typography";
import InputLabel from "@material-ui/core/InputLabel";
import MenuItem from "@material-ui/core/MenuItem";
import FormHelperText from "@material-ui/core/FormHelperText";
import FormControl from "@material-ui/core/FormControl";
import Select from "@material-ui/core/Select";
const styles = theme => ({
  root: {
    display: "flex",
    flexWrap: "wrap"
  },
  formControl: {
    margin: theme.spacing.unit,
    minWidth: 120
  },
  card: {
    width: 350,
    margin: "auto",
    textAlign: "center"
  },
  media: {
    height: 140
  }
});

const handleChange = event => {
  //this.setState({ [event.target.name]: event.target.value });
  //alert(event.target.value);
};

export class OrderRequest extends Component {
  constructor(props) {
    super(props);
  }

  handleSubmitClick(props) {
    
    console.log(props.Flavour);
    console.log(props.Size);
    console.log(props.SubmitOrderCallback);

    const samplePayload = {
      Id: 177,
      Flavour: props.Flavour,
      Quantity: 1,
      Size: props.Size
    };
    
    props.SubmitOrderCallback(samplePayload);
  }

  handleCupCakeFlavourChange(callback, payload) {
    callback(payload);
  }

  handleCupCakeSizeChange(callback, payload) {
    callback(payload);
  }

  render() {
    const { classes } = this.props;
    return (
      <Card className={classes.card}>
        <CardHeader title="Place New Order Here" />
        <CardContent>
          <Typography
            gutterBottom
            variant="h6"
            style={{ textAlign: "left" }}
            component="h2"
          >
            Chai Latte Cupcake
          </Typography>
          <form className={classes.root} autoComplete="off">
            <FormControl className={classes.formControl}>
              <InputLabel htmlFor="flavour">Choose Flavour</InputLabel>
              <Select
                onChange={event => {
                  this.handleCupCakeFlavourChange(
                    this.props.FlavourChange,
                    event.target.value
                  );
                }}
                // onChange={handleChange}
                value={this.props.Flavour}
                inputProps={{
                  name: "Flavour",
                  id: "flavour"
                }}
              >
                <MenuItem value="">
                  <em>None</em>
                </MenuItem>
                <MenuItem value={10}>Chocolate</MenuItem>
                <MenuItem value={20}>Vanilla</MenuItem>
                <MenuItem value={30}>Coffee</MenuItem>
              </Select>
              <br />
              <InputLabel htmlFor="size">Choose Size</InputLabel>
              <Select
                onChange={event => {
                  this.handleCupCakeSizeChange(
                    this.props.SizeChange,
                    event.target.value
                  );
                }}
                value={this.props.Size}
                inputProps={{
                  name: "Size",
                  id: "size"
                }}
              >
                <MenuItem value="">
                  <em>None</em>
                </MenuItem>
                <MenuItem value={10}>Small</MenuItem>
                <MenuItem value={20}>Medium</MenuItem>
                <MenuItem value={30}>Large</MenuItem>
              </Select>
            </FormControl>
          </form>
        </CardContent>
        <CardActions>
          <Button
            size="small"
            color="primary"
            onClick={event => {
              this.handleSubmitClick(this.props);
            }}
          >
            Order
          </Button>
        </CardActions>
      </Card>
    );
  }
}

export default withStyles(styles)(OrderRequest);
