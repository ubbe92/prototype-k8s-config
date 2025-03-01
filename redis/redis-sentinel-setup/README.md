# How to set up a high availability redis cluster with sentinel for kubernetes

https://www.youtube.com/watch?v=JmCn7k0PlV4&t=4s
https://github.com/marcel-dempers/docker-development-youtube-series/tree/master/storage/redis/kubernetes


## Set up redis with replication

```bash
cd prototype-k8s-config/redis/redis-sentinel-setup/redis
sudo kubectl create ns redis
sudo kubectl apply -n redis -f redis-configmap.yaml
sudo kubectl apply -n redis -f redis-statefulset.yaml
```

check if pods and peristent volumes are created and check redis replica logs

```bash
sudo kubectl -n redis get pods
sudo kubectl -n redis get pv

sudo kubectl -n redis logs redis-0
sudo kubectl -n redis logs redis-1
sudo kubectl -n redis logs redis-2
```

check replication status

```bash
sudo kubectl -n redis exec -it redis-0 -- sh
redis-cli
info replication
```
## Set up sentinel for service discovery 

Deploy redis sentinel with three instances

```bash
cd prototype-k8s-config/redis/redis-sentinel-setup/sentinel
sudo kubectl apply -n redis -f sentinel-statefulset.yaml

sudo kubectl -n redis get pods
sudo kubectl -n redis get pv
sudo kubectl -n redis logs sentinel-0
```


