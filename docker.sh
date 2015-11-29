#!/bin/bash

cd

# clean up clone
rm -Rf dnxTcpEcho

# git clone
git clone https://github.com/darkcrash/dnxTcpEcho.git
cd ~/dnxTcpEcho/src/

# Docker stop
DOCKERCONTAINARID=`docker ps -f image=dnxTcpEcho --no-trunc=false -q`
docker stop $DOCKERCONTAINARID

# Docker images remove
docker rmi -f dnxTcpEcho

# Docker Build
docker build -t dnxTcpEcho .

# Docker Run
docker run -d -t -p 5999:5999 dnxTcpEcho