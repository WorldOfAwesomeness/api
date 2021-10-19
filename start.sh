DOTNET_ROOT=/usr/share/dotnet

curl -sSL https://dot.net/v1/dotnet-install.sh | bash /dev/stdin -c 6.0 --install-dir $DOTNET_ROOT

cd ./src/Woa.Api
dotnet publish --configuration Release -o ./published

/workspace/src/Woa.Api/published/Woa.Api &
