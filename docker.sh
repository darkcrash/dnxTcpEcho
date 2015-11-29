#!/bin/bash

cd

# clean up clone
rm -Rf dnxTcpEcho

# git clone
git clone https://github.com/darkcrash/dnxTcpEcho.git
cd ~/dnxTcpEcho/

# Docker stop
#DOCKERCONTAINARID=`docker ps -f image=dnxtcpecho --no-trunc=false -q`
#docker stop $DOCKERCONTAINARID

# Docker images remove
docker rmi -f dnxtcpecho

# Docker Build
docker build -t dnxtcpecho .
