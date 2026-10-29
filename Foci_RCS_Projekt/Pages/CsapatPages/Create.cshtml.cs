using Foci_RCS_Projekt.Data;
using Foci_RCS_Projekt.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Foci_RCS_Projekt.Pages.CsapatPages;

public class CreateModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public CreateModel(ApplicationDbContext context)
    {
        _context = context;
    }

    [BindProperty]
    public Csapat Csapat { get; set; } = new();

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (await _context.Csapatok.AnyAsync(c => c.Nev == Csapat.Nev))
        {
            ModelState.AddModelError("Csapat.Nev",
                "Ilyen nevű csapat már létezik.");
        }

        if (!ModelState.IsValid)
        {
            return Page();
        }

        _context.Csapatok.Add(Csapat);
        await _context.SaveChangesAsync();

        TempData["Siker"] = "A csapat sikeresen felvéve.";

        return RedirectToPage("./Index");
    }
}