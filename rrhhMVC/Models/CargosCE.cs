using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations; //Esto nos permite hacer las validaciones

namespace rrhhMVC.Models
{    
    //Esta clase es usada para que los dataanottations no se pierdan al volver a generar el modelo 
    //Se debe hacer primero una copia exacta de la clase que genera el modelo automaticamente (Cargos.cs)
    //Aqui podemos hacer validaciones y extender la clase original de ser necesario.
    //

    public class CargosCE
    {          
        [Required(ErrorMessage = "El ID del Cargo es Obligatorio")]
        [Display (Name = "ID del Cargo")]
        [MaxLength (15)]
        public string IdCargo { get; set; }

        [Required(ErrorMessage = "La Descripción del Cargo es Obligatoria")]
        [Display (Name = "Descripción del Cargo")]
        [MaxLength(100)]
        public string DescripcionCargo { get; set; }
    
        
    }

    [MetadataType(typeof(CargosCE))] //Este codigo hace que la Clase Cargos (se indica abajo como clase parcial) sea reemplazada por la clase auxiliar CargosCE
    public partial class Cargos
    {

    }
}
