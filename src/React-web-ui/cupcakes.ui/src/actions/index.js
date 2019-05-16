import * as Actions from './types';
import axios from "axios";

export const submitUserOrderRequest = ({ Id, Flavour,Size, Quantity }) => {
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
      type: Actions.Submit_User_Order,
      payload: request
    };
  };

  export const submitBakedOrder = (payload) =>{
    const headers = {
      'Content-Type': 'application/json'
    }  
    const request = axios.post(
        "http://localhost:5000/api/bake",
        payload,
        {headers: headers}
      ).catch(err =>{console.log("Error occured while submitting baked order:",err);throw err;});
    
      return {
        type: Actions.Baked_Order,
        payload: request
      };
    };

    export const cupcakeFlavourChange =(payload)=>{
      return {
        type: Actions.CupCake_FlavourChange,
        payload
      };
    };

    export const cupcakeSizeChange =(payload)=>{
      return {
        type: Actions.CupCake_SizeChange,
        payload
      };
    };

    export const submitMixedOrder = (payload) =>{
      const headers = {
        'Content-Type': 'application/json'
      }  
      const request = axios.post(
          "http://localhost:5000/api/mix",
          payload,
          {headers: headers}
        ).catch(err =>{console.log("Error occured while submitting mixed order:",err);throw err;});
      
        return {
          type: Actions.Mixed_Order,
          payload: request
        };
      }
      export const submitDecoratedOrder = (payload) =>{
        const headers = {
          'Content-Type': 'application/json'
        }  
        const request = axios.post(
            "http://localhost:5000/api/decorate",
            payload,
            {headers: headers}
          ).catch(err =>{console.log("Error occured while submitting decorated order:",err);throw err;});
        
          return {
            type: Actions.Decorated_Order,
            payload: request
          };
        }
        export const submitBoxedOrder = (payload) =>{
          const headers = {
            'Content-Type': 'application/json'
          }  
          const request = axios.post(
              "http://localhost:5000/api/box",
              payload,
              {headers: headers}
            ).catch(err =>{console.log("Error occured while submitting boxed order:",err);throw err;});
          
            return {
              type: Actions.Boxed_Order,
              payload: request
            };
          }