Documento de Diseño - Sistema de Monitoreo IoT Simon Movilidad

Este documento detalla las decisiones de arquitectura y diseño técnico tomadas para el desarrollo del sistema de monitoreo de flotas vehiculares.

1. Arquitectura General 🚀
La base del sistema es una Arquitectura Limpia (Clean Architecture) combinada con el patrón CQRS (Command Query Responsibility Segregation).
¿Por qué esta elección?

Separación de Responsabilidades: Cada capa (Domain, Application, Infrastructure, API) tiene un propósito único, lo que hace que el código sea fácil de entender, mantener y escalar.

Alta Testeabilidad: La lógica de negocio más crítica vive en la capa de Application y no depende de detalles externos como la base de datos o la API. Esto nos permitió crear pruebas unitarias aisladas y fiables para funcionalidades como el cálculo de velocidad y la generación de alertas.

Independencia: El núcleo de la aplicación es agnóstico a la base de datos o al framework de la API, lo que facilita futuras migraciones o cambios tecnológicos.

Para la implementación de CQRS, se utilizó la librería MediatR, que actúa como un despachador en memoria para desacoplar los Commands y Queries de sus Handlers, manteniendo la cohesión de los casos de uso.

2. Diseño del Backend (.NET)

Base de Datos
Se eligió PostgreSQL por su robustez y excelente rendimiento con datos de series temporales. El modelo de datos se centra en Users, Vehicles, SensorReadings y Alerts.

Diagrama Entidad-Relación (ERD)
La estructura de la base de datos se visualiza en el siguiente diagrama:
docs/images/ERD Base de datos.png

Decisiones Clave del Modelo:
  - BIGSERIAL vs. UUID: Para la tabla de alto volumen SensorReadings, se usó BIGSERIAL para maximizar el rendimiento de inserción. Para las demás entidades, se usó UUID para garantizar la unicidad global.

  - Separación de Vehicle.Id y Vehicle.DeviceId: Vehicle.Id es el identificador permanente del vehículo, mientras que Vehicle.DeviceId es el del hardware IoT instalado. Esta separación permite que un vehículo mantenga un historial consolidado aunque su hardware sea reemplazado.

Lógica de Persistencia
Se implementó una estrategia híbrida:

  - Entity Framework Core: Se utiliza para todas las operaciones de escritura (Comandos), garantizando la consistencia transaccional.

  - Dapper: Se utiliza para las operaciones de lectura (Consultas) que alimentan el dashboard, escribiendo consultas SQL optimizadas a mano para obtener el máximo rendimiento.

Funcionalidades Clave
  - Autenticación JWT: Se implementó la generación y validación de tokens usando las herramientas nativas de .NET, configurando explícitamente los claims (incluyendo un "role" limpio) y el algoritmo de firma HS256.

  - Comunicación en Tiempo Real: Se eligió SignalR por ser la solución estándar de .NET para WebSockets. Se desacopló de la capa Application mediante la interfaz ITelemetryNotifier.

  - Validación: Se usó FluentValidation integrado con un PipelineBehavior de MediatR para ejecutar validaciones de forma automática y centralizada.

3. Diseño del Frontend (React)
Stack Tecnológico
  - Web: React con Vite por su velocidad de desarrollo.

  - Móvil: React Native con Expo para un desarrollo rápido y multiplataforma.

  - Enrutamiento: React Router (web) y Expo Router (móvil).

  - Comunicación API: Axios, configurado con interceptores para añadir automáticamente el token JWT y manejar errores globales.

  - Gestión de Estado: Zustand por su simplicidad, usando hooks personalizados (useAuth, useDashboard) para encapsular la lógica y reutilizarla entre la web y el móvil.

  - Mapas: Se eligió Leaflet (react-leaflet para web) y react-native-maps (para móvil) por su fiabilidad y facilidad de uso sin claves de API complejas.

Funcionalidades Clave
  - Reutilización de Lógica: Gracias a la separación de la lógica (hooks, stores, servicios) de la UI, más del 80% del código del frontend se pudo compartir entre la aplicación web y la móvil.

  - Resiliencia Offline: La aplicación implementa una estrategia "cache-first" usando el middleware persist de Zustand y localStorage / AsyncStorage. Al iniciar, muestra instantáneamente los datos guardados en caché y luego busca actualizaciones.

  - Renderizado Condicional Basado en Rol: La UI se adapta al rol del usuario (obtenido del token JWT), mostrando u ocultando componentes como el panel de alertas.

  - Notificaciones Push (Planificado): El diseño de la app móvil está preparado para integrar notificaciones push. El plan consiste en que la app obtenga un token de notificaciones de Expo/FCM, lo envíe a un nuevo endpoint del backend, y que el backend lo use para enviar una notificación cuando se genere una alerta.