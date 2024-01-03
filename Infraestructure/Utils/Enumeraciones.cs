using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.ComponentModel.DataAnnotations;

[JsonConverter(typeof(StringEnumConverter))]
public enum TypeEstado
{ 
    [Display(Name = "ACTIVO")]
    ACTIVO = 1,
    [Display(Name = "INACTIVO")]
    INACTIVO = 2
}

[JsonConverter(typeof(StringEnumConverter))]
public enum TypeEstadoTarjeta
{
    [Display(Name = "ACTIVO")]
    ACTIVO = 1,
    [Display(Name = "INACTIVO")]
    INACTIVO = 2,
    [Display(Name = "BLOQUEADO")]  //Vista en pantalla
    BLOQUEADO = 3                  //llamado en el aplicativo , valor del llamado
}

[JsonConverter(typeof(StringEnumConverter))]
public enum TypeEstadoTarjetaConsulta
{
    [Display(Name = "TODO")]
    TODO = 0,
    [Display(Name = "ACTIVO")]
    ACTIVO = 1,
    [Display(Name = "INACTIVO")]
    INACTIVO = 2,
    [Display(Name = "BLOQUEADO")]  //Vista en pantalla
    BLOQUEADO = 3                  //llamado en el aplicativo , valor del llamado
}

[JsonConverter(typeof(StringEnumConverter))]
public enum TypeEstadoFacturaConsulta
{
    [Display(Name = "TODO")]
    TODO = 0,
    [Display(Name = "PENDIENTE")]
    PENDIENTE = 1,
    [Display(Name = "PROCESADA")]
    PROCESADA = 2,
    [Display(Name = "ANULADA")]
    ANULADA = 3,
    [Display(Name = "CANCELADA")]
    CANCELADA = 4
}

[JsonConverter(typeof(StringEnumConverter))]
public enum TypeEstadoFactura
{
    [Display(Name = "PENDIENTE")]
    PENDIENTE = 1,
    [Display(Name = "PROCESADA")]
    PROCESADA = 2,
    [Display(Name = "ANULADA")]
    ANULADA = 3,
    [Display(Name = "CANCELADA")]
    CANCELADA = 4
}

[JsonConverter(typeof(StringEnumConverter))]
public enum TypeRoles {
    [Display(Name = "ADMINISTRADOR")]
    ADMINISTRADOR = 1,
    [Display(Name = "PROCESOS")]
    PROCESOS = 2,
    [Display(Name = "REPORTES")]
    REPORTES = 3,
    [Display(Name = "CONSULTAS")]
    CONSULTAS = 4
}