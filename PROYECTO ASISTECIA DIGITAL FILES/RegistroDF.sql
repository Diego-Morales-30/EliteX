CREATE DATABASE RegistroDF;
use RegistroDF;

-- =====================================
-- TABLA: USUARIO
-- =====================================
CREATE TABLE USUARIO (
    id_usuario INT IDENTITY(1,1) PRIMARY KEY,
    username VARCHAR(50) NOT NULL,
    password_hash VARCHAR(100) NOT NULL
);

-- =====================================
-- TABLA: EMPLEADOS
-- =====================================
CREATE TABLE EMPLEADOS (
    id_empleado INT IDENTITY(1,1) PRIMARY KEY,
    nombre VARCHAR(50) NOT NULL,
    apellido VARCHAR(50) NOT NULL,
    id_usuario INT,
    FOREIGN KEY (id_usuario) REFERENCES USUARIO(id_usuario)
);

-- =====================================
-- TABLA: TIPO_HORARIO
-- =====================================
CREATE TABLE TIPO_HORARIO (
    id_tipo_horario INT IDENTITY(1,1) PRIMARY KEY,
    descripcion VARCHAR(100) NOT NULL
);

-- =====================================
-- TABLA: CONTRATO
-- =====================================
CREATE TABLE CONTRATO (
    id_contrato INT IDENTITY(1,1) PRIMARY KEY,
    id_empleado INT NOT NULL,
    id_tipo_horario INT NOT NULL,
    sueldo DECIMAL(10,2) NOT NULL,
    FOREIGN KEY (id_empleado) REFERENCES EMPLEADOS(id_empleado),
    FOREIGN KEY (id_tipo_horario) REFERENCES TIPO_HORARIO(id_tipo_horario)
);


-- Usuarios
INSERT INTO USUARIO (username, password_hash) VALUES
('carlos', '1234'),
('maria', 'abcd');

-- Empleados
INSERT INTO EMPLEADOS (nombre, apellido, id_usuario) VALUES
('Carlos', 'Gómez', 1),
('María', 'López', 2);

-- Tipos de horario
INSERT INTO TIPO_HORARIO (descripcion) VALUES
('Tiempo completo'),
('Medio tiempo');

-- Contratos
INSERT INTO CONTRATO (id_empleado, id_tipo_horario, sueldo) VALUES
(1, 1, 2500.00),
(2, 2, 1200.00);
