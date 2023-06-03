using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Web_Digital_Potrfolio.Data;
using Web_Digital_Potrfolio.ViewModels;

namespace Web_Digital_Potrfolio.Controllers;

using Models;

public class PortfolioController : Controller
{
    private readonly DigitalPortfolioContext _context;

    public PortfolioController(DigitalPortfolioContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Detail(long? id)
    {
        if (id == null) return NotFound();

        var portfolio = await _context.Portfolios
            .Include(p => p.User)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (portfolio == null) return NotFound();

        ViewBag.Title = portfolio.Title;

        return View(portfolio);
    }

    [Authorize]
    [HttpGet]
    public IActionResult Add() => View();

    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Add(PortfolioViewModel model, string userName)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == userName);
        if (user == null) return NotFound();

        var portfolio = new Portfolio
        {
            Title = model.Title,
            Text = model.Text,
            UrlGitHub = model.GitHubUrl,
            UserId = model.UserId
        };

        try
        {
            await _context.Portfolios.AddAsync(portfolio);
            await _context.SaveChangesAsync();
            return RedirectToAction("Detail", portfolio.Id);
        }
        catch (DbUpdateException)
        {
            throw;
        }
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> Edit(long? id)
    {
        if (id == null) return NotFound();

        var portfolio = await _context.Portfolios.FindAsync(id);
        if (portfolio == null) return NotFound();

        var portfolioVM = new PortfolioViewModel
        {
            Title = portfolio.Title,
            Text = portfolio.Text,
            GitHubUrl = portfolio.UrlGitHub,
            UserId = portfolio.UserId
        };

        ViewBag.Id = portfolio.Id;

        return View(portfolioVM);
    }

    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(PortfolioViewModel model, long id)
    {
        var portfolio = await _context.Portfolios.FindAsync(id);

        portfolio.Title = model.Title;
        portfolio.Text = model.Text;
        portfolio.UrlGitHub = model.GitHubUrl;

        try
        {
            _context.Portfolios.Update(portfolio);
            await _context.SaveChangesAsync();
            return RedirectToAction("Detail", id);
        }
        catch (DbUpdateConcurrencyException)
        {
            throw;
        }
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> Delete(long? id)
    {
        if (id == null) return NotFound();

        var portfolio = await _context.Portfolios.FindAsync(id);
        if (portfolio == null) return NotFound();

        return View(portfolio);
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Delete(long id)
    {
        var portfolio = await _context.Portfolios.FirstOrDefaultAsync(p => p.Id == id);
        
        try
        {
            _context.Portfolios.Remove(portfolio);
            await _context.SaveChangesAsync();
            return RedirectToAction("Portfolio", "Home");
        }
        catch (DbUpdateConcurrencyException)
        {
            throw;
        }
    }
}