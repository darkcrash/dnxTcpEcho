#!/bin/bash

# Docker images remove
docker rmi -f dnxtcpecho

# Docker Build
docker build -t dnxtcpecho .
