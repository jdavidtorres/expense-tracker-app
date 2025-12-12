# Migración a SQLite - Expense Tracker App

## Cambios Realizados

Se ha migrado la aplicación de usar una API REST HTTP a usar **SQLite como fuente de datos local**. La aplicación ahora almacena todos los datos localmente sin realizar peticiones a ningún endpoint externo.

## Archivos Nuevos Creados

### 1. Entidades de Base de Datos
- **`ExpenseTracker/Data/Entities/SubscriptionEntity.cs`**
  - Entidad SQLite para almacenar suscripciones
  - Campos: Id, Name, Amount, BillingCycle, NextBillingDate, Category, Notes, CreatedAt, UpdatedAt

- **`ExpenseTracker/Data/Entities/InvoiceEntity.cs`**
  - Entidad SQLite para almacenar facturas
  - Campos: Id, Name, Amount, DueDate, Status, Category, Notes, CreatedAt, UpdatedAt

### 2. Servicio de Base de Datos
- **`ExpenseTracker/Data/DatabaseService.cs`**
  - Gestiona todas las operaciones con SQLite
  - Métodos CRUD para Subscriptions e Invoices
  - Métodos de resumen (mensual, anual, por categoría)
  - Base de datos almacenada en: `{AppDataDirectory}/expensetracker.db3`

### 3. Servicio Local de Gastos
- **`ExpenseTracker/Services/LocalExpenseService.cs`**
  - Reemplaza completamente a `ExpenseService` (que usaba HTTP)
  - Mantiene la misma interfaz para minimizar cambios en ViewModels
  - Mapea entre entidades SQLite y modelos de dominio
  - No hace peticiones HTTP, todo es local

## Archivos Modificados

### 1. Proyecto
- **`ExpenseTracker/ExpenseTracker.csproj`**
  - Agregados paquetes NuGet:
    - `sqlite-net-pcl` (v1.9.172)
    - `SQLitePCLRaw.bundle_green` (v2.1.10)

### 2. Registro de Servicios
- **`ExpenseTracker/Extensions/ServiceCollectionExtensions.cs`**
  - Registra `DatabaseService` como Singleton
  - Registra `LocalExpenseService` como Singleton (reemplaza a ExpenseService)
  - Eliminado el registro de HttpClient para ExpenseService

### 3. ViewModels Actualizados
Todos los ViewModels ahora usan `LocalExpenseService` en lugar de `ExpenseService`:

- **`ExpenseTracker/ViewModels/DashboardViewModel.cs`**
- **`ExpenseTracker/ViewModels/SubscriptionsViewModel.cs`**
- **`ExpenseTracker/ViewModels/InvoicesViewModel.cs`**
- **`ExpenseTracker/ViewModels/SubscriptionFormViewModel.cs`**
- **`ExpenseTracker/ViewModels/InvoiceFormViewModel.cs`**

## Características de SQLite

### Ubicación de la Base de Datos
```csharp
Path: FileSystem.AppDataDirectory + "/expensetracker.db3"
```

En cada plataforma:
- **Android**: `/data/data/com.companyname.expensetracker/files/`
- **iOS**: `~/Library/Application Support/`
- **Windows**: `%LOCALAPPDATA%/Packages/.../LocalState/`
- **macOS**: `~/Library/Application Support/`

### Tablas Creadas

#### Tabla: `subscriptions`
| Columna | Tipo | Descripción |
|---------|------|-------------|
| Id | INTEGER PRIMARY KEY AUTOINCREMENT | ID único |
| Name | TEXT NOT NULL | Nombre de la suscripción |
| Amount | DECIMAL NOT NULL | Monto |
| BillingCycle | TEXT NOT NULL | Ciclo de facturación (Weekly, Monthly, etc.) |
| NextBillingDate | DATETIME NOT NULL | Próxima fecha de facturación |
| Category | TEXT | Categoría |
| Notes | TEXT | Notas adicionales |
| CreatedAt | DATETIME | Fecha de creación |
| UpdatedAt | DATETIME | Fecha de última actualización |

#### Tabla: `invoices`
| Columna | Tipo | Descripción |
|---------|------|-------------|
| Id | INTEGER PRIMARY KEY AUTOINCREMENT | ID único |
| Name | TEXT NOT NULL | Nombre de la factura |
| Amount | DECIMAL NOT NULL | Monto |
| DueDate | DATETIME NOT NULL | Fecha de vencimiento |
| Status | TEXT NOT NULL | Estado (Pending, Paid, Overdue, Cancelled) |
| Category | TEXT | Categoría |
| Notes | TEXT | Notas adicionales |
| CreatedAt | DATETIME | Fecha de creación |
| UpdatedAt | DATETIME | Fecha de última actualización |

## Ventajas de la Migración

### ✅ Ventajas
1. **Sin Dependencias de Red**: La app funciona completamente offline
2. **Privacidad**: Los datos permanecen en el dispositivo del usuario
3. **Velocidad**: Acceso instantáneo sin latencia de red
4. **Confiabilidad**: No hay errores de conectividad
5. **Simplicidad**: No requiere backend ni servidor
6. **Portabilidad**: Funciona en todas las plataformas MAUI

### 🎯 Consideraciones
- Los datos NO se sincronizan entre dispositivos
- Cada instalación tiene su propia base de datos local
- Las copias de seguridad deben manejarse a nivel de sistema operativo
- Para migrar datos, se debe exportar/importar manualmente

## Estructura de Datos

### Mapeo de Enums a Strings

Los enums se almacenan como strings en SQLite para mayor claridad:

**BillingCycle:**
- `"Weekly"`, `"Monthly"`, `"Quarterly"`, `"SemiAnnually"`, `"Annually"`

**InvoiceStatus:**
- `"Pending"`, `"Paid"`, `"Overdue"`, `"Cancelled"`

### Conversión de IDs

Los IDs se manejan internamente como `int` en SQLite, pero se exponen como `string` en los modelos para mantener compatibilidad con el diseño anterior.

## Uso de la Base de Datos

### Operaciones CRUD Automáticas

El `DatabaseService` maneja automáticamente:
- ✅ Creación de tablas al iniciar la app
- ✅ Insert si `Id == 0`
- ✅ Update si `Id != 0`
- ✅ Timestamps automáticos (CreatedAt, UpdatedAt)

### Consultas de Resumen

```csharp
// Resumen mensual
var monthlyTotal = await databaseService.GetMonthlyTotalAsync(2024, 12);

// Resumen anual
var yearlyTotal = await databaseService.GetYearlyTotalAsync(2024);

// Por categoría
var categoryTotals = await databaseService.GetCategorySummaryAsync();
```

## Testing de la Migración

### Verificar que Funcione
1. ✅ Compilación exitosa
2. ✅ Agregar nueva suscripción
3. ✅ Agregar nueva factura
4. ✅ Ver dashboard con resúmenes
5. ✅ Editar suscripción existente
6. ✅ Eliminar factura
7. ✅ Cerrar y reabrir la app (persistencia)

## Próximos Pasos Opcionales

### Mejoras Futuras
1. **Exportar/Importar Datos**: CSV o JSON para backup
2. **Búsqueda Avanzada**: Filtros por fecha, categoría, monto
3. **Reportes**: Gráficos y estadísticas detalladas
4. **Recordatorios**: Notificaciones para próximos pagos
5. **Múltiples Monedas**: Soporte para conversión de divisas
6. **Sync Cloud (Opcional)**: Azure SQL, Firebase, etc.

## Comandos de Desarrollo

### Limpiar Base de Datos (Para Testing)
Si necesitas reiniciar la base de datos durante desarrollo:

**Android:**
```bash
adb shell run-as com.companyname.expensetracker rm /data/data/com.companyname.expensetracker/files/expensetracker.db3
```

**iOS/macOS/Windows:**
Desinstalar y reinstalar la aplicación.

## Notas Técnicas

### Threading
- SQLite en .NET MAUI usa `SQLiteAsyncConnection`
- Todas las operaciones son async/await
- Thread-safe por defecto

### Rendimiento
- Índices automáticos en Primary Keys
- Para grandes volúmenes de datos (>10k registros), considerar índices adicionales
- Las consultas actuales son eficientes para uso típico

### Migraciones de Esquema
Para cambios futuros en la estructura de tablas:
1. Incrementar versión de base de datos
2. Implementar lógica de migración en `DatabaseService`
3. Usar `ALTER TABLE` o recrear tablas según necesidad

## Resumen

✨ **La aplicación ahora es completamente offline y usa SQLite como única fuente de datos.**

Todos los datos de suscripciones, facturas y resúmenes se almacenan y recuperan localmente, sin ninguna conexión a API externa.
