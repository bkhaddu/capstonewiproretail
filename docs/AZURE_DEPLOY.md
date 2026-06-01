# Azure Deployment (Docker + App Service)

This project can be deployed as a Linux container to **Azure Web App for Containers**.

## 1) Create Azure resources (one-time)

```bash
# Variables
RG=rg-retail-prod
LOC=centralindia
PLAN=asp-retail-prod
WEBAPP=retailopt-webapp-prod
ACR=retailoptacr

az group create -n $RG -l $LOC
az acr create -n $ACR -g $RG --sku Basic
az appservice plan create -n $PLAN -g $RG --is-linux --sku B1
az webapp create -n $WEBAPP -g $RG -p $PLAN --deployment-container-image-name mcr.microsoft.com/azuredocs/aci-helloworld
```

## 2) Configure GitHub secrets

In GitHub repository Settings -> Secrets and variables -> Actions, add:

- `AZURE_CREDENTIALS` (service principal JSON from `az ad sp create-for-rbac`)
- `ACR_NAME` (example: `retailoptacr`)
- `AZURE_RESOURCE_GROUP` (example: `rg-retail-prod`)
- `AZURE_WEBAPP_NAME` (example: `retailopt-webapp-prod`)

Create SP JSON:

```bash
SUBSCRIPTION_ID=$(az account show --query id -o tsv)
az ad sp create-for-rbac \
  --name github-retail-deploy-sp \
  --role contributor \
  --scopes /subscriptions/$SUBSCRIPTION_ID/resourceGroups/$RG \
  --sdk-auth
```

Copy the JSON output into `AZURE_CREDENTIALS`.

## 3) Grant Web App permission to pull from ACR

```bash
PRINCIPAL_ID=$(az webapp identity assign -g $RG -n $WEBAPP --query principalId -o tsv)
ACR_ID=$(az acr show -n $ACR -g $RG --query id -o tsv)
az role assignment create --assignee $PRINCIPAL_ID --scope $ACR_ID --role AcrPull
```

## 4) Configure runtime app settings

Set your production connection string and JWT values on the Web App:

```bash
az webapp config appsettings set -g $RG -n $WEBAPP --settings \
  WEBSITES_PORT=8080 \
  ConnectionStrings__DefaultConnection="<azure-sql-connection-string>" \
  Jwt__Key="<strong-secret-key>" \
  Jwt__Issuer="RetailOptimizationPlatform" \
  Jwt__Audience="RetailOptimizationUsers" \
  Jwt__ExpiryMinutes="60" \
  ASPNETCORE_ENVIRONMENT="Production"
```

## 5) Deploy

Push to `main` or run GitHub Action manually:

- Workflow: `.github/workflows/deploy-azure-webapp.yml`
- It builds Docker image, pushes to ACR, and updates Web App container image.

## 6) Verify

```bash
az webapp browse -g $RG -n $WEBAPP
az webapp log tail -g $RG -n $WEBAPP
```

## Notes

- The local `appsettings.json` currently points to `Server=sqlserver`; this will not work in Azure unless replaced by Azure SQL/hosted SQL connection string.
- Keep secrets only in Azure Web App settings and GitHub Secrets.
