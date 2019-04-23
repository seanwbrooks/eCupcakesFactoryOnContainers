import {Submit_User_Order} from './types';
import axios from "axios";

export function submitUserOrderRequest({ Id, Flavour,Size, Quantity }) {
    const request = axios.post(
      "http://localhost:5000/api/order",
      {
        Id,
        Flavour,
        Size,
        Quantity
      }
    );
  
    return {
      type: Submit_User_Order,
      payload: request
    };
  }
