using Foci_RCS_Projekt.Data;
using Foci_RCS_Projekt.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Foci_RCS_Projekt.Pages.MerkozesPages;

public class DeleteModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public DeleteModel(ApplicationDbContext context)
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
            .FirstOrDefaultAsync(m => m.Id == id);

        if (merkozes == null)
            return NotFound();

        _context.Merkozesek.Remove(merkozes);
        await _context.SaveChangesAsync();

        TempData["Siker"] = "A mérkőzés sikeresen törölve.";

        return RedirectToPage("./Index");
    }
}
