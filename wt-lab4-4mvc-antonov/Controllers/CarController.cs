using Microsoft.AspNetCore.Mvc;
using wt_lab4_4mvc_antonov.Models;

namespace wt_lab4_4mvc_antonov.Controllers;

public class CarController : Controller
{
    // Данные хранятся в памяти. static — чтобы сохранялись между запросами,
    // но исчезают при перезапуске приложения.
    private static readonly List<Car> Cars = new()
    {
        new Car { Id = 1, Brand = "Toyota", Model = "Camry", Year = 2021, PricePerDay = 4500, IsAvailable = true  },
        new Car { Id = 2, Brand = "BMW",    Model = "X5",    Year = 2020, PricePerDay = 8000, IsAvailable = true  },
        new Car { Id = 3, Brand = "Kia",    Model = "Rio",   Year = 2019, PricePerDay = 2500, IsAvailable = false },
        new Car { Id = 4, Brand = "Audi",   Model = "A6",    Year = 2022, PricePerDay = 6000, IsAvailable = true  },
        new Car { Id = 5, Brand = "Ford",   Model = "Focus", Year = 2018, PricePerDay = 3000, IsAvailable = true  },
    };

    private static int _nextId = 6;

    // GET: /Car/Index  или  /Car
    public IActionResult Index()
    {
        return View(Cars);
    }

    // GET: /Car/Details/1
    public IActionResult Details(int id)
    {
        var car = Cars.FirstOrDefault(c => c.Id == id);
        if (car == null)
        {
            return NotFound();  // 404
        }
        return View(car);
    }

    // GET: /Car/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: /Car/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Car car)
    {
        if (!ModelState.IsValid)
        {
            return View(car);
        }

        car.Id = _nextId++;
        Cars.Add(car);
        return RedirectToAction(nameof(Index));
    }

    // GET: /Car/Edit/1
    public IActionResult Edit(int id)
    {
        var car = Cars.FirstOrDefault(c => c.Id == id);
        if (car == null)
        {
            return NotFound();
        }
        return View(car);
    }

    // POST: /Car/Edit/1
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(int id, Car car)
    {
        if (!ModelState.IsValid)
        {
            return View(car);
        }

        var existing = Cars.FirstOrDefault(c => c.Id == id);
        if (existing == null)
        {
            return NotFound();
        }

        existing.Brand = car.Brand;
        existing.Model = car.Model;
        existing.Year = car.Year;
        existing.PricePerDay = car.PricePerDay;
        existing.IsAvailable = car.IsAvailable;

        return RedirectToAction(nameof(Index));
    }

    // POST: /Car/Delete/1
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Delete(int id)
    {
        var car = Cars.FirstOrDefault(c => c.Id == id);
        if (car != null)
        {
            Cars.Remove(car);
        }
        return RedirectToAction(nameof(Index));
    }

    // Дополнительная операция: фильтр по марке
    // GET: /Car/Filter?brand=toyota
    public IActionResult Filter(string brand)
    {
        if (string.IsNullOrWhiteSpace(brand))
        {
            return View("Index", Cars);
        }

        var result = Cars
            .Where(c => c.Brand.Contains(brand, StringComparison.OrdinalIgnoreCase))
            .ToList();

        ViewBag.FilterBrand = brand;
        return View("Index", result);
    }
}