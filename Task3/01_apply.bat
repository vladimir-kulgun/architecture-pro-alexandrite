cls
kubectl apply -f https://github.com/cert-manager/cert-manager/releases/download/v1.13.3/cert-manager.yaml

kubectl apply -f k8s/namespaces.yaml
kubectl apply -f https://github.com/jaegertracing/jaeger-operator/releases/download/v1.51.0/jaeger-operator.yaml -n observability
kubectl apply -f k8s/jaeger-instance.yaml

# Сборка образов
minikube image build -t order-service:latest services/orderservice/
minikube image build -t calculation-service:latest services/calculationservice/

# Развертывание
kubectl apply -f k8s/services.yaml

kubectl port-forward svc/simplest-query 16686:16686