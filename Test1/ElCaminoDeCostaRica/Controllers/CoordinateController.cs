using System.Collections.Generic;
using System.Web.Mvc;
using ElCaminoDeCostaRica.Models;
using Newtonsoft.Json;

namespace ElCaminoDeCostaRIca.Controllers
{
    public class CoordinateController : Controller
    {
        Database database = new Database();

        [HttpPost]
        public bool addCoordinates(string coordinates)
        {
            bool success = false;
            var deserializedCoords = JsonConvert.DeserializeObject<List<Coordinate>>(coordinates);
            database.openConnection();
            foreach (var coordinate in deserializedCoords)
            {
                success = database.addCoordinate(coordinate);
                if (success)
                {
                    ViewBag.Message = "Las coordenadas de la etapa fueron creadas con éxito.";
                }
                else {
                    ViewBag.Message = "Algo salió mal al crear las coordenadas.";
                }
            }
            database.closeConnection();
            return success;
        }
    }
}