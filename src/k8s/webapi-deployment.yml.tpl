apiVersion: extensions/v1beta1
kind: Deployment
metadata:
  labels:
    app: ecupfactory-api-deployment
  name: ecupfactory-api-deployment
spec:
  replicas: 1
  template:
    metadata:
      labels:
        app: ecupfactory-api-deployment
    spec:
      containers:
      - name: web
        image: docker pull gcr.io/optical-highway-240115/ecupcontainer-api:1.0.0
        ports:
        - containerPort: 5000
        livenessProbe:
          httpGet:
            path: /
            port: 80
        readinessProbe:
          httpGet:
            path: /
            port: 80
ecupcontainer