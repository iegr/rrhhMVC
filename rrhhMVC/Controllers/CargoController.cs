using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using rrhhMVC.Models; //Se debe importar los modelos para poder hacer referencia a ellos
using PagedList; //Namespace usado para implementar Paginación y debe ser instalado.  
                 //Detalles de la implementacion de PagedList en: https://docs.microsoft.com/en-us/aspnet/mvc/overview/getting-started/getting-started-with-ef-using-mvc/sorting-filtering-and-paging-with-the-entity-framework-in-an-asp-net-mvc-application

namespace rrhhMVC.Controllers
{
    public class CargoController : Controller
    {
        //
        // GET: /Cargo/
        public ActionResult Index(string sortOrder, string searchString, string currentFilter, int? page)
        {
            //guardamos el orden actual para paginación
            ViewBag.CurrentSort = sortOrder; 
            
            //ViewBags usados para ordenar los registros
            ViewBag.IdSortParm = String.IsNullOrEmpty(sortOrder) ? "id_desc" : "";
            ViewBag.DescSortParm = sortOrder == "Desc" ? "desc_desc" : "Desc";

            //Preparamos la Paginación
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            ViewBag.CurrentFilter = searchString;


            using (rrhhContext db = new rrhhContext())
            {
                //List<Cargos> listadoCargos = db.Cargos.ToList(); 
				//usamos Linq
                var listadoCargos = from c in db.Cargos
                                    select c;

                //Aplicamos el filtro a la lista si existe alguno
                if (!String.IsNullOrEmpty(searchString))
                {
                    listadoCargos = listadoCargos.Where(c => c.DescripcionCargo.Contains(searchString));                                             
                }

                //Aplicamos el orden por la columna que corresponda 
                switch (sortOrder)
                {
                    case "id_desc":
                        listadoCargos = listadoCargos.OrderByDescending (c => c.IdCargo);
                        break;
                    case "Desc":
                        listadoCargos = listadoCargos.OrderBy (c => c.DescripcionCargo);
                        break;
                    case "desc_desc":
                        listadoCargos = listadoCargos.OrderByDescending(c => c.DescripcionCargo);
                        break;
                    default:
                        listadoCargos = listadoCargos.OrderBy (c => c.IdCargo);
                        break;
                }

                //Retorna los registros de la pagina que corresponda
                int pageSize = 8;
                int pageNumber = (page ?? 1);
                return View(listadoCargos.ToPagedList(pageNumber, pageSize));
                //return View(listadoCargos.ToList());
            }            
        }

        //Metodo para agregar un nuevo registro
        public ActionResult Agregar()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Agregar(Cargos cargo)
        {
            if (!ModelState.IsValid) //si el modelo no es valido (el modelo son las propiedades en la clase cargo, retorna la vista. Esto lo usamos en conjunto con los DataAnotations
            {
                return View();
            }

            try
            {
                using (rrhhContext db = new rrhhContext()) //Al hacer el using se le está diciendo que abra la conexión y a la vez la cierre
                {
                    cargo.IdCargo = cargo.IdCargo.ToUpper();
                    cargo.DescripcionCargo = cargo.DescripcionCargo.ToUpper();

                    db.Cargos.Add(cargo);
                    db.SaveChanges();
                    
                    return RedirectToAction ("Index");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Posible Id Duplicado: " + ex.Message);
                return View();
            }            
            
        }

        //Método para modificar un registro
        public ActionResult Editar(string idcargo)
        {
            try
            {
                using (rrhhContext db = new rrhhContext())
                {
                    Cargos cargo = db.Cargos.Find(idcargo); //usamos Find cuando una sola columna es la clave primaria de la tabla
                    //Cargos cargo = db.Cargos.Where(c => c.IdCargo == idcargo).FirstOrDefault(); //este Where se usa cuando la clave primaria de la tabla esta compuesta por mas de una columna
                    return View(cargo);
                }
                
            }
            catch (Exception ex)
            {                
                ModelState.AddModelError("", ex.Message);
                return View();
            }
            
            
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Editar(Cargos cargo)
        {
            return View();
        }

	}
}