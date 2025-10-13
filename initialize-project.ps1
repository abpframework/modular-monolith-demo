# Updates database
dotnet run --project app/Shopularity.Admin --migrate-database

# Install client-side libraries
abp install-libs

# Generates a signing certificate
dotnet dev-certs https -v -ep openiddict.pfx -p c2aeb07f-c57f-4df6-9819-daacd6811c9a

exit $LASTEXITCODE