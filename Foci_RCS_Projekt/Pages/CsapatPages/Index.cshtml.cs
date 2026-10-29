using Foci_RCS_Projekt.Data;
using Foci_RCS_Projekt.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Foci_RCS_Projekt.Pages.CsapatPages;

public class IndexModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public IndexModel(ApplicationDbContext context)
    {
        _context = context;
    }

    public IList<Csapat> Csapatok { get; set; } = new List<Csapat>();

    [BindProperty(SupportsGet = true)]
    public string? Kereses { get; set; }

    public async Task OnGetAsync()
    {
        var lekérdezés = _context.Csapatok
            .AsNoTracking()
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(Kereses))
        {
            string keresettSzoveg = Kereses.Trim();

            lekérdezés = lekérdezés.Where(c =>
                c.Nev.Contains(keresettSzoveg) ||
                (c.Varos != null && c.Varos.Contains(keresettSzoveg)));
        }

        Csapatok = await lekérdezés
            .OrderBy(c => c.Nev)
            .ToListAsync();
    }
}
