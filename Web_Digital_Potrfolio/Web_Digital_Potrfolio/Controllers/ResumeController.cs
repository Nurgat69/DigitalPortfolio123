using Microsoft.AspNetCore.Mvc;

namespace Web_Digital_Potrfolio.Controllers;

using Data;
using ViewModels;

public class ResumeController : Controller
{
    private readonly DigitalPortfolioContext _context;

    public ResumeController(DigitalPortfolioContext context)
    {
        _context = context;
    }
}