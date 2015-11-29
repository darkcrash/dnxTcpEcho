FROM microsoft/dotnet:0.0.1-alpha 
 
RUN mkdir -p /dnxTcpEcho 
WORKDIR /dnxTcpEcho
 
ENTRYPOINT ["dotnet", "run"] 
 
ONBUILD COPY . /dnxTcpEcho 
ONBUILD RUN dotnet restore 
