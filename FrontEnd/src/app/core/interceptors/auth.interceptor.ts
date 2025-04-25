import { HttpInterceptorFn } from '@angular/common/http';
 
import { inject } from '@angular/core';
 

export const TokenInterceptor: HttpInterceptorFn = (req, next) => {
  const token = localStorage.getItem('token'); // Use getCookie directly
  
  
   
  // Check if token is expired
  // if (token) {
  //   const expiryTime = tokenHandler.getExpiryTime();
  //   if (expiryTime && Date.now() >= expiryTime * 1000) {
  //     // Token expired, trigger logout
  //     localStorage.removeItem('token');
  //     window.location.reload();
  //     return next(req);
  //   }
  // }

  const authReq = req.clone({
    setHeaders: {
      Authorization: `Bearer  ${token ? token : ''}`,
      'Access-Control-Allow-Origin': '*',
     
      'Accept-Language' :  'en',
    },
  });
  return next(authReq);
};