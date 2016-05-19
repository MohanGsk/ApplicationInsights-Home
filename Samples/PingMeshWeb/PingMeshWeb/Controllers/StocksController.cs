using System.Web.Mvc;

namespace PingMeshWeb.Controllers
{
    public class StocksController : Controller
    {

        public StocksController()
        {
        }

        // GET: Stocks
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ViewStockTable()
        {
            return View();
        }

    }
}
