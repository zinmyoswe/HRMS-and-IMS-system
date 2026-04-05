

SELECT * FROM dbo.Staff;

SELECT * FROM dbo.Departments;

INSERT INTO dbo.Departments (DepartmentName)
VALUES ('Management'),('Admin'),('IT'),('HR'),('Production'),('Supply Chain'),('Customer Support'),('Quality'),
       ('Engineering');

INSERT INTO dbo.Departments (DepartmentName)
VALUES ('Finance'),('New Product Introduce'),('Warehouse');

SELECT * FROM Positions;

INSERT INTO Positions
VALUES ('CEO'),('CF0'),('CTO'),('Admin Manager'),
       ('IT Manager'),('Supply Chain Manager'),('Admin Manager'),('HR Manager'),('Warehouse Manager'),('Production Manger'),('Lean Manager'),('Plant Manager'),
       ('Software Engineer'),('Senior IT Engineer'),('IT Engineer'),('Quality Engineer'),('Senior Quality Engineer'),('Logistic Specialist'),
       ('Production Plan Engineer'), ('ME technician'), 
       ('IT Supervisor'),('Supply Chain Supervisor'),('Admin Staff'),('HR Staff'),('Warehouse Supervisor'),('Production Supervisor'),('Lean Supervisor'),('Purchase Materaila Control'),
       ('Quality Supervisor');