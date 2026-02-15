cls
kubectl delete -f k8s/services.yaml
kubectl delete -f k8s/jaeger-instance.yaml
kubectl delete -f k8s/namespaces.yaml
kubectl delete -f https://github.com/cert-manager/cert-manager/releases/download/v1.13.3/cert-manager.yaml