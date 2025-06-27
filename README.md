"# Plano-de-Refeicoes" 
"# PlanoRefeicoes" 
"# PlanoRefeicoes" 

instruções referentes ao Auth

Login 1
user: admin
password: admin123

LOGIN 2
user: nutri
password: nutri123

Ao receber token JWT Clicar em "Authorize" > Insira 'Bearer' + espaço + o token JWT. Exemplo: Bearer abc123


COMANDOS UTILIZADOS: 

CREATE DATABASE Refeicoes

CREATE TABLE Patients (
    ID BIGINT PRIMARY KEY IDENTITY(1,1),
    Name VARCHAR(100) NOT NULL,
    Gender VARCHAR(20),
    Height_cm DECIMAL(5,2),
    Weight_kg DECIMAL(5,2),
    Created_at DATETIME DEFAULT GETDATE(),
    Updated_at DATETIME DEFAULT GETDATE(),
    Deleted_at DATETIME
);

CREATE TABLE Foods (
    ID BIGINT PRIMARY KEY IDENTITY(1,1),
    Name VARCHAR(100) NOT NULL,
    Calories_per_100g DECIMAL(6,2) NOT NULL,
    Created_at DATETIME DEFAULT GETDATE(),
    Updated_at DATETIME DEFAULT GETDATE()
);

CREATE TABLE MealPlans (
    ID BIGINT PRIMARY KEY IDENTITY(1,1),
    ID_Patient BIGINT NOT NULL,
    Date DATE NOT NULL,
    TotalCalories DECIMAL(8,2), 
    CreatedAt DATETIME DEFAULT GETDATE(),
    UpdatedAt DATETIME DEFAULT GETDATE(),
    CONSTRAINT FK_MealPlans_ID_Patient FOREIGN KEY (ID_Patient) REFERENCES Patients(ID)
);

CREATE TABLE MealPlanFoods (
    ID BIGINT PRIMARY KEY IDENTITY(1,1),
    ID_MealPlan BIGINT NOT NULL,
    ID_Food BIGINT NOT NULL,
    PortionSizeG DECIMAL(6,2) NOT NULL,
    Calories DECIMAL(8,2), 
    CreatedAt DATETIME DEFAULT GETDATE(),
    UpdatedAt DATETIME DEFAULT GETDATE(),
    CONSTRAINT FK_MealPlanFoods_ID_MealPlan FOREIGN KEY (ID_MealPlan) REFERENCES MealPlans(ID),
    CONSTRAINT FK_MealPlanFoods_ID_Food FOREIGN KEY (ID_Food) REFERENCES Foods(ID)
);
