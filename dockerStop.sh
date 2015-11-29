#!/bin/bash

# Docker stop
DOCKERCONTAINARID=`docker ps -f image=dnxtcpecho --no-trunc=false -q`
docker stop $DOCKERCONTAINARID
