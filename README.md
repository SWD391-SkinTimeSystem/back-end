# SkinTimeSystem Backend

## 📌 Introduction
SkinTimeSystem Backend is a skincare service management system that helps schedule appointments, manage specialists, customers, and service processes.  
- **Technologies used**: .NET 9.0.1, ASP.NET Core, Entity Framework Core.  
- **Goal**: To provide a robust platform for skincare service management.

## ⚙️ System Requirements
Before running the project, make sure you have installed:  
- **[.NET SDK 9.0.2](https://dotnet.microsoft.com/en-us/download)**  
- **[Visual Studio 2022](https://visualstudio.microsoft.com/thank-you-downloading-visual-studio/?sku=Community&channel=Release&version=VS2022&source=VSLandingPage&cid=2030&passive=false)**  
  - [Setup Tutorial](https://www.youtube.com/watch?v=w4ZOEyMuBtw)  
- **MySQL Database Management System**:  
  - [MySQL Community Server](https://dev.mysql.com/downloads/mysql/)  
  - [MySQL Workbench](https://dev.mysql.com/downloads/workbench/)  
  - [Setup Tutorial](https://www.youtube.com/watch?v=dq1L1Lrbg6s&t=397s)  

## 🚀 Setup Instructions
1. **Clone the repository**:  
   ```bash
   git clone https://github.com/SWD391-SkinTimeSystem/back-end.git
   ```
2. **Install dependencies**:  
   ```bash
   dotnet restore
   ```
3. **Build the project** to check for any potential bugs:  
   ```bash
   dotnet clean
   dotnet build
   ```
4. **Environment Configuration**:  
   - Edit the `appsettings.json` file in the `SkinTime` package:  
   ```json
    "DefaultConnection": "Server=127.0.0.1;Database=SkinTimeDb;User=sa;Password=12345;TrustServerCertificate=true"
   ```
   - Replace `User` and `Password` with your MySQL Workbench credentials.
5. **Initialize the database** (using Entity Framework):  
   ```bash
   add-migrations <MigrationName>
   update-database
   ```

## ▶️ Running the Project
- **Run via Visual Studio**: Click the **Run** button ▶️.  
  ![Run Button](https://github.com/user-attachments/assets/56125c39-c854-4c84-bb1b-58e5ca4573d3)  

- **Clean and build** the project before running:  
  ![Clean & Build](https://github.com/user-attachments/assets/658ea72e-3abe-4144-b0bc-34edbe1a330e)  

## 📜 API Documentation
- **Access Swagger**:  
  ```bash
  https://localhost:<port>/swagger
  Example: https://localhost:7095/swagger/index.html
  ```

**Note**: After starting the project, the Swagger interface should open automatically.
