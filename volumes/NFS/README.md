# How to set up NFS for sharing files in k8s cluster

## PRIMARY NODE RUNNING THE NSF SERVER

### Install NFS server on a node or dedicated machine:

sudo apt-get update
sudo apt install nfs-kernel-server

### Create and configure an export directory:

sudo mkdir -p /mnt/nfs_share
sudo chown nobody:nogroup /mnt/nfs_share
sudo chmod 777 /mnt/nfs_share

### Edit /etc/exports to make the directory available:

/mnt/nfs_share *(rw,sync,no_subtree_check)


### Apply the changes and start NFS server:

sudo exportfs -a
sudo systemctl restart nfs-kernel-server


## CONNECTED NODES 

### Install NFS client utilities on connected nodes

sudo apt-get update
sudo apt-get install nfs-common

### Ensure that the NFS server (<IP TO SERVER NODE>) is reachable from the other nodes:

ping <IP TO SERVER NODE>

### Verify that the NFS share is exported correctly on the server: On the NFS server (<IP TO SERVER NODE>), check the /etc/exports file and ensure it includes:

/mnt/nfs_share *(rw,sync,no_subtree_check)

### Restart the NFS server service after making any changes:

sudo systemctl restart nfs-kernel-server

### On other nodes, try manually mounting the NFS share to test:

sudo mount -t nfs <IP TO SERVER NODE>:/mnt/nfs_share /mnt/nfs_share
