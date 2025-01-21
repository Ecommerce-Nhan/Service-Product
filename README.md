# Product Service README

## Overview
The **Product Service** is a .NET-based microservice designed to manage product-related data and operations. It is built using .NET Core, follows clean architecture principles, and leverages AWS for cloud infrastructure. The service supports image upload, resizing, and storage functionalities and is containerized for deployment in an ECS Cluster using CI/CD pipelines.
Overall Microservice System Design: https://app.eraser.io/workspace/ZuxJp0sPbJ4zaCz4Pmvn?origin=share
=======
---

## Architecture

### Components:
1. **AWS CloudWatch**: Monitors the service and logs application events.
2. **AWS CloudFront**: Serves images from the S3 bucket as a CDN for faster delivery.
3. **AWS Lambda**: Triggers and resizes images uploaded to the S3 bucket.
4. **AWS S3**: Stores images uploaded by the Product Service.
5. **Hangfire**: Manages background jobs like uploading images to S3.
6. **PostgreSQL**: Used for storing product metadata.
7. **ECS Cluster**: Hosts the containerized Product Service.
8. **GitHub Actions**: CI/CD pipeline to build, test, and deploy the service.

### Workflow:
1. Product metadata is stored in **PostgreSQL**.
2. Image files are uploaded to **AWS S3** via the Product Service.
3. **AWS Lambda** resizes images upon upload and updates the S3 bucket.
4. Images are served via **AWS CloudFront** for optimized delivery.
5. **Hangfire** schedules and executes image uploads and other background tasks.
6. Logs and metrics are monitored in **AWS CloudWatch**.

![product-service](https://github.com/user-attachments/assets/00e3ed2b-5af8-4ae5-bb50-225bbf896bb6)

=======
---

## Prerequisites
- **Development Environment**:
  - .NET Core SDK 9.0
  - Docker
  - AWS CLI configured with appropriate IAM permissions
  
=======
 
- **Cloud Resources**:
  - AWS Account with access to ECR, ECS, S3, Lambda, CloudWatch, and CloudFront

- **Other Tools**:
  - PostgreSQL instance
  - GitHub repository

---

## Local Development (Docker)

### Configuration:
1. Edit `appsettings.Development.json` files with:
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Host=<your-database-host>;Database=<db-name>;User Id=<username>;Password=<password>"
       "ReadOnlyConnection": "Host=<your-database-host>;Database=<db-name>;User Id=<username>;Password=<password>",
     },
     "AWS": {
       "Profile": "<profile-name>",
       "Region": "<region-name>",
       "CloudFrontDistribution": "<distribution-id>"

     }
   }
   ```

2. Environment variables for sensitive configurations:
   - AWS Access Key and Secret Key
   - Database credentials

### Running Locally:
1. Clone the repository:
   ```bash
   git clone [<repo-url>](https://github.com/Ecommerce-Nhan/ProductService.git)
   ```
2. Start docker desktop
3. Run docker compose:
   ```bash
   docker-compose up --build -d
   ```
4. Use tools like Postman or Swagger UI to test APIs.

---

## Deployment

### CI/CD Pipeline:
The deployment process uses **GitHub Actions** to build, test, and deploy the application.

1. **CI/CD Workflow YAML** (stored in `.github/workflows/deploy.yml`):
2. Push code changes to the master branch to trigger CI/CD.

Deployment with Docker and AWS
Docker Configuration
The Product Service is containerized using Docker to ensure a consistent runtime environment.
Dockerfiles are used to build the application image, containing all dependencies and configurations required for deployment.
AWS Deployment Workflow
ECR (Elastic Container Registry):

Docker images are pushed to AWS Elastic Container Registry for storage and version control.
The repository for both front-end and back-end services is maintained in ECR.
ECS Cluster:

The ECS Cluster runs the Docker containers for both front-end and back-end services.
Each service has its own task definition specifying resource allocation (e.g., CPU, memory) and container configurations.
ALB (Application Load Balancer):

Routes traffic based on defined rules:
/ routes to the Front-end Service.
/api/* routes to the Back-end Service.
ALB supports HTTPS (port 443) and HTTP (port 80) for secure communication.
S3 and CloudFront:

S3 is used to store static files and images uploaded by the Product Service.
AWS CloudFront serves files from the S3 bucket, providing a Content Delivery Network (CDN) for fast and secure access.
PostgreSQL:

A managed database service stores product-related metadata, connected securely to the ECS Cluster via a private endpoint within the VPC.
CI/CD Pipeline:

Managed using GitHub Actions, which:
Builds and tests the Docker images.
Pushes images to ECR.
Triggers updates in the ECS services to deploy the latest images.

![container](https://github.com/user-attachments/assets/92096c9e-acfc-4124-ae5c-9376092bf2c7)

---

## Authors
© Developed by Tran Thanh Nhan.
