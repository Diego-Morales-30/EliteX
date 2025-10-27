CREATE DATABASE BD_Data_File;
GO

USE BD_Data_File;
GO

CREATE TABLE Empleados (
    IdEmpleado INT IDENTITY(1,1) PRIMARY KEY,
    Nombre VARCHAR(50) NOT NULL,
    Apellidos VARCHAR(100) NOT NULL,
    FechaNacimiento DATE NOT NULL,
    Edad INT CHECK (Edad >= 18),
    EstadoCivil VARCHAR(20),
    Email VARCHAR(100) UNIQUE,
    Telefono VARCHAR(15),
    DNI CHAR(8) UNIQUE NOT NULL
);
GO

CREATE TABLE Usuario (
    IdUsuario INT IDENTITY(1,1) PRIMARY KEY,
    Usuario VARCHAR(50) UNIQUE NOT NULL,
    Contrasena VARCHAR(255) NOT NULL,
    Cargo VARCHAR(50) NOT NULL,
    IdEmpleado INT UNIQUE,
    FOREIGN KEY (IdEmpleado) REFERENCES Empleados(IdEmpleado)
);
GO

CREATE TRIGGER TRG_AutoUsuario
ON Usuario
INSTEAD OF INSERT
AS
BEGIN
    INSERT INTO Usuario (Usuario, Contrasena, Cargo, IdEmpleado)
    SELECT 
        'DT' + e.DNI,   
        i.Contrasena,
        i.Cargo,
        i.IdEmpleado
    FROM inserted i
    INNER JOIN Empleados e ON i.IdEmpleado = e.IdEmpleado;
END;
GO


INSERT INTO Empleados (Nombre, Apellidos, FechaNacimiento, Edad, EstadoCivil, Email, Telefono, DNI)
VALUES 
('Juan', 'P�rez L�pez', '1990-05-12', 35, 'Soltero', 'juan.perez@example.com', '987654321', '12345678'),
('Mar�a', 'G�mez Ruiz', '1988-09-22', 37, 'Casada', 'maria.gomez@example.com', '912345678', '87654321'),
('Luis', 'Fern�ndez D�az', '1995-03-10', 30, 'Soltero', 'luis.fernandez@example.com', '934567890', '11223344');


INSERT INTO Usuario (Contrasena, Cargo, IdEmpleado)
VALUES 
('Pass@123', 'Administrador', 1),
('Clave#456', 'Trabajador', 2),
('Segura!789', 'Jefe', 3);


SELECT u.IdUsuario, u.Usuario, u.Cargo, e.Nombre, e.DNI
FROM Usuario u
INNER JOIN Empleados e ON u.IdEmpleado = e.IdEmpleado;

SELECT * FROM Usuario
SELECT * FROM Empleados