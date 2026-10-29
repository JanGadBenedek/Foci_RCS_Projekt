using Foci_RCS_Projekt.Data;
using Foci_RCS_Projekt.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Foci_RCS_Projekt.Pages.MerkozesPages;

public class IndexModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public IndexModel(ApplicationDbContext context)
    {
        _context = context;
    }

    public IList<Merkozes> Merkozesek { get; set; } = new List<Merkozes>();

    [BindProperty(SupportsGet = true)]
    public string Szuro { get; set; } = "osszes";

    public async Task OnGetAsync()
    {
        var lekérdezés = _context.Merkozesek
            .AsNoTracking()
            .Include(m => m.HazaiCsapat)
            .Include(m => m.VendegCsapat)
            .AsQueryable();

        if (Szuro == "lejatszott")
        {
            lekérdezés = lekérdezés.Where(m =>
                m.HazaiGol.HasValue &&
                m.VendegGol.HasValue);
        }
        else if (Szuro == "hatralevo")
        {
            lekérdezés = lekérdezés.Where(m =>
                !m.HazaiGol.HasValue ||
                !m.VendegGol.HasValue);
        }
        else
        {
            Szuro = "osszes";
        }

        Merkozesek = await lekérdezés
            .OrderByDescending(m => m.Datum)
            .ToListAsync();
    }
}
