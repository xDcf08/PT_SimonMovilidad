Guía de Despliegue Local - Simon Movilidad
Esta guía contiene los pasos necesarios para configurar y ejecutar la aplicación completa (backend y frontend) en un entorno de desarrollo local.

1. Prerrequisitos
Asegúrate de tener instalado el siguiente software:
.NET SDK 8.0 (o superior).
PostgreSQL 15 (o superior).
Node.js 20.x (o superior) y pnpm.
Git.

2. Configuración del Backend (.NET API)
 1. Clonar el Repositorio:
  git clone https://github.com/xDcf08/PT_SimonMovilidad.git
  cd PT_SimonMovilidad

 2. Crear y Configurar la Base de Datos: 
  2.1. Crear la Base de Datos:
    Abre tu herramienta de gestión de PostgreSQL (pgAdmin, DBeaver) y ejecuta:
    CREATE DATABASE "SimonMovilidadDb";
  
  2.2. Crear las Tablas con el Script SQL
    Ahora, en la base de datos SimonMovilidadDb que acabas de crear, ejecuta el siguiente script SQL completo. Este comando creará todas las tablas, relaciones e índices necesarios.

    -- Script para crear el esquema de la base de datos

    -- Tabla para los usuarios del sistema
    CREATE TABLE "users" (
        id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
        email VARCHAR(255) UNIQUE NOT NULL,
        password_hash VARCHAR(255) NOT NULL,
        role VARCHAR(50) NOT NULL CHECK (role IN ('Admin', 'Viewer')),
        created_at TIMESTAMPTZ NOT NULL DEFAULT NOW()
    );

    -- Tabla para los vehículos de la flota
    CREATE TABLE "vehicles" (
        id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
        device_id VARCHAR(100) UNIQUE NOT NULL,
        license_plate VARCHAR(20) UNIQUE,
        avg_consumption DECIMAL(5, 2) NOT NULL,
        created_at TIMESTAMPTZ NOT NULL DEFAULT NOW()
    );

    -- Tabla para almacenar los datos de los sensores (telemetría)
    CREATE TABLE "sensor_readings" (
        id BIGSERIAL PRIMARY KEY,
        vehicle_id UUID NOT NULL REFERENCES "vehicles"(id) ON DELETE CASCADE,
        "timestamp" TIMESTAMPTZ NOT NULL DEFAULT NOW(),
        latitude DECIMAL(9, 6) NOT NULL,
        longitude DECIMAL(9, 6) NOT NULL,
        fuel_level DECIMAL(5, 2) NOT NULL,
        temperature DECIMAL(5, 2)
    );

    -- Índice para búsquedas rápidas por vehículo y fecha
    CREATE INDEX idx_vehicle_timestamp ON "sensor_readings" (vehicle_id, "timestamp" DESC);

    -- Tabla para registrar las alertas generadas
    CREATE TABLE "alerts" (
        id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
        vehicle_id UUID NOT NULL REFERENCES "vehicles"(id) ON DELETE CASCADE,
        alert_type VARCHAR(50) NOT NULL,
        message TEXT,
        "timestamp" TIMESTAMPTZ NOT NULL DEFAULT NOW(),
        is_resolved BOOLEAN NOT NULL DEFAULT FALSE
    );

 3. Configurar la Cadena de Conexión:
  Navega al proyecto de la API: src/API/SimonMovilidad.API/.
  Abre el archivo appsettings.json.
  Modifica el valor de DefaultConnection con tus credenciales de PostgreSQL.
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Port=5432;Database=SimonMovilidadDb;User Id=tu_usuario;Password=tu_contraseña;"
  }

 4. Iniciar el Backend:
  - Desde una terminal en la carpeta del proyecto de la API, ejecuta: dotnet run
  - El backend estará disponible en https://localhost:7241 (o el puerto que indique la terminal).

3. Configuración del Frontend (Web y Móvil)

Frontend Web

  1. Navega a la carpeta del proyecto frontend web.

  2. Crea un archivo .env.local y añade: VITE_API_BASE_URL=https://localhost:7241/api

  3. Instala dependencias y ejecuta:
    - pnpm install
    - pnpm dev
    La aplicación estará disponible en http://localhost:5173.

Frontend Móvil
  1. Navega a la carpeta del proyecto simon-movilidad-mobile.

  2. Crea un archivo .env y añade la URL de tu backend usando tu IP local y el puerto HTTP: EXPO_PUBLIC_API_BASE_URL=http://192.168.1.11:5108/api

  3. En el archivo app.config.ts, asegúrate de que android.usesCleartextTraffic esté en true para permitir conexiones locales.

  4. Instala dependencias y ejecuta:
    - pnpm install
    - pnpm start

  5. Escanea el código QR con la app Expo Go o presiona a para abrirlo en tu emulador de Android.