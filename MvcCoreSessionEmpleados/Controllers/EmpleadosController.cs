using Microsoft.AspNetCore.Mvc;
using MvcCoreSessionEmpleados.Extensions;
using MvcCoreSessionEmpleados.Models;
using MvcCoreSessionEmpleados.Repositories;
using System.Threading.Tasks;

namespace MvcCoreSessionEmpleados.Controllers
{
    public class EmpleadosController : Controller
    {
        private RepositoryEmpleados repo;

        public EmpleadosController(RepositoryEmpleados repo)
        {
            this.repo = repo;
        }

        public async Task<IActionResult> SessionSalarios(int? salario)
        {
            if(salario != null)
            {
                int sumaTotal = 0;
                
                if(HttpContext.Session.GetString("SUMASALARIAL") != null)
                {
                    sumaTotal = HttpContext.Session.GetObject<int>("SUMASALARIAL");
                }
                sumaTotal += salario.Value;

                HttpContext.Session.SetObject("SUMASALARIAL", sumaTotal);
                ViewData["MENSAJE"] = "Salario almacenado: " + salario;
            }

            List<Empleado> empleados = await this.repo.GetEmpleadosAsync();

            return View(empleados);
        }

        public IActionResult SumaSalarial()
        {
            return View();
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> SessionEmpleados(int? idempleado)
        {
            if (idempleado != null)
            {
                Empleado empleado = await this.repo.FindEmpleadoAsync(idempleado.Value);
                List<Empleado> empleadosList;

                if (HttpContext.Session.GetObject<List<Empleado>>("EMPLEADOS") != null)
                {
                    empleadosList = HttpContext.Session.GetObject<List<Empleado>>("EMPLEADOS");
                }
                else 
                {
                    empleadosList= new List<Empleado>();
                }
                
                empleadosList.Add(empleado);

                HttpContext.Session.SetObject("EMPLEADOS", empleadosList);

                ViewData["MENSAJE"] = "Empleado " + empleado.Apellido + " almacenado correctamente";
            }

            List<Empleado> empleados = await this.repo.GetEmpleadosAsync();

            return View(empleados);
        }

        public IActionResult EmpleadosAlmacenados()
        {
            return View();
        }

        public async Task<IActionResult> SessionEmpleadosOk(int? idempleado)
        {
            if (idempleado != null)
            {
                List<int> idsEmpleados;

                if (HttpContext.Session.GetObject<List<int>>("IDSEMPLEADOS") != null)
                {
                    idsEmpleados = HttpContext.Session.GetObject<List<int>>("IDSEMPLEADOS");
                }
                else
                {
                    idsEmpleados = new List<int>();
                }

                idsEmpleados.Add(idempleado.Value);

                HttpContext.Session.SetObject("IDSEMPLEADOS", idsEmpleados);

                ViewData["MENSAJE"] = "Empleados almacenados: " + idsEmpleados.Count;
            }

            List<Empleado> empleados = await this.repo.GetEmpleadosAsync();
            
            return View(empleados);
        }

        public async Task<IActionResult> EmpleadosAlmacenadosOk()
        {
            List<int> idsEmpleados = HttpContext.Session.GetObject<List<int>>("IDSEMPLEADOS");
            
            if (idsEmpleados == null)
            {
                ViewData["MENSAJE"] = "No existen enp,eados en Session";
                
                return View();
            }
            else
            {
                List<Empleado> empleados = await this.repo.GetEmpleadosSessionAsync(idsEmpleados);
                
                return View(empleados);
            }
        }

        public async Task<IActionResult> SessionEmpleadosV4(int? idempleado)
        {
            if (idempleado != null)
            {
                List<int> idsEmpleados;

                if (HttpContext.Session.GetObject<List<int>>("IDSEMPLEADOS") != null)
                {
                    idsEmpleados = HttpContext.Session.GetObject<List<int>>("IDSEMPLEADOS");
                }
                else
                {
                    idsEmpleados = new List<int>();
                }

                idsEmpleados.Add(idempleado.Value);

                HttpContext.Session.SetObject("IDSEMPLEADOS", idsEmpleados);

                ViewData["MENSAJE"] = "Empleados almacenados: " + idsEmpleados.Count;
            }

            List<int> storedIds = HttpContext.Session.GetObject<List<int>>("IDSEMPLEADOS");
            List<Empleado> empleados;

            if (storedIds != null)
            {
                empleados = await this.repo.GetEmpleadosV4Async(storedIds);
            }
            else
            {
                empleados = await this.repo.GetEmpleadosAsync();
            }

            return View(empleados);
        }

        public async Task<IActionResult> EmpleadosAlmacenadosV4()
        {
            List<int> idsEmpleados = HttpContext.Session.GetObject<List<int>>("IDSEMPLEADOS");

            if (idsEmpleados == null)
            {
                ViewData["MENSAJE"] = "No existen enp,eados en Session";

                return View();
            }
            else
            {
                List<Empleado> empleados = await this.repo.GetEmpleadosSessionAsync(idsEmpleados);

                return View(empleados);
            }
        }
    }
}