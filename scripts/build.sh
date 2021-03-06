#!/bin/bash
source ../deploy-envs.sh

#AWS_ACCOUNT_NUMBER={} set in private variable
export AWS_ECS_REPO_DOMAIN=$AWS_ACCOUNT_NUMBER.dkr.ecr.$AWS_DEFAULT_REGION.amazonaws.com

# Build process

docker build -f ./src/Services/Agent/Agent.API/Dockerfile -t agent-api ./src/src/Services/Agent/Agent.API --no-cache
docker tag agent-api $AWS_ECS_REPO_DOMAIN/fibonapi

