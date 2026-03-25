DROP DATABASE IF EXISTS BDVeterinariaWeb;
GO
CREATE DATABASE BDVeterinariaWeb;
GO
USE BDVeterinariaWeb;
GO

-- ============================================================================
-- 1. TABLAS CATÁLOGO
-- ============================================================================

CREATE TABLE Especie (
    IdEspecie        INT           IDENTITY(1,1) NOT NULL,
    Nombre           NVARCHAR(50)  NOT NULL,
    Descripcion      NVARCHAR(200) NULL,
    Activo           BIT           NOT NULL DEFAULT 1,
    FechaCreacion    DATETIME2     NOT NULL DEFAULT SYSDATETIME(),
    FechaModificacion DATETIME2    NULL,
    CONSTRAINT PK_Especie  PRIMARY KEY CLUSTERED (IdEspecie),
    CONSTRAINT UK_Especie_Nombre UNIQUE (Nombre),
    CONSTRAINT CHK_Especie_Nombre CHECK (LEN(TRIM(Nombre)) > 0)
);
GO

CREATE TABLE Raza (
    IdRaza           INT           IDENTITY(1,1) NOT NULL,
    IdEspecie        INT           NOT NULL,
    Nombre           NVARCHAR(100) NOT NULL,
    Dimension        NVARCHAR(20)  NULL CHECK (Dimension IN ('Pequeño','Mediano','Grande','Gigante')),
    Activo           BIT           NOT NULL DEFAULT 1,
    FechaCreacion    DATETIME2     NOT NULL DEFAULT SYSDATETIME(),
    FechaModificacion DATETIME2    NULL,
    CONSTRAINT PK_Raza         PRIMARY KEY CLUSTERED (IdRaza),
    CONSTRAINT FK_Raza_Especie FOREIGN KEY (IdEspecie) REFERENCES Especie(IdEspecie),
    CONSTRAINT UK_Raza_Nombre  UNIQUE (Nombre, IdEspecie),
    CONSTRAINT CHK_Raza_Nombre CHECK (LEN(TRIM(Nombre)) > 0)
);
GO

CREATE TABLE CategoriaProducto (
    IdCategoria      INT           IDENTITY(1,1) NOT NULL,
    Nombre           NVARCHAR(100) NOT NULL,
    Descripcion      NVARCHAR(300) NULL,
    EsMedicamento    BIT           NOT NULL DEFAULT 0,
    RequiereReceta   BIT           NOT NULL DEFAULT 0,
    Activo           BIT           NOT NULL DEFAULT 1,
    FechaCreacion    DATETIME2     NOT NULL DEFAULT SYSDATETIME(),
    FechaModificacion DATETIME2    NULL,
    CONSTRAINT PK_CategoriaProducto  PRIMARY KEY CLUSTERED (IdCategoria),
    CONSTRAINT UK_CategoriaProducto_Nombre UNIQUE (Nombre),
    CONSTRAINT CHK_Categoria_Nombre CHECK (LEN(TRIM(Nombre)) > 0)
);
GO

CREATE TABLE TipoServicio (
    IdTipoServicio         INT           IDENTITY(1,1) NOT NULL,
    Nombre                 NVARCHAR(100) NOT NULL,
    EsMedico               BIT           NOT NULL DEFAULT 1,
    DuracionEstimadaMin    INT           NULL CHECK (DuracionEstimadaMin > 0),
    Activo                 BIT           NOT NULL DEFAULT 1,
    FechaCreacion          DATETIME2     NOT NULL DEFAULT SYSDATETIME(),
    FechaModificacion      DATETIME2     NULL,
    CONSTRAINT PK_TipoServicio  PRIMARY KEY CLUSTERED (IdTipoServicio),
    CONSTRAINT UK_TipoServicio_Nombre UNIQUE (Nombre),
    CONSTRAINT CHK_TipoServicio_Nombre CHECK (LEN(TRIM(Nombre)) > 0)
);
GO

CREATE TABLE FormaPago (
    IdFormaPago          INT           IDENTITY(1,1) NOT NULL,
    Nombre               NVARCHAR(50)  NOT NULL,
    ComisionPorcentaje   DECIMAL(5,2)  NOT NULL DEFAULT 0,
    RequiereAutorizacion BIT           NOT NULL DEFAULT 0,
    Activo               BIT           NOT NULL DEFAULT 1,
    FechaCreacion        DATETIME2     NOT NULL DEFAULT SYSDATETIME(),
    FechaModificacion    DATETIME2     NULL,
    CONSTRAINT PK_FormaPago       PRIMARY KEY CLUSTERED (IdFormaPago),
    CONSTRAINT UK_FormaPago_Nombre UNIQUE (Nombre),
    CONSTRAINT CHK_FormaPago_Comision CHECK (ComisionPorcentaje BETWEEN 0 AND 100)
);
GO

-- ============================================================================
-- 2. SEGURIDAD: Usuario
-- ============================================================================

CREATE TABLE Usuario (
    IdUsuario         INT            IDENTITY(1,1) NOT NULL,
    Email             NVARCHAR(150)  NOT NULL,
    PasswordHash      NVARCHAR(256)  NOT NULL,
    NombreCompleto    NVARCHAR(150)  NOT NULL,
    Rol               NVARCHAR(30)   NOT NULL CHECK (Rol IN ('Admin','Veterinario','Asistente','Vendedor')),
    Especialidad      NVARCHAR(100)  NULL,
    NumeroColegiado   NVARCHAR(50)   NULL,
    Telefono          NVARCHAR(20)   NULL,
    UltimoAcceso      DATETIME2      NULL,
    Activo            BIT            NOT NULL DEFAULT 1,
    CreatedAt         DATETIME2      NOT NULL DEFAULT SYSDATETIME(),
    CreatedBy         NVARCHAR(150)  NULL,
    UpdatedAt         DATETIME2      NULL,
    UpdatedBy         NVARCHAR(150)  NULL,
    CONSTRAINT PK_Usuario        PRIMARY KEY CLUSTERED (IdUsuario),
    CONSTRAINT UK_Usuario_Email  UNIQUE (Email),
    CONSTRAINT CHK_Usuario_Email CHECK (Email LIKE '%_@__%.__%'),
    CONSTRAINT CHK_Usuario_Nombre CHECK (LEN(TRIM(NombreCompleto)) > 0)
);
GO

CREATE NONCLUSTERED INDEX IX_Usuario_Rol ON Usuario(Rol) WHERE Activo = 1;
GO

-- ============================================================================
-- 3. PERSONAS: Cliente y Proveedor
-- ============================================================================

CREATE TABLE Cliente (
    IdCliente             INT            IDENTITY(1,1) NOT NULL,
    NombreCompleto        NVARCHAR(150)  NOT NULL,
    TipoIdentificacion    NVARCHAR(30)   NOT NULL,
    NumIdentificacion     NVARCHAR(20)   NOT NULL,
    Email                 NVARCHAR(150)  NULL,
    TelefonoPrincipal     NVARCHAR(20)   NOT NULL,
    TelefonoAlternativo   NVARCHAR(20)   NULL,
    Direccion             NVARCHAR(300)  NOT NULL,
    Ciudad                NVARCHAR(100)  NOT NULL,
    ContactoEmergencia    NVARCHAR(150)  NULL,
    TelefonoEmergencia    NVARCHAR(20)   NULL,
    Observaciones         NVARCHAR(500)  NULL,
    Activo                BIT            NOT NULL DEFAULT 1,
    
    CreatedAt             DATETIME2      NOT NULL DEFAULT SYSDATETIME(),
    CreatedBy             NVARCHAR(150)  NULL,
    UpdatedAt             DATETIME2      NULL,
    UpdatedBy             NVARCHAR(150)  NULL,

    CONSTRAINT PK_Cliente PRIMARY KEY CLUSTERED (IdCliente),
    CONSTRAINT UK_Cliente_Identificacion UNIQUE (TipoIdentificacion, NumIdentificacion),
    CONSTRAINT CHK_Cliente_Nombre CHECK (LEN(TRIM(NombreCompleto)) > 0)
);
GO

CREATE NONCLUSTERED INDEX IX_Cliente_Nombre ON Cliente(NombreCompleto) WHERE Activo = 1;
CREATE NONCLUSTERED INDEX IX_Cliente_NumIdentificacion ON Cliente(NumIdentificacion) WHERE Activo = 1;
GO


CREATE TABLE Proveedor (
    IdProveedor        INT            IDENTITY(1,1) NOT NULL,
    TipoIdentificacion NVARCHAR(30)   NOT NULL,
    NumIdentificacion  NVARCHAR(20)   NOT NULL,
    RazonSocial        NVARCHAR(150)  NOT NULL,
    NombreContacto     NVARCHAR(150)  NULL,
    Email              NVARCHAR(150)  NULL,
    TelefonoPrincipal  NVARCHAR(20)   NULL,
    Direccion          NVARCHAR(300)  NULL,
    Distrito           NVARCHAR(100)  NULL,
    GiroComercial      NVARCHAR(100)  NULL,
    Observaciones      NVARCHAR(500)  NULL,
    Activo             BIT            NOT NULL DEFAULT 1,
    CreatedAt          DATETIME2      NOT NULL DEFAULT SYSDATETIME(),
    CreatedBy          NVARCHAR(150)  NULL,
    UpdatedAt          DATETIME2      NULL,
    UpdatedBy          NVARCHAR(150)  NULL,
    CONSTRAINT PK_Proveedor PRIMARY KEY CLUSTERED (IdProveedor),
    CONSTRAINT UK_Proveedor_Identificacion UNIQUE (TipoIdentificacion, NumIdentificacion),
    CONSTRAINT CHK_Proveedor_Razon CHECK (LEN(TRIM(RazonSocial)) > 0)
);
GO

-- ============================================================================
-- 4. PACIENTES: Mascota
-- ============================================================================

CREATE TABLE Mascota (
    IdMascota              INT            IDENTITY(1,1) NOT NULL,
    IdCliente              INT            NOT NULL,
    Nombre                 NVARCHAR(100)  NOT NULL,
    IdEspecie              INT            NOT NULL,
    IdRaza                 INT            NULL,
    FechaNacimiento        DATE           NULL,
    Sexo                   NCHAR(1)       NOT NULL DEFAULT 'M' CHECK (Sexo IN ('M','F')),
    ColorPelaje            NVARCHAR(100)  NULL,
    MicrochipId            NVARCHAR(50)   NOT NULL,
    FotoUrl                NVARCHAR(500)  NULL,
    FechaFallecimiento     DATE           NULL,
    Activo                 BIT            NOT NULL DEFAULT 1,
    CreatedAt              DATETIME2      NOT NULL DEFAULT SYSDATETIME(),
    CreatedBy              NVARCHAR(150)  NULL,
    UpdatedAt              DATETIME2      NULL,
    UpdatedBy              NVARCHAR(150)  NULL,
    CONSTRAINT PK_Mascota           PRIMARY KEY CLUSTERED (IdMascota),
    CONSTRAINT FK_Mascota_Cliente   FOREIGN KEY (IdCliente)  REFERENCES Cliente(IdCliente),
    CONSTRAINT FK_Mascota_Especie   FOREIGN KEY (IdEspecie)  REFERENCES Especie(IdEspecie),
    CONSTRAINT FK_Mascota_Raza      FOREIGN KEY (IdRaza)     REFERENCES Raza(IdRaza),
    CONSTRAINT UK_Mascota_Microchip UNIQUE (MicrochipId),
    CONSTRAINT CHK_Mascota_Nombre   CHECK (LEN(TRIM(Nombre)) > 0),
    CONSTRAINT CHK_Mascota_Fecha CHECK (FechaNacimiento IS NULL OR FechaNacimiento <= CAST(GETDATE() AS DATE))
);
GO

CREATE NONCLUSTERED INDEX IX_Mascota_IdCliente ON Mascota(IdCliente)  INCLUDE (Nombre, Activo);
CREATE NONCLUSTERED INDEX IX_Mascota_Nombre    ON Mascota(Nombre)     WHERE Activo = 1;
GO

-- ============================================================================
-- 5. PRODUCTOS Y SERVICIOS
-- ============================================================================

CREATE SEQUENCE seq_CodigoBarras_Producto
    START WITH 1000
    INCREMENT BY 1
    CACHE 50;
GO


CREATE TABLE Producto (
    IdProducto        INT            IDENTITY(1,1) NOT NULL,
    IdCategoria       INT            NOT NULL,
    CodigoBarras      NVARCHAR(50)   NOT NULL DEFAULT ('PROD-' + CAST(NEXT VALUE FOR seq_CodigoBarras_Producto AS NVARCHAR(10))),
    CodigoInterno     NVARCHAR(50)   NOT NULL,
    Nombre            NVARCHAR(200)  NOT NULL,
    Descripcion       NVARCHAR(1000) NULL,
    TipoProducto      NVARCHAR(20)   NOT NULL CHECK (TipoProducto IN ('Producto','Medicamento','Servicio')),
    PrecioVenta       DECIMAL(10,2)  NOT NULL CHECK (PrecioVenta >= 0),
    PrecioCosto       DECIMAL(10,2)  NOT NULL CHECK (PrecioCosto >= 0),
    UnidadMedida      NVARCHAR(20)   NOT NULL DEFAULT 'UNIDAD',
    RequiereReceta    BIT            NOT NULL DEFAULT 0,
    Activo            BIT            NOT NULL DEFAULT 1,
    CreatedAt         DATETIME2      NOT NULL DEFAULT SYSDATETIME(),
    CreatedBy         NVARCHAR(150)  NULL,
    UpdatedAt         DATETIME2      NULL,
    UpdatedBy         NVARCHAR(150)  NULL,
    CONSTRAINT PK_Producto PRIMARY KEY CLUSTERED (IdProducto),
    CONSTRAINT FK_Producto_Categoria FOREIGN KEY (IdCategoria) REFERENCES CategoriaProducto(IdCategoria),
    CONSTRAINT UK_Producto_CodigoBarras UNIQUE (CodigoBarras),
    CONSTRAINT UK_Producto_CodigoInterno UNIQUE (CodigoInterno),
    CONSTRAINT CHK_Producto_Nombre CHECK (LEN(TRIM(Nombre)) > 0)
);
GO

CREATE NONCLUSTERED INDEX IX_Producto_Categoria ON Producto(IdCategoria) INCLUDE (Nombre, PrecioVenta, Activo);
CREATE NONCLUSTERED INDEX IX_Producto_Nombre ON Producto(Nombre) WHERE Activo = 1;
GO

CREATE TABLE Servicio (
    IdServicio               INT            IDENTITY(1,1) NOT NULL,
    IdTipoServicio           INT            NOT NULL,
    CodigoServicio           NVARCHAR(20)   NOT NULL,
    Nombre                   NVARCHAR(200)  NOT NULL,
    Descripcion              NVARCHAR(1000) NULL,
    PrecioBase               DECIMAL(10,2)  NOT NULL CHECK (PrecioBase >= 0),
    RequiereAyuno            BIT            NOT NULL DEFAULT 0,
    RequierePreparacion      BIT            NOT NULL DEFAULT 0,
    InstruccionesPreparacion NVARCHAR(500)  NULL,
    Observaciones            NVARCHAR(500)  NULL,
    Activo                   BIT            NOT NULL DEFAULT 1,
    CreatedAt                DATETIME2      NOT NULL DEFAULT SYSDATETIME(),
    CreatedBy                NVARCHAR(150)  NULL,
    UpdatedAt                DATETIME2      NULL,
    UpdatedBy                NVARCHAR(150)  NULL,
    CONSTRAINT PK_Servicio           PRIMARY KEY CLUSTERED (IdServicio),
    CONSTRAINT FK_Servicio_Tipo      FOREIGN KEY (IdTipoServicio) REFERENCES TipoServicio(IdTipoServicio),
    CONSTRAINT UK_Servicio_Codigo    UNIQUE (CodigoServicio),
    CONSTRAINT CHK_Servicio_Nombre   CHECK (LEN(TRIM(Nombre)) > 0)
);
GO

CREATE NONCLUSTERED INDEX IX_Servicio_Tipo ON Servicio(IdTipoServicio) WHERE Activo = 1;
GO

-- ============================================================================
-- 6. ATENCIONES MÉDICAS
-- ============================================================================


CREATE TABLE GrupoAtencion (
    IdGrupo       INT           IDENTITY(1,1) NOT NULL,
    Fecha         DATE          NOT NULL,
    IdVeterinario INT           NOT NULL,
    Observaciones NVARCHAR(500) NULL,
    CreatedAt     DATETIME2     NOT NULL DEFAULT SYSDATETIME(),
    CONSTRAINT PK_GrupoAtencion      PRIMARY KEY CLUSTERED (IdGrupo),
    CONSTRAINT FK_GrupoAtencion_Vet  FOREIGN KEY (IdVeterinario) REFERENCES Usuario(IdUsuario)
);
GO

CREATE TABLE AtencionMascotaAdicional (
    IdGrupo      INT       NOT NULL,
    IdMascota    INT       NOT NULL,
    EsPrincipal  BIT       NOT NULL DEFAULT 0,
    CreatedAt    DATETIME2 NOT NULL DEFAULT SYSDATETIME(),
    CONSTRAINT PK_AtencionMascotaAdicional     PRIMARY KEY CLUSTERED (IdGrupo, IdMascota),
    CONSTRAINT FK_AtencionMascota_Grupo        FOREIGN KEY (IdGrupo)   REFERENCES GrupoAtencion(IdGrupo),
    CONSTRAINT FK_AtencionMascota_Mascota      FOREIGN KEY (IdMascota) REFERENCES Mascota(IdMascota)
);
GO


CREATE UNIQUE INDEX UX_AtencionMascota_UnPrincipal
    ON AtencionMascotaAdicional(IdGrupo)
    WHERE EsPrincipal = 1;
GO

CREATE TABLE Atencion (
    IdAtencion            INT            IDENTITY(1,1) NOT NULL,
    IdMascota             INT            NOT NULL,
    IdVeterinario         INT            NOT NULL,
    IdGrupo               INT            NULL,
    FechaHoraInicio       DATETIME2      NOT NULL DEFAULT SYSDATETIME(),
    FechaHoraFin          DATETIME2      NULL,
    TipoAtencion          NVARCHAR(50)   NOT NULL 
        CHECK (TipoAtencion IN ('Consulta','Urgencia','Cirugia','Vacunacion','Control','Hospitalizacion')),
    MotivoConsulta        NVARCHAR(1000) NOT NULL,
    Sintomas              NVARCHAR(2000) NULL,
    ExamenFisico          NVARCHAR(2000) NULL,
    Diagnostico           NVARCHAR(2000) NULL,
    Tratamiento           NVARCHAR(2000) NULL,
    Recomendaciones       NVARCHAR(2000) NULL,
    PesoAtencion          DECIMAL(5,2)   NULL,
    Temperatura           DECIMAL(4,1)   NULL,
    FrecuenciaCardiaca    INT            NULL,
    FrecuenciaRespiratoria INT           NULL,
    Estado                NVARCHAR(20)   NOT NULL 
        DEFAULT 'Programada' CHECK (Estado IN ('Programada','EnProceso','Completada','Cancelada')),
    Observaciones         NVARCHAR(500)  NULL,
    CreatedAt             DATETIME2      NOT NULL DEFAULT SYSDATETIME(),
    CreatedBy             NVARCHAR(150)  NULL,
    UpdatedAt             DATETIME2      NULL,
    UpdatedBy             NVARCHAR(150)  NULL,
    CONSTRAINT PK_Atencion          PRIMARY KEY CLUSTERED (IdAtencion),
    CONSTRAINT FK_Atencion_Mascota  FOREIGN KEY (IdMascota)     REFERENCES Mascota(IdMascota),
    CONSTRAINT FK_Atencion_Vet      FOREIGN KEY (IdVeterinario) REFERENCES Usuario(IdUsuario),
    CONSTRAINT FK_Atencion_Grupo    FOREIGN KEY (IdGrupo)       REFERENCES GrupoAtencion(IdGrupo),
    CONSTRAINT CHK_Atencion_Fechas  CHECK (FechaHoraFin IS NULL OR FechaHoraFin >= FechaHoraInicio),
    CONSTRAINT CHK_Atencion_Motivo  CHECK (LEN(TRIM(MotivoConsulta)) > 0)
);
GO

CREATE NONCLUSTERED INDEX IX_Atencion_Mascota ON Atencion(IdMascota) INCLUDE (FechaHoraInicio, TipoAtencion, Estado);
CREATE NONCLUSTERED INDEX IX_Atencion_Fecha ON Atencion(FechaHoraInicio) WHERE Estado != 'Cancelada';
CREATE NONCLUSTERED INDEX IX_Atencion_Veterinario ON Atencion(IdVeterinario, FechaHoraInicio);
GO

CREATE TABLE DetalleAtencion (
    IdDetalle          INT            IDENTITY(1,1) NOT NULL,
    IdAtencion         INT            NOT NULL,
    IdProducto         INT            NULL,
    IdServicio         INT            NULL,
    Cantidad           DECIMAL(10,3)  NOT NULL CHECK (Cantidad > 0),
    PrecioUnitario     DECIMAL(10,2)  NOT NULL CHECK (PrecioUnitario >= 0),
    DescuentoPorcentaje DECIMAL(5,2)  NOT NULL DEFAULT 0 CHECK (DescuentoPorcentaje BETWEEN 0 AND 100),
    Subtotal AS(Cantidad * PrecioUnitario * (1 - DescuentoPorcentaje / 100.0)) PERSISTED,
    TipoItem           NVARCHAR(20)   NOT NULL CHECK (TipoItem IN ('PRODUCTO','SERVICIO')),
    Dosis              NVARCHAR(100)  NULL,
    Frecuencia         NVARCHAR(100)  NULL,
    DuracionDias       INT            NULL CHECK (DuracionDias IS NULL OR DuracionDias > 0),
    Instrucciones      NVARCHAR(500)  NULL,
    Observaciones      NVARCHAR(500)  NULL,
    CreatedAt          DATETIME2      NOT NULL DEFAULT SYSDATETIME(),
    CONSTRAINT PK_DetalleAtencion           PRIMARY KEY CLUSTERED (IdDetalle),
    CONSTRAINT FK_DetalleAtencion_Atencion  FOREIGN KEY (IdAtencion)  REFERENCES Atencion(IdAtencion),
    CONSTRAINT FK_DetalleAtencion_Producto  FOREIGN KEY (IdProducto)  REFERENCES Producto(IdProducto),
    CONSTRAINT FK_DetalleAtencion_Servicio  FOREIGN KEY (IdServicio)  REFERENCES Servicio(IdServicio),
    CONSTRAINT CHK_DetalleAtencion_Item     CHECK (
        (IdProducto IS NOT NULL AND IdServicio IS NULL AND TipoItem = 'PRODUCTO') OR
        (IdProducto IS NULL  AND IdServicio IS NOT NULL AND TipoItem = 'SERVICIO')
    )
);
GO

CREATE NONCLUSTERED INDEX IX_DetalleAtencion_Atencion ON DetalleAtencion(IdAtencion) INCLUDE (TipoItem, Cantidad, PrecioUnitario);
CREATE NONCLUSTERED INDEX IX_DetalleAtencion_Producto ON DetalleAtencion(IdProducto) WHERE IdProducto IS NOT NULL;
CREATE NONCLUSTERED INDEX IX_DetalleAtencion_Servicio ON DetalleAtencion(IdServicio) WHERE IdServicio IS NOT NULL;
GO

-- ============================================================================
-- 7. VENTAS
-- ============================================================================

CREATE TABLE Venta (
    IdVenta            INT            IDENTITY(1,1) NOT NULL,
    IdCliente          INT            NOT NULL,
    IdAtencion         INT            NULL,
    IdUsuario          INT            NOT NULL,
    Fecha              DATETIME2      NOT NULL DEFAULT SYSDATETIME(),
    Subtotal           DECIMAL(12,2)  NOT NULL CHECK (Subtotal >= 0),
    Descuento          DECIMAL(12,2)  NOT NULL DEFAULT 0 CHECK (Descuento >= 0),
    Impuestos          DECIMAL(12,2)  NOT NULL DEFAULT 0 CHECK (Impuestos >= 0),
    Total              DECIMAL(12,2)  NOT NULL CHECK (Total >= 0),
    IdFormaPago        INT            NOT NULL,
    EstadoPago         NVARCHAR(20)   NOT NULL 
        DEFAULT 'Pendiente' CHECK (EstadoPago IN ('Pagado','Pendiente','Parcial','Anulado')),
    NumeroComprobante  NVARCHAR(50)   NOT NULL,
    RutaPDF            NVARCHAR(500)  NULL,
    Observaciones      NVARCHAR(500)  NULL,
    CreatedAt          DATETIME2      NOT NULL DEFAULT SYSDATETIME(),
    CreatedBy          NVARCHAR(150)  NULL,
    UpdatedAt          DATETIME2      NULL,
    UpdatedBy          NVARCHAR(150)  NULL,
    CONSTRAINT PK_Venta                  PRIMARY KEY CLUSTERED (IdVenta),
    CONSTRAINT FK_Venta_Cliente          FOREIGN KEY (IdCliente)   REFERENCES Cliente(IdCliente),
    CONSTRAINT FK_Venta_Atencion         FOREIGN KEY (IdAtencion)  REFERENCES Atencion(IdAtencion),
    CONSTRAINT FK_Venta_Usuario          FOREIGN KEY (IdUsuario)   REFERENCES Usuario(IdUsuario),
    CONSTRAINT FK_Venta_FormaPago        FOREIGN KEY (IdFormaPago) REFERENCES FormaPago(IdFormaPago),
    CONSTRAINT UK_Venta_Comprobante      UNIQUE (NumeroComprobante),
    CONSTRAINT CHK_Venta_Total           CHECK (Total = Subtotal - Descuento + Impuestos)
);
GO

CREATE NONCLUSTERED INDEX IX_Venta_Fecha    ON Venta(Fecha)             INCLUDE (Total, EstadoPago);
CREATE NONCLUSTERED INDEX IX_Venta_Cliente  ON Venta(IdCliente, Fecha)  INCLUDE (Total);
CREATE NONCLUSTERED INDEX IX_Venta_Atencion ON Venta(IdAtencion)        WHERE IdAtencion IS NOT NULL;
GO

CREATE TABLE DetalleVenta (
    IdDetalle           INT           IDENTITY(1,1) NOT NULL,
    IdVenta             INT           NOT NULL,
    IdProducto          INT           NOT NULL,
    Cantidad            DECIMAL(10,3) NOT NULL CHECK (Cantidad > 0),
    PrecioUnitario      DECIMAL(10,2) NOT NULL CHECK (PrecioUnitario >= 0),
    DescuentoPorcentaje DECIMAL(5,2)  NOT NULL DEFAULT 0 CHECK (DescuentoPorcentaje BETWEEN 0 AND 100),
    Lote                NVARCHAR(50)  NULL,
    Subtotal AS(Cantidad * PrecioUnitario * (1 - DescuentoPorcentaje / 100.0)) PERSISTED,
    FechaVencimiento    DATE          NULL,
    Observaciones       NVARCHAR(200) NULL,
    CreatedAt           DATETIME2     NOT NULL DEFAULT SYSDATETIME(),
    CONSTRAINT PK_DetalleVenta         PRIMARY KEY CLUSTERED (IdDetalle),
    CONSTRAINT FK_DetalleVenta_Venta   FOREIGN KEY (IdVenta)     REFERENCES Venta(IdVenta),
    CONSTRAINT FK_DetalleVenta_Prod    FOREIGN KEY (IdProducto)  REFERENCES Producto(IdProducto)
);
GO

CREATE NONCLUSTERED INDEX IX_DetalleVenta_Venta   ON DetalleVenta(IdVenta)    INCLUDE (IdProducto, Cantidad);
CREATE NONCLUSTERED INDEX IX_DetalleVenta_Producto ON DetalleVenta(IdProducto) INCLUDE (Cantidad);
GO

-- ============================================================================
-- 8. COMPRAS (ahora con FK a Proveedor)
-- ============================================================================

CREATE TABLE Compra (
    IdCompra         INT            IDENTITY(1,1) NOT NULL,
    IdProveedor      INT            NOT NULL,
    IdUsuario        INT            NOT NULL,
    Fecha            DATETIME2      NOT NULL DEFAULT SYSDATETIME(),
    Subtotal         DECIMAL(12,2)  NOT NULL CHECK (Subtotal >= 0),
    Impuestos        DECIMAL(12,2)  NOT NULL DEFAULT 0,
    Total            DECIMAL(12,2)  NOT NULL CHECK (Total >= 0),
    IdFormaPago      INT            NOT NULL,
    EstadoPago       NVARCHAR(20)   NOT NULL DEFAULT 'Pendiente' CHECK (EstadoPago IN ('Pagado','Pendiente','Parcial')),
    NumeroFactura    NVARCHAR(50)   NOT NULL,
    RutaDocumento    NVARCHAR(500)  NULL,
    Observaciones    NVARCHAR(500)  NULL,
    CreatedAt        DATETIME2      NOT NULL DEFAULT SYSDATETIME(),
    CreatedBy        NVARCHAR(150)  NULL,
    UpdatedAt        DATETIME2      NULL,
    UpdatedBy        NVARCHAR(150)  NULL,
    CONSTRAINT PK_Compra              PRIMARY KEY CLUSTERED (IdCompra),
    CONSTRAINT FK_Compra_Proveedor    FOREIGN KEY (IdProveedor) REFERENCES Proveedor(IdProveedor),
    CONSTRAINT FK_Compra_Usuario      FOREIGN KEY (IdUsuario)   REFERENCES Usuario(IdUsuario),
    CONSTRAINT FK_Compra_FormaPago    FOREIGN KEY (IdFormaPago) REFERENCES FormaPago(IdFormaPago),
    CONSTRAINT UK_Compra_Factura      UNIQUE (IdProveedor, NumeroFactura),
    CONSTRAINT CHK_Compra_Total       CHECK (Total = Subtotal + Impuestos)
);
GO

CREATE NONCLUSTERED INDEX IX_Compra_Fecha ON Compra(Fecha) INCLUDE (Total, EstadoPago);
CREATE NONCLUSTERED INDEX IX_Compra_Proveedor ON Compra(IdProveedor, Fecha);
GO

CREATE TABLE DetalleCompra (
    IdDetalle         INT           IDENTITY(1,1) NOT NULL,
    IdCompra          INT           NOT NULL,
    IdProducto        INT           NOT NULL,
    Cantidad          DECIMAL(10,3) NOT NULL CHECK (Cantidad > 0),
    PrecioUnitario    DECIMAL(10,2) NOT NULL CHECK (PrecioUnitario >= 0),
    Lote              NVARCHAR(50)  NOT NULL,
    FechaVencimiento  DATE          NOT NULL,
    FechaFabricacion  DATE          NULL,
    Observaciones     NVARCHAR(200) NULL,
    CreatedAt         DATETIME2     NOT NULL DEFAULT SYSDATETIME(),
    CONSTRAINT PK_DetalleCompra         PRIMARY KEY CLUSTERED (IdDetalle),
    CONSTRAINT FK_DetalleCompra_Compra  FOREIGN KEY (IdCompra)   REFERENCES Compra(IdCompra),
    CONSTRAINT FK_DetalleCompra_Prod    FOREIGN KEY (IdProducto) REFERENCES Producto(IdProducto),
    CONSTRAINT CHK_DetalleCompra_Fechas CHECK (FechaFabricacion IS NULL OR FechaFabricacion <= FechaVencimiento)
);
GO

CREATE NONCLUSTERED INDEX IX_DetalleCompra_Compra ON DetalleCompra(IdCompra) INCLUDE (IdProducto, Cantidad);
CREATE NONCLUSTERED INDEX IX_DetalleCompra_Prod_Lote  ON DetalleCompra(IdProducto, Lote);
GO

-- ============================================================================
-- 9. INVENTARIO: Kardex y Lotes
-- ============================================================================

CREATE TABLE LoteProducto (
    IdLote           INT           IDENTITY(1,1) NOT NULL,
    IdProducto       INT           NOT NULL,
    NumeroLote       NVARCHAR(50)  NOT NULL,
    FechaVencimiento DATE          NOT NULL,
    FechaFabricacion DATE          NULL,
    CantidadInicial  DECIMAL(10,2) NOT NULL CHECK (CantidadInicial >= 0),
    CantidadActual   DECIMAL(10,2) NOT NULL CHECK (CantidadActual >= 0),
    FechaIngreso     DATETIME2     NOT NULL DEFAULT SYSDATETIME(),
    Activo           BIT           NOT NULL DEFAULT 1,
    Observaciones    NVARCHAR(200) NULL,
    CONSTRAINT PK_LoteProducto         PRIMARY KEY CLUSTERED (IdLote),
    CONSTRAINT FK_LoteProducto_Prod    FOREIGN KEY (IdProducto) REFERENCES Producto(IdProducto),
    CONSTRAINT UK_LoteProducto_ProdLote UNIQUE (IdProducto, NumeroLote),
    CONSTRAINT CHK_LoteProducto_Fechas CHECK (FechaFabricacion IS NULL OR FechaFabricacion <= FechaVencimiento)
);
GO

CREATE NONCLUSTERED INDEX IX_LoteProducto_Venc ON LoteProducto(FechaVencimiento) WHERE Activo = 1 AND CantidadActual > 0;
CREATE NONCLUSTERED INDEX IX_LoteProducto_Producto ON LoteProducto(IdProducto, FechaVencimiento) WHERE CantidadActual > 0;
GO

CREATE TABLE Kardex (
    IdKardex          INT            IDENTITY(1,1) NOT NULL,
    IdProducto        INT            NOT NULL,
    IdLote            INT            NOT NULL,
    Cantidad          DECIMAL(10,2)  NOT NULL,
    StockAnterior     DECIMAL(10,2)  NOT NULL,
    StockNuevo        DECIMAL(10,2)  NOT NULL,
    TipoMovimiento    NVARCHAR(20)   NOT NULL 
        CHECK (TipoMovimiento IN ('VENTA','COMPRA','AJUSTE','DEVOLUCION','MERMA','CONSULTA','TRANSFERENCIA')),
    IdDocumentoRef    INT            NULL,
    TipoDocumentoRef  NVARCHAR(20)   NULL,
    IdUsuario         INT            NULL,
    NombreUsuario     NVARCHAR(150)  NULL,
    Fecha             DATETIME2      NOT NULL DEFAULT SYSDATETIME(),
    Observaciones     NVARCHAR(500)  NULL,
    CONSTRAINT PK_Kardex         PRIMARY KEY CLUSTERED (IdKardex),
    CONSTRAINT FK_Kardex_Producto FOREIGN KEY (IdProducto) REFERENCES Producto(IdProducto),
    CONSTRAINT FK_Kardex_Lote FOREIGN KEY (IdLote) REFERENCES LoteProducto(IdLote),
    CONSTRAINT CHK_Kardex_Cant   CHECK (Cantidad != 0)
);
GO

CREATE NONCLUSTERED INDEX IX_Kardex_Prod_Fecha ON Kardex(IdProducto, Fecha) INCLUDE (Cantidad, TipoMovimiento, StockNuevo);
CREATE NONCLUSTERED INDEX IX_Kardex_Fecha ON Kardex(Fecha) INCLUDE (IdProducto, Cantidad, TipoMovimiento);
GO

-- ============================================================================
-- 10. SALUD: Vacunas, Historial, Desparasitación
-- ============================================================================

CREATE TABLE Vacuna (
    IdVacuna                INT           IDENTITY(1,1) NOT NULL,
    Nombre                  NVARCHAR(100) NOT NULL,
    Tipo                    NVARCHAR(30)  NOT NULL CHECK (Tipo IN ('Obligatoria','Opcional','Recomendada')),
    FrecuenciaRefuerzoMeses INT           NULL CHECK (FrecuenciaRefuerzoMeses IS NULL OR FrecuenciaRefuerzoMeses > 0),
    EdadPrimeraDosisSemanas INT           NULL CHECK (EdadPrimeraDosisSemanas IS NULL OR EdadPrimeraDosisSemanas > 0),
    Laboratorio             NVARCHAR(100) NULL,
    EnfermedadesPrevine     NVARCHAR(500) NULL,
    Activo                  BIT           NOT NULL DEFAULT 1,
    FechaCreacion           DATETIME2     NOT NULL DEFAULT SYSDATETIME(),
    FechaModificacion       DATETIME2     NULL,
    CONSTRAINT PK_Vacuna        PRIMARY KEY CLUSTERED (IdVacuna),
    CONSTRAINT UK_Vacuna_Nombre  UNIQUE (Nombre),
    CONSTRAINT CHK_Vacuna_Nombre CHECK (LEN(TRIM(Nombre)) > 0)
);
GO

CREATE TABLE HistorialVacunas (
    IdHistorial          INT           IDENTITY(1,1) NOT NULL,
    IdMascota            INT           NOT NULL,
    IdVacuna             INT           NOT NULL,
    FechaAplicacion      DATETIME2     NOT NULL DEFAULT SYSDATETIME(),
    FechaProximoRefuerzo DATE          NULL,
    IdVeterinario        INT           NOT NULL,
    IdLote               INT           NOT NULL,
    DosisNumero          INT           NOT NULL DEFAULT 1 CHECK (DosisNumero > 0),
    ReaccionesAdversas   NVARCHAR(500) NULL,
    Observaciones        NVARCHAR(500) NULL,
    CreatedAt            DATETIME2     NOT NULL DEFAULT SYSDATETIME(),
    CONSTRAINT PK_HistorialVacunas          PRIMARY KEY CLUSTERED (IdHistorial),
    CONSTRAINT FK_HistorialVacunas_Mascota  FOREIGN KEY (IdMascota)     REFERENCES Mascota(IdMascota),
    CONSTRAINT FK_HistorialVacunas_Vacuna   FOREIGN KEY (IdVacuna)      REFERENCES Vacuna(IdVacuna),
    CONSTRAINT FK_HistorialVacunas_Vet      FOREIGN KEY (IdVeterinario) REFERENCES Usuario(IdUsuario),
    CONSTRAINT FK_HistorialVacunas_Lotes    FOREIGN KEY (IdLote)        REFERENCES LoteProducto(IdLote),
    CONSTRAINT CHK_HistorialVacunas_Fechas  
        CHECK (FechaProximoRefuerzo IS NULL OR FechaProximoRefuerzo >= CAST(FechaAplicacion AS DATE))
);
GO

CREATE NONCLUSTERED INDEX IX_HistVac_Mascota   ON HistorialVacunas(IdMascota)          INCLUDE (FechaAplicacion, FechaProximoRefuerzo);
CREATE NONCLUSTERED INDEX IX_HistVac_Refuerzo  ON HistorialVacunas(FechaProximoRefuerzo) WHERE FechaProximoRefuerzo IS NOT NULL;
GO

CREATE TABLE Desparasitacion (
    IdDesparasitacion   INT           IDENTITY(1,1) NOT NULL,
    IdMascota           INT           NOT NULL,
    TipoDesparasitacion NVARCHAR(20)  NOT NULL,
    IdProducto          INT           NOT NULL,
    FechaAplicacion     DATETIME2     NOT NULL DEFAULT SYSDATETIME(),
    FechaProxima        DATE          NULL,
    IdVeterinario       INT           NOT NULL,
    PesoAplicacion      DECIMAL(5,2)  NULL,
    DosisAplicada       NVARCHAR(100) NULL,
    Observaciones       NVARCHAR(500) NULL,
    CreatedAt           DATETIME2     NOT NULL DEFAULT SYSDATETIME(),
    CONSTRAINT PK_Desparasitacion          PRIMARY KEY CLUSTERED (IdDesparasitacion),
    CONSTRAINT FK_Desparasitacion_Mascota  FOREIGN KEY (IdMascota)     REFERENCES Mascota(IdMascota),
    CONSTRAINT FK_Desparasitacion_Vet      FOREIGN KEY (IdVeterinario) REFERENCES Usuario(IdUsuario),
    CONSTRAINT FK_Desparasitacion_Producto FOREIGN KEY (IdProducto) REFERENCES Producto(IdProducto),
    CONSTRAINT CHK_Desparasitacion_Fechas  CHECK (FechaProxima IS NULL OR FechaProxima >= CAST(FechaAplicacion AS DATE))
);
GO

CREATE NONCLUSTERED INDEX IX_Desparasitacion_Mascota ON Desparasitacion(IdMascota, FechaAplicacion);
CREATE NONCLUSTERED INDEX IX_Desparasitacion_Proxima ON Desparasitacion(FechaProxima) WHERE FechaProxima IS NOT NULL;
GO

-- ============================================================================
-- 11. AUDITORÍA
-- ============================================================================

CREATE TABLE AuditLog (
    IdAuditLog        BIGINT         IDENTITY(1,1) NOT NULL,
    Tabla             NVARCHAR(100)  NOT NULL,
    IdRegistro        INT            NOT NULL,
    Accion            NVARCHAR(10)   NOT NULL CHECK (Accion IN ('INSERT','UPDATE','DELETE')),
    ValoresAnteriores NVARCHAR(MAX)  NULL,
    ValoresNuevos     NVARCHAR(MAX)  NULL,
    IdUsuario         INT            NULL,
    NombreUsuario     NVARCHAR(150)  NULL,
    IpOrigen          NVARCHAR(50)   NULL,
    Fecha             DATETIME2      NOT NULL DEFAULT SYSDATETIME(),
    CONSTRAINT PK_AuditLog PRIMARY KEY CLUSTERED (IdAuditLog)
);
GO

CREATE NONCLUSTERED INDEX IX_AuditLog_Tabla_Fecha ON AuditLog(Tabla, Fecha);
CREATE NONCLUSTERED INDEX IX_AuditLog_Registro    ON AuditLog(IdRegistro, Tabla);
GO
