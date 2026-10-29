using Foci_RCS_Projekt.Data;
using Foci_RCS_Projekt.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Foci_RCS_Projekt.Pages.CsapatPages;

public class EditModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public EditModel(ApplicationDbContext context)
    {
        _context = context;
    }

    [BindProperty]
    public Csapat Csapat { get; set; } = new();

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null)
            return NotFound();

        var csapat = await _context.Csapatok
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == id);

        if (csapat == null)
            return NotFound();

        Csapat = csapat;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (await _context.Csapatok.AnyAsync(c =>
            c.Nev == Csapat.Nev && c.Id != Csapat.Id))
        {
            ModelState.AddModelError("Csapat.Nev",
                "Ilyen nevű csapat már létezik.");
        }

        if (!ModelState.IsValid)
            return Page();

        var csapat = await _context.Csapatok.FindAsync(Csapat.Id);

        if (csapat == null)
            return NotFound();

        csapat.Nev = Csapat.Nev;
        csapat.Varos = Csapat.Varos;
        csapat.AlapitasEve = Csapat.AlapitasEve;

        await _context.SaveChangesAsync();

        TempData["Siker"] = "A csapat adatai sikeresen módosítva.";

        return RedirectToPage("./Index");
    }
}