    
import { Submit_User_Order } from "../actions/types";

const initialState={
    submittedorders:[{"Id":123},{"Id":345},{"Id":789}],
    recievedorders:[]
};
const orderReducer = (state = initialState, action) => {
  switch (action.type) {
    case Submit_User_Order:
      return { ...state, submittedorders:[...state.submittedorders,action.payload.data] };
    default:
      return state;
  }
}

export default orderReducer;