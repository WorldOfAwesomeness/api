#!/bin/bash

if [ -d "$HOME/.dotnet" ] 
then
    echo ".Net 6.0 Already installed."
else
    curl -sSL https://dot.net/v1/dotnet-install.sh | bash /dev/stdin --channel 6.0.1xx --quality preview --install-dir ~/.dotnet
fi

if [ $( docker ps -a | grep sqlserver | wc -l ) -gt 0 ]; then
  echo "MSSql Already installed." 
else
  pushd
  cd container
  docker-compose up
  popd
fi
