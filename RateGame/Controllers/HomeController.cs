using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RateGame.DAL;
using RateGame.Models;

namespace RateGame.Controllers
{
    public class HomeController : Controller
    {
        private readonly IGameRepository _gameRepository;

        public HomeController(ILogger<HomeController> logger, IGameRepository gameRepository)
        {
            _gameRepository = gameRepository;
        }

        public IActionResult RateGame()
        {
            return View();
        }

        public IActionResult GamesList(List<Game> model = null)
        {
            return View("GamesList", model);
        }

        public async Task<IActionResult> SearchByTitle(string queryString)
        {
            if(string.IsNullOrEmpty(queryString))
            {
                return GamesList();
            }

            var model = await _gameRepository.FindByTitle(queryString);

            if(!model.Any())
            {
                ViewData["Message"] = $"Didn't find any games with Title: {queryString}";
                return GamesList();
            }

            return GamesList(model);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
    
    public class GamesListViewComponent : ViewComponent
    {
        private readonly IGameRepository _gameRepository;

        public GamesListViewComponent(IGameRepository gameRepository)
        {
            _gameRepository = gameRepository;
        }

        public async Task<IViewComponentResult> InvokeAsync(List<Game> model)
        {
            if (model == null || !model.Any())
            { 
                model = await _gameRepository.Get();
            }

            return View("_List", model);
        }
    }
}
