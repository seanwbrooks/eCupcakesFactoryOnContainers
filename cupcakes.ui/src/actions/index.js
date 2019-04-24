import {Submit_User_Order} from './types';
import axios from "axios";

export function submitUserOrderRequest({ Id, Flavour,Size, Quantity }) {
  const headers = {
    'Content-Type': 'application/json'
}  
  const request = axios.post(
      "http://localhost:5000/api/order",
      {
        Id,
        Flavour,
        Size,
        Quantity
      },
      {headers: headers}
    ).catch(err =>{console.log("Error occured during POST request:",err);throw err;});
  
    return {
      type: Submit_User_Order,
      payload: request
    };
  }
