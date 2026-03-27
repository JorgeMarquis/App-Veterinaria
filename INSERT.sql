USE BDVeterinariaWeb;
GO

-- 1. Insertar Especies
INSERT INTO Especie (Nombre, Descripcion) VALUES 
('Canino', 'Perros domésticos de todas las razas'),
('Felino', 'Gatos domésticos y exóticos'),
('Ave', 'Aves de compañía como loros o canarios');
GO

-- 2. Insertar Razas (Asociadas a las especies anteriores)
INSERT INTO Raza (IdEspecie, Nombre, Dimension) VALUES 
(1, 'Golden Retriever', 'Grande'),
(1, 'Bulldog Francés', 'Pequeño'),
(2, 'Persa', 'Mediano'),
(2, 'Siamés', 'Mediano');
GO

-- 3. Crear Usuario Administrador (PasswordHash ficticio para pruebas)
-- Nota: En producción usa BCrypt para generar este Hash.
INSERT INTO Usuario (Email, PasswordHash, NombreCompleto, Rol, Especialidad) VALUES 
('admin@veterinaria.com', 'grupo3', 'Jaime Administrador', 'Admin', 'Sistemas');
GO

-- 4. Insertar Clientes
INSERT INTO Cliente (NombreCompleto, TipoIdentificacion, NumIdentificacion, Email, TelefonoPrincipal, Direccion, Ciudad) VALUES 
('Luis Orlando Sullca', 'DNI', '77889900', 'luis.sullca@email.com', '987654321', 'Av. Las Gardenias 123', 'Ate Vitarte'),
('Franshesko Landeo', 'DNI', '11223344', 'franshesko.landeo@email.com', '912345678', 'Calle Los Jazmines 456', 'Vitarte');
GO

-- 5. Insertar Mascotas de Prueba
-- Nota: El MicrochipId es un código técnico que encaja con tu estilo táctico.
INSERT INTO Mascota (IdCliente, Nombre, IdEspecie, IdRaza, FechaNacimiento, Sexo, MicrochipId, CreatedBy) VALUES 
(1, 'Ghost', 1, 1, '2023-01-15', 'M', 'TAC-9988-X1', 'Sistema_Seed'),
(1, 'Shadow', 2, 3, '2024-05-20', 'F', 'TAC-4433-Y2', 'Sistema_Seed'),
(2, 'Rex', 1, 2, '2022-11-10', 'M', 'TAC-1122-Z3', 'Sistema_Seed');
GO



select * from Cliente;

