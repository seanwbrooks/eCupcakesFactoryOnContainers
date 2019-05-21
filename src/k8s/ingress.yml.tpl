apiVersion: extensions/v1beta1
kind: Ingress
metadata:
  name: ecupcakes-ingress
  annotations:
    kubernetes.io/ingress.global-static-ip-name: ecupContainers-ingress-DEPLOYMENT_ENV
  labels:
    last_updated: "1"
spec:
  rules:
  - host: ecupcakes-DEPLOYMENT_ENV.tribalscale.com
    http:
      paths:
      - path: /*
        backend:
          serviceName: ecupfactory-api-service
          servicePort: 80
  
