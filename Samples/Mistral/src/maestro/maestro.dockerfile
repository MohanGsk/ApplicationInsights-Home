FROM microsoft/dotnet:1.1-sdk

WORKDIR /maestro

# Restore nuget packages
# Doing this separately from build allows docker to cache this step
COPY ./src/Maestro.csproj .
RUN dotnet restore

# Copy Maestro and build
COPY ./src/ .
RUN dotnet publish -c Release -o out

# Expose port and run
EXPOSE 5000
ENV ASPNETCORE_URLS http://*:5000
ENTRYPOINT ["dotnet", "out/Maestro.dll"]