using Foci_RCS_Projekt.Data;
using Foci_RCS_Projekt.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Foci_RCS_Projekt.Pages.MerkozesPages;

public class EditModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public EditModel(ApplicationDbContext context)
    {
        _context = context;
    }

    public Merkozes Merkozes { get; set; } = new();

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null)
            return NotFound();

        var merkozes = await _context.Merkozesek
            .AsNoTracking()
            .Include(m => m.HazaiCsapat)
            .Include(m => m.VendegCsapat)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (merkozes == null)
            return NotFound();

        Merkozes = merkozes;

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int id)
    {
        var merkozes = await _context.Merkozesek
            .Include(m => m.HazaiCsapat)
            .Include(m => m.VendegCsapat)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (merkozes == null)
            return NotFound();

        // Eredmény csak a mérkőzés napján vagy utána rögzíthető.
        if (DateTime.Today < merkozes.Datum.Date)
        {
            TempData["Hiba"] =
                "Az eredmény csak a mérkőzés napján vagy utána rögzíthető.";

            return RedirectToPage("./Index");
        }

        var hazaiGol = Request.Form["Merkozes.HazaiGol"].ToString();
        var vendegGol = Request.Form["Merkozes.VendegGol"].ToString();

        if (!int.TryParse(hazaiGol, out int hazai) ||
            !int.TryParse(vendegGol, out int vendeg))
        {
            ModelState.AddModelError(
                string.Empty,
                "Mindkét csapat gólját egész számmal add meg.");

            Merkozes = merkozes;
            return Page();
        }

        if (hazai < 0 || vendeg < 0 || hazai > 99 || vendeg > 99)
        {
            ModelState.AddModelError(
                string.Empty,
                "A gólok száma 0 és 99 közötti lehet.");

            Merkozes = merkozes;
            return Page();
        }

        merkozes.HazaiGol = hazai;
        merkozes.VendegGol = vendeg;

        await _context.SaveChangesAsync();

        TempData["Siker"] = "A mérkőzés eredménye sikeresen mentve.";

        return RedirectToPage("./Index");
    }
}
