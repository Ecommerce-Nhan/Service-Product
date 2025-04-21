# Product Service README

## ⚠️ Important Notes
- A **GitHub Token** is required to access shared library packages from GitHub Package.
- AWS **Access Token** must be configured properly.

---

## Overview

The **Product Service** is a .NET-based microservice designed to manage product-related data and operations. It is built using .NET Core, follows clean architecture principles, and leverages AWS for cloud infrastructure. The service supports image upload, resizing, and storage functionalities and is containerized for deployment in an **ECS Cluster** using CI/CD pipelines.

[Overall Microservice System Design](https://app.eraser.io/workspace/ZuxJp0sPbJ4zaCz4Pmvn?origin=share)

=======

## Architecture

### Components
1. **AWS CloudWatch** - Monitors the service and logs application events.
2. **AWS CloudFront** - Serves images from the S3 bucket as a CDN for faster delivery.
3. **AWS Lambda** - Triggers and resizes images uploaded to the S3 bucket.
4. **AWS S3** - Stores images uploaded by the Product Service.
5. **Hangfire** - Manages background jobs like uploading images to S3.
6. **PostgreSQL** - Stores product metadata.
7. **ECS Cluster** - Hosts the containerized Product Service.
8. **GitHub Actions** - Automates CI/CD pipeline for build, test, and deployment.

### Workflow
1. Product metadata is stored in **PostgreSQL**.
2. Image files are uploaded to **AWS S3** via the Product Service.
3. **AWS Lambda** resizes images upon upload and updates the S3 bucket.
4. Images are served via **AWS CloudFront** for optimized delivery.
5. **Hangfire** schedules and executes image uploads and other background tasks.
6. Logs and metrics are monitored in **AWS CloudWatch**.

![Product Service Architecture](https://github.com/user-attachments/assets/00e3ed2b-5af8-4ae5-bb50-225bbf896bb6)

---

## Prerequisites

### Development Environment
- .NET Core SDK 9.0
- Docker
- AWS CLI configured with appropriate IAM permissions

### Cloud Resources
- AWS Account with access to **ECR, ECS, S3, Lambda, CloudWatch, and CloudFront**

### Other Tools
- PostgreSQL instance
- GitHub repository

---

## Local Development (Docker)

### Configuration
1. Edit `appsettings.Development.json` with:
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Host=<your-database-host>;Database=<db-name>;User Id=<username>;Password=<password>",
       "ReadOnlyConnection": "Host=<your-database-host>;Database=<db-name>;User Id=<username>;Password=<password>"
     },
     "AWS": {
       "Profile": "<profile-name>",
       "Region": "<region-name>",
       "CloudFrontDistribution": "<distribution-id>"
     }
   }
   ```
2. Set environment variables for sensitive configurations:
   - AWS Access Key and Secret Key
   - Database credentials

### Running Locally
1. Clone the repository:
   ```bash
   git clone https://github.com/Ecommerce-Nhan/ProductService.git
   ```
2. Start Docker Desktop.
3. Run Docker Compose:
   ```bash
   docker-compose up --build -d
   ```
4. Use Postman or Swagger UI to test APIs.

---

## Deployment

### CI/CD Pipeline
The deployment process is automated using **GitHub Actions** for build, test, and deployment.

#### Deployment Steps:
1. Push code changes to the **main branch** to trigger CI/CD.
2. **GitHub Actions** runs the workflow defined in `.github/workflows/deploy.yml`.
3. The pipeline builds and tests the Docker image.
4. The image is pushed to **AWS ECR**.
5. ECS tasks are updated to deploy the latest image.

### AWS Deployment Workflow
#### **Docker Configuration**
- The service is containerized using Docker for a consistent runtime environment.
- The `Dockerfile` defines dependencies and configurations.

#### **ECR (Elastic Container Registry)**
- Stores Docker images with version control.
- Separate repositories for frontend and backend services.

#### **ECS Cluster**
- Runs Docker containers for both frontend and backend services.
- Task definitions specify CPU, memory, and container configurations.

#### **ALB (Application Load Balancer)**
- Routes traffic based on defined rules:
  - `/` → Frontend Service
  - `/api/*` → Backend Service
- Supports HTTPS (port 443) and HTTP (port 80).

#### **S3 and CloudFront**
- S3 stores static files and images.
- CloudFront serves files via CDN for fast, secure access.

#### **PostgreSQL**
- Managed database for product metadata.
- Connected securely to the ECS Cluster via a private VPC endpoint.

#### **CI/CD Pipeline with GitHub Actions**
- **Builds and tests** Docker images.
- **Pushes images** to ECR.
- **Deploys updates** to ECS services.

![Container Deployment](https://github.com/user-attachments/assets/92096c9e-acfc-4124-ae5c-9376092bf2c7)

---

## Authors
**© Developed by Tran Thanh Nhan.**
