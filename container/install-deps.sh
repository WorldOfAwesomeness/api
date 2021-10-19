#!/bin/bash

if [ -f "/usr/local/bin/gh" ] 
then
    echo "github cli already installed."
else
    pushd
    cd ~
    
    VERSION=`curl  "https://api.github.com/repos/cli/cli/releases/latest" | grep '"tag_name"' | sed -E 's/.*"([^"]+)".*/\1/' | cut -c2-` 
    echo $VERSION
    
    wget https://github.com/cli/cli/releases/download/v${VERSION}/gh_${VERSION}_linux_amd64.tar.gz
    tar xvf gh_${VERSION}_linux_amd64.tar.gz
    sudo cp gh_${VERSION}_linux_amd64/bin/gh /usr/local/bin/
    sudo cp -r gh_${VERSION}_linux_amd64/share/man/man1/* /usr/share/man/man1/
    
    popd
fi

if [ -d "$HOME/.dotnet" ] 
then
    echo ".Net 6.0 Already installed."
else
  curl -sSL https://dot.net/v1/dotnet-install.sh | bash /dev/stdin -c 6.0
fi

if [ $( docker ps -a | grep sqlserver | wc -l ) -gt 0 ]; then
  echo "MSSql Already installed." 
else
  pushd
  cd container
  docker-compose up
  popd
fi
